using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;

public class Enemy : MonoBehaviour
{
    public AudioSource[] effectSound;
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // 타겟을 따라가는 AI 네비게이션 클래스
    public Animator anim;
    public Vector3 respawnPosition; // 리스폰 위치 저장

    [Header("Attack Position")]
    public GameObject punchPosition;
    public GameObject headPosition;

    private int health;
    public int Health { get { return health; } }
    public int maxHealth;
    protected bool isDie;
    protected bool isChase;
    protected bool isAttack;
    protected bool isNight = false;
    private bool isRespawned = false; // 리스폰 여부 체크
    public bool IsDie { get { return isDie; } }
    [Header("Die Effect")]
    [SerializeField]
    private GameObject dieEffect;
    private GameObject effectInstance;
    private ParticleSystem particleSystem;
    [Header("Item Reward")]
    public GameObject[] item;
    [Header("Experience Drop")]
    public GameObject expPrefab; // 드랍할 경험치 프리팹
    [SerializeField]
    private int expDropCount = 3; // 드랍할 경험치 갯수

    private DayAndNight dayAndNightScript;

    public GameObject player; // 플레이어 오브젝트
    public static Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z 최소값
    public static Vector3 maxBounds = new Vector3(183, 20, 148);

    // ======= Fade Parameters =======
    private Renderer fadeRenderer; // 페이드 아웃 효과를 위한 Renderer

    public Shader transparentShader = null; // 페이드 효과를 적용하기 위한 다른 셰이더
    private float fadeDuration = 3f; // 페이드 지속 시간

    // ===============================

    private void Start()
    {
        health = maxHealth;

        fadeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (fadeRenderer != null && transparentShader != null)
            fadeRenderer.material.shader = transparentShader;

        // 시작할 때 플레이어의 기본 위치를 현재 위치로 설정 (추가적인 로직이 없는 경우)
        respawnPosition = transform.position;

        GameObject directionalLight = GameObject.Find("Directional Light");
        if (directionalLight != null)
        {
            dayAndNightScript = directionalLight.GetComponent<DayAndNight>();
        }
    }

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (type != Type.Boss)
            anim.SetBool("isIdle", true);
        else if (type == Type.Boss)
            ChaseStart();
    }

    protected void Update()
    {
        bool isWithinBounds = IsPlayerWithinBounds();

        if (type == Type.Boss & isWithinBounds)
        {
            health = maxHealth;
        }

        if (isDie)
        {
            return;
        }
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }

        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight; // isNight 상태 동기화
        }
    }

    protected void FixedUpdate()
    {
        if (isChase && !isAttack)
            Targetting();

        // 낮 상태에서 몬스터가 한 번만 죽고 가만히 있도록 설정
        if (!isNight && type != Type.Boss && isChase && !isRespawned)
        {
            isRespawned = true;  // 낮 상태에서는 리스폰된 상태로 설정
            ChaseStop();
        }
        else if (!isNight && type != Type.Boss && isRespawned)
        {
            anim.SetBool("isIdle", true);
            if (nav.isActiveAndEnabled && nav.isOnNavMesh)
                nav.isStopped = true;
        }
        else if (isNight && type != Type.Boss && !isAttack)
        {
            // 밤이 되면 다시 움직이도록 설정
            isRespawned = false; // 밤 상태에서는 리스폰되지 않은 상태로 설정
            ChaseStart();
        }
        else if (type == Type.Boss && !isAttack)
        {
            ChaseStart();
        }
    }

    void ChaseStart()
    {
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
            nav.isStopped = false;
        isChase = true;
        if (type != Type.Boss)
            anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);

    }

    void ChaseStop()
    {
        isChase = false;
        StopAllCoroutines();
        Die();
    }

    public void TakeDamage(int damage)
    {
        if (isNight || type == Type.Boss)
        {
            health -= damage;
            anim.SetTrigger("doHit");
            if (health <= 0)
                Die();
        }
    }

    void Die()
    {
        /*if (effectSound.Length > 0 && effectSound[0] != null)
        {
            effectSound[0].Play();
        }
        else
        {
            Debug.LogError("효과음 배열이 비어있거나 0번째 인덱스가 null입니다. 효과음을 재생할 수 없습니다.");
        }*/

        if (type == Type.Boss)
        {
            if (effectSound.Length > 1 && effectSound[1] != null) // 배열 길이와 null 체크
            {
                effectSound[1].Play(); // 1번 효과음 재생
            }
            else
            {
                Debug.LogError("효과음 배열이 비어있거나 1번째 인덱스가 null입니다. 효과음을 재생할 수 없습니다.");
            }
        }
        else
        {
            // Boss가 아닌 경우 0번 효과음 재생
            if (effectSound.Length > 0 && effectSound[0] != null)
            {
                effectSound[0].Play();
            }
            else
            {
                Debug.LogError("효과음 배열이 비어있거나 0번째 인덱스가 null입니다. 효과음을 재생할 수 없습니다.");
            }
        }

        isDie = true;
        StopAllCoroutines();
        isChase = false;
        nav.isStopped = true; // 이동 중지
        anim.SetTrigger("doDie");
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        StartCoroutine(DieEffect());
        StartCoroutine(FadeOut());

        isRespawned = true; // 죽은 후 리스폰 상태로 설정
    }

    void Targetting()
    {
        if (!isDie && (isNight || type == Type.Boss))
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (type)
            {
                case Type.Mush:
                    targetRadius = 1f;
                    targetRange = 1.5f;
                    break;
                case Type.Cact:
                    targetRadius = 1f;
                    targetRange = 1.5f;
                    break;
                case Type.Boss:
                    targetRadius = 20f;
                    targetRange = 22f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                if (distanceToPlayer <= targetRange)
                {
                    anim.SetBool("isRun", false);
                    StartCoroutine(Attack());
                }
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        isChase = false;
        nav.isStopped = true; // 이동 중지

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y축 회전을 제한하여 평면만 회전
        transform.rotation = Quaternion.LookRotation(lookDirection);

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        isAttack = true;

        if (type != Type.Boss)
        {
            int ranAction = Random.Range(0, 5);
            if (type == Type.Mush)
            {
                switch (ranAction)
                {
                    case 0:
                    case 1:
                        anim.SetTrigger("doHeadA");
                        StartCoroutine(HeadAttack());
                        break;
                    case 2:
                    case 3:
                        anim.SetTrigger("doBodyA");
                        StartCoroutine(PunchAttack());
                        break;
                    case 4:
                        anim.SetTrigger("doSpinA");
                        StartCoroutine(HeadAttack());
                        break;
                }
            }
            else if (type == Type.Cact)
            {
                switch (ranAction)
                {
                    case 0:
                    case 1:
                    case 2:
                        anim.SetTrigger("doPunchA");
                        StartCoroutine(PunchAttack());
                        break;
                    case 3:
                    case 4:
                        anim.SetTrigger("doHeadA");
                        StartCoroutine(HeadAttack());
                        break;
                }
            }
        }

        if (type != Type.Boss)
            yield return new WaitForSeconds(3f);

        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        isAttack = false;
        nav.isStopped = false;
        isChase = true;
    }

    IEnumerator HeadAttack()
    {
        headPosition.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        headPosition.SetActive(false);
    }

    IEnumerator PunchAttack()
    {
        punchPosition.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        punchPosition.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        // �״� ����� �Ϸ�� ������ ��� (�ִϸ��̼� ���̿� �°� ����)
        yield return new WaitForSeconds(1.4f);

        // ���� �� Fade Out ȿ��
        if (fadeRenderer == null)
            Debug.LogFormat("Enemy_FadeOut : Fade Renderer가 존재하지 않습니다");
        else
        {
            if (transparentShader == null)
                Debug.LogFormat("Enemy_FadeOut : 기본 Transparent Shader가 설정되지 않았습니다");

            float elapsedTime = 0.0f;
            Color c = fadeRenderer.material.color;
            while (elapsedTime <= fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                c.a = ((fadeDuration - elapsedTime) / fadeDuration * 1.0f);
                fadeRenderer.material.color = c;
                yield return new WaitForSeconds(0.003f);
            }
        }

        
        if (isNight || type == Type.Boss)
        {
            if (item.Length <= 1)
            {
                GameObject itemReward = Instantiate(item[0], transform.position, item[0].transform.rotation);
                FloatingItem ft = itemReward.GetComponent<FloatingItem>();
                ft.Initialize(transform.position);
            }
            else
            {
                foreach (GameObject reward in item)
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                    GameObject bossReward = Instantiate(reward, transform.position + randomOffset, reward.transform.rotation);
                    FloatingItem ft = bossReward.GetComponent<FloatingItem>();
                    ft.Initialize(transform.position);
                }
            }
            for (int i = 0; i < expDropCount; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                GameObject expReward = Instantiate(expPrefab, transform.position + randomOffset, Quaternion.identity);
                FloatingItem ft = expReward.GetComponent<FloatingItem>();
                ft.Initialize(transform.position);
            }
        }
        gameObject.SetActive(false);
    }

    IEnumerator DieEffect()
    {
        yield return new WaitForSeconds(1.5f);
        if (effectInstance == null)
        {
            Vector3 adjustedPos = transform.position + new Vector3(0, 2.0f, 0);

            effectInstance = Instantiate(dieEffect, adjustedPos, Quaternion.identity);
            particleSystem = effectInstance.GetComponent<ParticleSystem>();
        }
            particleSystem?.Play();
        
    }
    public void Respawn()
    {
        // 체력을 초기화
        health = maxHealth;

        // 리스폰 위치로 이동
        transform.position = respawnPosition;

        // 투명도 초기화
        if (fadeRenderer != null)
        {
            Color c = fadeRenderer.material.color;
            c.a = 1.0f; // 불투명하게 설정
            fadeRenderer.material.color = c;
        }
        // 상태 초기화
        isDie = false;
        isChase = isNight ? true : false;
        isAttack = false;
        Collider col = GetComponent<Collider>();
        col.enabled = true;
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
            nav.isStopped = false;
    }
    private bool IsPlayerWithinBounds()
    {
        if (player == null) return false;

        Vector3 playerPosition = player.transform.position;
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.y >= minBounds.y && playerPosition.y <= maxBounds.y &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }
}
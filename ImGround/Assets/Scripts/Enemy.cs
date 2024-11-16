using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // Ÿ���� �����ϴ� AI ���� Ŭ����
    public Animator anim;
    public Vector3 respawnPosition; // ������ ��ġ ����

    [Header("Attack Position")]
    public GameObject punchPosition;
    public GameObject headPosition;

    private int health;
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

    // ======= Fade Parameters =======

    private Renderer fadeRenderer; // ���� �� Fadeout ������ ���� Renderer

    public Shader transparentShader = null; // ���� ������ ������ ������ �ٸ� ���̴�
    private float fadeDuration = 3f; // ������� �ð�

    // ===============================

    private void Start()
    {
        health = maxHealth;

        fadeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (fadeRenderer != null && transparentShader != null)
            fadeRenderer.material.shader = transparentShader;

        // ������ �� �÷��̾��� �⺻ ������ ��ġ�� ���� ��ġ�� ����(ħ�� �߰��� �� �ڵ�� ���� ����)
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
            isNight = dayAndNightScript.isNight; // isNight ���� ��������
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
        else if(!isNight && type != Type.Boss && isRespawned)
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
        health -= damage;

        anim.SetTrigger("doHit");
        if (health <= 0)
            Die();
    }

    void Die()
    {
        isDie = true;
        StopAllCoroutines();
        isChase = false;
        nav.isStopped = true; // �̵��� ���߱�
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
                    targetRadius = 10f;
                    targetRange = 12f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                // Player���� �Ÿ� ���
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                // ���� �Ÿ� �̳��� ���� ��쿡�� ����
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
        // ������ ���߰� ���� �ִϸ��̼� ����
        isChase = false;
        nav.isStopped = true; // �̵��� ���߱�

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ��
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        isAttack = true;

        if (type != Type.Boss) // Boss�� �ƴ� �ٸ� Ÿ���� �� ����
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

        // ������ ������ ���� ������ �� �ٽ� ������ ����
        if (type != Type.Boss)
            yield return new WaitForSeconds(3f); // �Ϲ� ������ ��� ���� ������

        // �÷��̾���� �Ÿ��� �ָ� ������ �簳
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // �޸��� �ִϸ��̼� Ȱ��ȭ
        }
        else
        {
            anim.SetBool("isRun", false); // ���� ���� ����
        }

        // ������ ������ �ٽ� ������ ����
        isAttack = false;
        nav.isStopped = false; // �ٽ� �̵� �����ϰ� ����
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
            Debug.LogFormat("Enemy_FadeOut : Fade Renderer�� �������� ����");
        else
        {
            if (transparentShader == null)
                Debug.LogFormat("Enemy_FadeOut : �⺻ Transparent Shader�� ����ȭ ó��");

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

        gameObject.SetActive(false);
        if (isNight || type == Type.Boss)
        {
            if (item.Length <= 1)
                Instantiate(item[0], transform.position, item[0].transform.rotation);
            else
            {
                foreach (GameObject reward in item)
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                    Instantiate(reward, transform.position + randomOffset, reward.transform.rotation);
                }
            }
            for (int i = 0; i < expDropCount; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
                Instantiate(expPrefab, transform.position + randomOffset, Quaternion.identity);
            }
        }
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
        // ü���� �ʱ�ȭ
        health = maxHealth;

        // ������ ��ġ�� �̵�
        transform.position = respawnPosition;

        // ������ ����
        if (fadeRenderer != null)
        {
            Color c = fadeRenderer.material.color;
            c.a = 1.0f; // ������ ����
            fadeRenderer.material.color = c;
        }
        // ��� ���� ����
        isDie = false;
        isChase = isNight ? true : false;
        isAttack = false;
        Collider col = GetComponent<Collider>();
        col.enabled = true;
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
            nav.isStopped = false;
    }
}

/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // 타켓을 추적하는 AI 관련 클래스
    public Animator anim;
    public Vector3 respawnPosition; // 리스폰 위치 설정

    [Header("Attack Position")]
    public GameObject punchPosition;
    public GameObject headPosition;

    private int health;
    public int maxHealth;
    protected bool isDie;
    protected bool isChase;
    protected bool isAttack;
    public bool isNight = false;
    private bool isDeadCooldown = false; // 사망 후 5초 동안의 쿨다운
    public bool IsDie {  get { return isDie; } }
    [Header("Item Reward")]
    public GameObject item;
    
    // ======= Fade Parameters =======

    private Renderer fadeRenderer; // 죽은 후 Fadeout 적용을 위한 Renderer

    public Shader transparentShader = null; // 알파 렌더링 문제시 적용할 다른 쉐이더
    private float fadeDuration = 3f; // 사라지는 시간

    // ===============================

    private void Start()
    {
        health = maxHealth;

        fadeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (fadeRenderer != null && transparentShader != null)
            fadeRenderer.material.shader = transparentShader;

        // 시작할 때 플레이어의 기본 리스폰 위치를 현재 위치로 설정(침대 추가시 이 코드는 삭제 예정)
        respawnPosition = transform.position;
    }
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if(type != Type.Boss)
            anim.SetBool("isIdle", true);
        else if(type == Type.Boss)
            ChaseStart();
    }


    protected void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
        if (isDie)
        {
            return;
        }
    }
    protected void FixedUpdate()
    {
        if(isChase && !isAttack)
            Targetting();
        if (!isNight && type != Type.Boss)
            ChaseStop();
        else if(isNight && type != Type.Boss && !isAttack)
        {
            ChaseStart();
        }
    }
    void ChaseStart()
    {
        nav.isStopped = false;
        isChase = true;
        if(type != Type.Boss)
            anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);
    }
    void ChaseStop()
    {
        nav.isStopped = true;
        isChase = false;
        StopAllCoroutines();
        anim.SetBool("isIdle", true);
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
        nav.isStopped = true; // 이동을 멈추기
        anim.SetTrigger("doDie");
        StartCoroutine(FadeOut());
    }

    void Targetting()
    {
        if (!isDie)
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
                    targetRadius = 6f;
                    targetRange = 6f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                // Player와의 거리 계산
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                // 일정 거리 이내에 있을 경우에만 공격
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
        // 추적을 멈추고 공격 애니메이션 실행
        isChase = false;
        nav.isStopped = true; // 이동을 멈추기

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y축 회전을 방지하여 수평으로만 회전
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        isAttack = true;
        
        if(type != Type.Boss) // Boss가 아닌 다른 타입의 적 공격
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

        // 공격이 끝나면 일정 딜레이 후 다시 추적을 시작
        if (type != Type.Boss)
            yield return new WaitForSeconds(3f); // 일반 몬스터의 경우 공격 딜레이

        // 플레이어와의 거리가 멀면 추적을 재개
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // 달리는 애니메이션 활성화
        }
        else
        {
            anim.SetBool("isRun", false); // 정지 상태 유지
        }

        // 공격이 끝나면 다시 추적을 시작
        isAttack = false;
        nav.isStopped = false; // 다시 이동 가능하게 설정
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
        // 죽는 모션이 완료될 때까지 대기 (애니메이션 길이에 맞게 조정)
        yield return new WaitForSeconds(1.4f);

        // 죽은 후 Fade Out 효과
        if (fadeRenderer == null)
            Debug.LogFormat("Enemy_FadeOut : Fade Renderer를 설정하지 못함");
        else
        {
            if (transparentShader == null)
                Debug.LogFormat("Enemy_FadeOut : 기본 Transparent Shader로 투명화 처리");

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
        GameObject reward = Instantiate(item, transform.position, Quaternion.identity);
    }

    public void Respawn()
    {
        // 체력을 초기화
        health = maxHealth;

        // 리스폰 위치로 이동
        transform.position = respawnPosition;

        // 투명도 복원
        if (fadeRenderer != null)
        {
            Color c = fadeRenderer.material.color;
            c.a = 1.0f; // 투명도 복원
            fadeRenderer.material.color = c;
        }
        // 사망 상태 해제
        isDie = false;
        isChase = true;
        isAttack = false;
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
            nav.isStopped = false; 
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Mush, Cact, Boss };
    public Type type;
    public Transform target;
    public NavMeshAgent nav; // 타켓을 추적하는 AI 관련 클래스
    public Animator anim;
    public Vector3 respawnPosition; // 리스폰 위치 설정

    [Header("Attack Position")]
    public GameObject punchPosition;
    public GameObject headPosition;

    private int health;
    public int maxHealth;
    protected bool isDie;
    protected bool isChase;
    protected bool isAttack;
    protected bool isNight = false;
    private bool isDeadCooldown = false; // 사망 후 5초 동안의 쿨다운
    public bool IsDie { get { return isDie; } }
    [Header("Item Reward")]
    public GameObject item;

    private DayAndNight dayAndNightScript;

    // ======= Fade Parameters =======

    private Renderer fadeRenderer; // 죽은 후 Fadeout 적용을 위한 Renderer

    public Shader transparentShader = null; // 알파 렌더링 문제시 적용할 다른 쉐이더
    private float fadeDuration = 3f; // 사라지는 시간

    // ===============================

    private void Start()
    {
        health = maxHealth;

        fadeRenderer = gameObject.GetComponentInChildren<Renderer>();
        if (fadeRenderer != null && transparentShader != null)
            fadeRenderer.material.shader = transparentShader;

        // 시작할 때 플레이어의 기본 리스폰 위치를 현재 위치로 설정(침대 추가시 이 코드는 삭제 예정)
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
       if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
        if (isDie)
        {
            return;
        }
        
        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight; // isNight 변수 가져오기
        }

        // 밤낮 상태에 따라 오브젝트의 동작을 활성화/비활성화
        if (isNight)
        {
            if (!nav.isStopped && !isDie)
            {
                nav.enabled = true;
                anim.enabled = true;
            }
        }
        else
        {
            nav.enabled = false;
            anim.SetBool("isIdle", true);
            anim.enabled = false;
        }
    }
    protected void FixedUpdate()
    {
        if (isChase && !isAttack)
            Targetting();
        if (!isNight && type != Type.Boss)
            ChaseStop();
        else if (isNight && type != Type.Boss && !isAttack)
        {
            ChaseStart();
        }
    }
    void ChaseStart()
    {
        nav.isStopped = false;
        isChase = true;
        if (type != Type.Boss)
            anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);
    }
    void ChaseStop()
    {
        /*nav.isStopped = true;
        isChase = false;
        StopAllCoroutines();
        anim.SetBool("isIdle", true);*/
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
        {
            nav.isStopped = true;
        }
        isChase = false;
        StopAllCoroutines();
        anim.SetBool("isIdle", true);

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
        nav.isStopped = true; // 이동을 멈추기
        anim.SetTrigger("doDie");
        StartCoroutine(FadeOut());
    }

    void Targetting()
    {
        if (!isDie)
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
                    targetRadius = 6f;
                    targetRange = 6f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                // Player와의 거리 계산
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                // 일정 거리 이내에 있을 경우에만 공격
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
        // 추적을 멈추고 공격 애니메이션 실행
        isChase = false;
        nav.isStopped = true; // 이동을 멈추기

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; // Y축 회전을 방지하여 수평으로만 회전
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        isAttack = true;

        if (type != Type.Boss) // Boss가 아닌 다른 타입의 적 공격
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

        // 공격이 끝나면 일정 딜레이 후 다시 추적을 시작
        if (type != Type.Boss)
            yield return new WaitForSeconds(3f); // 일반 몬스터의 경우 공격 딜레이

        // 플레이어와의 거리가 멀면 추적을 재개
        if (distanceToPlayer > nav.stoppingDistance)
        {
            anim.SetBool("isRun", true); // 달리는 애니메이션 활성화
        }
        else
        {
            anim.SetBool("isRun", false); // 정지 상태 유지
        }

        // 공격이 끝나면 다시 추적을 시작
        isAttack = false;
        nav.isStopped = false; // 다시 이동 가능하게 설정
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
        // 죽는 모션이 완료될 때까지 대기 (애니메이션 길이에 맞게 조정)
        yield return new WaitForSeconds(1.4f);

        // 죽은 후 Fade Out 효과
        if (fadeRenderer == null)
            Debug.LogFormat("Enemy_FadeOut : Fade Renderer를 설정하지 못함");
        else
        {
            if (transparentShader == null)
                Debug.LogFormat("Enemy_FadeOut : 기본 Transparent Shader로 투명화 처리");

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
        GameObject reward = Instantiate(item, transform.position, Quaternion.identity);
    }

    public void Respawn()
    {
        // 체력을 초기화
        health = maxHealth;

        // 리스폰 위치로 이동
        transform.position = respawnPosition;

        // 투명도 복원
        if (fadeRenderer != null)
        {
            Color c = fadeRenderer.material.color;
            c.a = 1.0f; // 투명도 복원
            fadeRenderer.material.color = c;
        }
        // 사망 상태 해제
        isDie = false;
        isChase = true;
        isAttack = false;
        if (nav.isActiveAndEnabled && nav.isOnNavMesh)
            nav.isStopped = false;
    }
}

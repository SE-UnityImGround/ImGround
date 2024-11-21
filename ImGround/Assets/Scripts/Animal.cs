using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private int maxHealth = 5;
    public int health;
    [SerializeField]
    private NavMeshAgent nav;

    public float patrolRadius = 5.0f; // 순찰 범위
    public float patrolWaitTime = 3.0f; // 각 순찰 지점에서 대기하는 시간
    protected float patrolWaitTimer;

    protected bool surprised = false; // 닭 전용 플래그
    protected bool flying = false; // 공중에 날아다니는 동물(곤충) 전용
    private bool isDie = false;
    [SerializeField]
    private bool isHuntAble = true;

    protected NavMeshAgent navAgent;
    private Vector3 patrolTarget;
    public Animator anim;
    public Transform target;
    private Renderer renderer;
    private Color originalColor; // 원래 색상

    [Header("Item Reward")]
    public GameObject item;

    [Header("Experience Drop")]
    public GameObject expPrefab; // 드랍할 경험치 프리팹
    [SerializeField]
    private int expDropCount = 3; // 드랍할 경험치 갯수

    void Awake()
    {
        health = maxHealth;
        renderer = GetComponentInChildren<Renderer>();
        // 원래 색상 저장
        originalColor = renderer.material.color;
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        SetNewRandomPatrolTarget();
    }

    protected void Update()
    {
        if (!isDie)
        {
            Patrol();
            if (!surprised && !flying)
                LookAt();
        }
    }

    void Patrol()
    {
        // 순찰 중 목적지에 도착했는지 확인
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            anim.SetBool("isWalk", false);
            patrolWaitTimer += Time.deltaTime;

            // 대기 시간이 끝나면 새로운 랜덤 목적지 설정
            if (patrolWaitTimer >= patrolWaitTime)
            {

                SetNewRandomPatrolTarget();
                patrolWaitTimer = 0.0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isHuntAble)
        {
            health -= damage;
            if (health > 0)
                StartCoroutine(Damaged());
            else
                StartCoroutine(Die());
        }
    }

    public void StartDie()
    {
        StartCoroutine(Die());
    }
    IEnumerator Damaged()
    {
        // 색상을 빨간색으로 변경
        renderer.material.color = Color.red;

        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(0.2f);

        // 원래 색상으로 복원
        renderer.material.color = originalColor;
    }
    IEnumerator Die()
    {
        float currentZAngle = transform.eulerAngles.z;
        isDie = true;
        nav.enabled = false;
        anim.SetBool("isWalk", false);

        Color originalColor = renderer.material.color;
        Color targetColor = Color.red;

        float elapsedTime = 0f;
        float duration = 1f; // 색상이 변하는 시간 (1초 동안)

        while (elapsedTime < duration)
        {
            // 점진적으로 붉은색으로 변하게 만듦
            if (renderer != null) // 렌더러가 존재하는지 확인
            {
                renderer.material.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
            }

            // 회전 각도 점진적으로 변경
            float t = elapsedTime / duration;
            float newZAngle = Mathf.Lerp(currentZAngle, 90f, t); // Lerp를 사용해 점진적으로 회전
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZAngle);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 회전을 최종적으로 90도에 맞춤
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 90f);

        // 최종 색상 붉은색으로 고정
        if (renderer != null)
        {
            renderer.material.color = targetColor;
        }
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        if(item != null)
            Instantiate(item, transform.position, item.transform.rotation);


        yield return new WaitForSeconds(3f);
        GameObject reward = Instantiate(item, transform.position, item.transform.rotation);
        FloatingItem floatingItem = reward.AddComponent<FloatingItem>();
        floatingItem.Initialize(transform.position);
        Destroy(gameObject);


        for (int i = 0; i < expDropCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            Instantiate(expPrefab, transform.position + randomOffset, Quaternion.identity);
        }
    }



    // 랜덤한 위치를 순찰 지점으로 설정
    protected void SetNewRandomPatrolTarget()
    {

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            patrolTarget = hit.position;
            navAgent.SetDestination(patrolTarget);
        }
        anim.SetBool("isWalk", true);
    }

    void LookAt()
    {
        float targetRadius = 2f;
        float targetRange = 4f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0)
        {
            // Player와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            // 일정 거리 이내에 있을 경우에만 플레이어를 향함
            if (distanceToPlayer <= targetRange)
            {
                navAgent.isStopped = true;
                Vector3 lookDirection = (target.position - transform.position).normalized;
                lookDirection.y = 0; // Y축 회전을 방지하여 수평으로만 회전

                // 목표 회전 값 계산
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                // 현재 회전 값에서 목표 회전 값으로 부드럽게 회전 (Slerp 사용)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                anim.SetBool("isWalk", false);
            }
            else
            {
                // 플레이어가 범위를 벗어나면 NavMeshAgent 다시 활성화
                navAgent.isStopped = false;
                anim.SetBool("isWalk", true);
            }
        }
    }

    public virtual void Respawn()
    {
        health = maxHealth;
        isDie = false;
        nav.enabled = true;
        anim.SetBool("isWalk", true);
        renderer.material.color = originalColor;
    }
}
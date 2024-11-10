using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

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

    protected NavMeshAgent navAgent;
    private Vector3 patrolTarget;
    public Animator anim;
    public Transform target;

    [Header("Item Reward")]
    public GameObject item;

    void Awake()
    {
        health = maxHealth;
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
        //navAgent.remainingDistance < 0.5f
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
        health -= damage;
        if (health <= 0)
            StartCoroutine(Die());
    }

    public void StartDie()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        float currentZAngle = transform.eulerAngles.z;
        isDie = true;
        nav.enabled = false;

        Renderer renderer = GetComponentInChildren<Renderer>();
        Color originalColor = renderer.material.color;
        Color targetColor = Color.red;

        float elapsedTime = 0f;
        float duration = 2f; // 색상이 변하는 시간 (2초 동안)

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

        yield return new WaitForSeconds(3f);
        GameObject reward = Instantiate(item, transform.position, item.transform.rotation);
        Destroy(gameObject);
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

}



/*using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Animal : MonoBehaviour
{
    public float patrolRadius = 5.0f; // 순찰 범위
    public float patrolWaitTime = 3.0f; // 각 순찰 지점에서 대기하는 시간
    private float patrolWaitTimer;

    protected bool surprised = false; // 닭 전용 플래그
    protected bool flying = false; // 공중에 날아다니는 동물(곤충) 전용

    protected NavMeshAgent navAgent;
    public Vector3 patrolTarget;
    public Animator anim;
    public Transform target;

    void Awake()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        SetNewRandomPatrolTarget();
    }

    protected void Update()
    {
        Patrol();
        if (!surprised && !flying)
            LookAt();
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

    // 랜덤한 위치를 순찰 지점으로 설정
    protected virtual void SetNewRandomPatrolTarget()
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

}
*/

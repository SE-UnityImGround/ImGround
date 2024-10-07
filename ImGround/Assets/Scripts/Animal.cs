using UnityEngine;
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
    private Vector3 patrolTarget;
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
        if(!surprised && !flying)
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


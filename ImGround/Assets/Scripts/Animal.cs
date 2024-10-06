using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public float patrolRadius = 5.0f; // 순찰 범위
    public float patrolWaitTime = 3.0f; // 각 순찰 지점에서 대기하는 시간
    private float patrolWaitTimer;

    private NavMeshAgent navAgent;
    private Vector3 patrolTarget;
    public Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        SetNewRandomPatrolTarget();
    }

    void Update()
    {
        Patrol();
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
    void SetNewRandomPatrolTarget()
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
}


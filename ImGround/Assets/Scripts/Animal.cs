using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public float patrolRadius = 5.0f; // ���� ����
    public float patrolWaitTime = 3.0f; // �� ���� �������� ����ϴ� �ð�
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
        // ���� �� �������� �����ߴ��� Ȯ��
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            anim.SetBool("isWalk", false);
            patrolWaitTimer += Time.deltaTime;

            // ��� �ð��� ������ ���ο� ���� ������ ����
            if (patrolWaitTimer >= patrolWaitTime)
            {
                
                SetNewRandomPatrolTarget();                
                patrolWaitTimer = 0.0f;
            }
        }
    }

    // ������ ��ġ�� ���� �������� ����
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


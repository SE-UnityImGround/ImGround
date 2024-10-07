using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Animal : MonoBehaviour
{
    public float patrolRadius = 5.0f; // ���� ����
    public float patrolWaitTime = 3.0f; // �� ���� �������� ����ϴ� �ð�
    private float patrolWaitTimer;

    protected bool surprised = false; // �� ���� �÷���
    protected bool flying = false; // ���߿� ���ƴٴϴ� ����(����) ����

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
            // Player���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            // ���� �Ÿ� �̳��� ���� ��쿡�� �÷��̾ ����
            if (distanceToPlayer <= targetRange)
            {
                navAgent.isStopped = true;
                Vector3 lookDirection = (target.position - transform.position).normalized;
                lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� �������θ� ȸ��

                // ��ǥ ȸ�� �� ���
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                // ���� ȸ�� ������ ��ǥ ȸ�� ������ �ε巴�� ȸ�� (Slerp ���)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                anim.SetBool("isWalk", false);
            }
            else
            {
                // �÷��̾ ������ ����� NavMeshAgent �ٽ� Ȱ��ȭ
                navAgent.isStopped = false;
                anim.SetBool("isWalk", true);
            }
        }
    }

}


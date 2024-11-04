using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        flying = true;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Vector3 targetPosition = navAgent.transform.position;
        targetPosition.y = 6;
        transform.position = targetPosition;
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Animal
{
    private float targetY; // ��ǥ y ��

    // Start is called before the first frame update
    void Start()
    {
        flying = true;
        targetY = transform.position.y; // �ʱ� y �� ����
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Vector3 targetPosition = navAgent.transform.position;

        // ���� �������� ���ο� ��ǥ y ���� ����
        if (Mathf.Abs(transform.position.y + targetY) < 0.01f)
        {
            targetY += Random.Range(-0.5f, 0.5f); // -0.5 ~ 0.5 ������ ���� �� �߰�
            targetY = Mathf.Clamp(targetY, 1, 30); // y ���� 1���� 30 ���̷� ���ѵǵ��� ����
        }

        // ���� y ���� ��ǥ y ������ �ڿ������� ����
        targetPosition.y = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * 2); // 2�� ���� �ӵ� ����

        transform.position = targetPosition;
    }
}

*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Animal
{
    private Vector3 targetPosition;
    private float moveSpeed = 2.0f; // �̵� �ӵ�

    // Start is called before the first frame update
    void Start()
    {
        flying = true;
        SetNewRandomPatrolTarget(); // �ʱ� ��ǥ ���� ����
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        // ��ǥ ������ �����ߴ��� Ȯ���Ͽ� ���ο� ��ǥ ����
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetNewRandomPatrolTarget();
        }

        // ��ǥ �������� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    protected override void SetNewRandomPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        // y�� ���� �����ϰ� �����Ͽ� ���Ʒ��� �����̰� �� (��: 2 ~ 10 ����)
        randomDirection.y = Random.Range(2f, 10f);

        targetPosition = randomDirection;
    }
}

*/

/*using UnityEngine;
using UnityEngine.AI;

public class Butterfly : MonoBehaviour
{
    public float range = 10f; // ���� ��ġ�� ������ ����
    private NavMeshAgent agent;

    void Start()
    {
        // NavMeshAgent ������Ʈ�� ��������
        agent = GetComponent<NavMeshAgent>();
        SetNewRandomDestination();
    }

    void Update()
    {
        // ������Ʈ�� ��ǥ ������ ���� �������� �� ���ο� ��ġ ����
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                SetNewRandomDestination();
            }
        }
    }

    void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position; // ���� ��ġ�� �������� ���� ��ġ �߰�

        NavMeshHit hit;
        // NavMesh ������ ��ȿ�� ��ġ�� ã��
        if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
        {
            // y ���� ���Ե� ��ȿ�� ��ġ�� ����
            agent.SetDestination(hit.position);
        }
    }
}
*/
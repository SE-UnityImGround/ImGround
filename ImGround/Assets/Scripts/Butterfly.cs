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
    private float targetY; // 목표 y 값

    // Start is called before the first frame update
    void Start()
    {
        flying = true;
        targetY = transform.position.y; // 초기 y 값 설정
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Vector3 targetPosition = navAgent.transform.position;

        // 일정 간격으로 새로운 목표 y 값을 설정
        if (Mathf.Abs(transform.position.y + targetY) < 0.01f)
        {
            targetY += Random.Range(-0.5f, 0.5f); // -0.5 ~ 0.5 사이의 랜덤 값 추가
            targetY = Mathf.Clamp(targetY, 1, 30); // y 값이 1에서 30 사이로 제한되도록 설정
        }

        // 현재 y 값을 목표 y 값으로 자연스럽게 보간
        targetPosition.y = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * 2); // 2는 보간 속도 조절

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
    private float moveSpeed = 2.0f; // 이동 속도

    // Start is called before the first frame update
    void Start()
    {
        flying = true;
        SetNewRandomPatrolTarget(); // 초기 목표 지점 설정
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        // 목표 지점에 도달했는지 확인하여 새로운 목표 설정
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetNewRandomPatrolTarget();
        }

        // 목표 지점으로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    protected override void SetNewRandomPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        // y축 값을 랜덤하게 설정하여 위아래로 움직이게 함 (예: 2 ~ 10 사이)
        randomDirection.y = Random.Range(2f, 10f);

        targetPosition = randomDirection;
    }
}

*/

/*using UnityEngine;
using UnityEngine.AI;

public class Butterfly : MonoBehaviour
{
    public float range = 10f; // 랜덤 위치를 지정할 범위
    private NavMeshAgent agent;

    void Start()
    {
        // NavMeshAgent 컴포넌트를 가져오기
        agent = GetComponent<NavMeshAgent>();
        SetNewRandomDestination();
    }

    void Update()
    {
        // 에이전트가 목표 지점에 거의 도달했을 때 새로운 위치 설정
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
        randomDirection += transform.position; // 현재 위치를 기준으로 랜덤 위치 추가

        NavMeshHit hit;
        // NavMesh 내에서 유효한 위치를 찾기
        if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
        {
            // y 값이 포함된 유효한 위치를 설정
            agent.SetDestination(hit.position);
        }
    }
}
*/
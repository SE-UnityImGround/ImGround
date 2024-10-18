using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public Vector3 Origin;
    public bool SetOriginAsStartPos; // 만약 True이면 Origin은 오브젝트의 처음 위치로 설정됩니다.
    public NPCType type;
    public float Radius; // 랜덤 위치 범위
    public Vector3 PlayerLookOffset;

    /* ===============================================================================
     *  참고 : 각 컴포넌트는 인스펙터에서 입력받지 않고, 직접 검색하여 찾도록 구현함!
    // =============================================================================== */
    private Animator animator; // 이동 애니메이션 관리 컴포넌트
    private NavMeshAgent agent; // 이동 AI를 담당할 에이전트

    private float timerExpireDuration; // 랜덤 위치 이동의 시간간격
    private const float MAX_DURATION = 5.0f;
    private const float MIN_DURATION = 30.0f;
    private float stdTime = 0.0f; // 타이머용 변수
    private Vector3 currentTarget; // 현재 랜덤위치

    private Transform head;
    private Quaternion lastHeadRotation;
    private Quaternion deltaAngle;

    // 애니메이션 파라미터 이름값
    private class animationParameters
    {
        static public string isWalk = "IsWalking";
    }

    // Start is called before the first frame update
    void Start()
    {
        // 필요 객체들을 스크립트로 입력받지 않고, 직접 찾도록 구현
        if ((animator = gameObject.GetComponent<Animator>()) == null)
            Debug.LogErrorFormat("gameObject {0}의 Animator를 찾을 수 없습니다!!", gameObject.name);
        if ((agent = gameObject.GetComponent<NavMeshAgent>()) == null)
            Debug.LogErrorFormat("gameObject {0}의 NavMeshAgent를 찾을 수 없습니다!!", gameObject.name);

        agent.updateRotation = false; // NPC 방향 회전은 NavMeshAgent를 이용하니 너무 부자연스러워 직접 회전값을 사용하도록 세팅

        timerExpireDuration = Random.Range(MIN_DURATION, MAX_DURATION);
        stdTime = Time.time - timerExpireDuration; // 초기 타이머 세팅 (참고 : 즉시 시작하도록 세팅됨)

        if (SetOriginAsStartPos)
            Origin = transform.position;

        if ((head = findChildRecursively(transform, "Head")) == null)
            Debug.LogErrorFormat("gameObject {0}의 Head를 찾을 수 없습니다!!", gameObject.name);
        else
        {
            deltaAngle = Quaternion.Euler(head.eulerAngles - transform.eulerAngles);
            lastHeadRotation = head.rotation;
        }
    }

    private Transform findChildRecursively(Transform root, string name)
    {
        Queue<Transform> childs = new Queue<Transform>();
        childs.Enqueue(root);
        Transform child;
        Transform parent;

        do
        {
            for (int cnt = childs.Count; cnt > 0; cnt--)
            {
                parent = childs.Dequeue();
                for (int i = 0; i < parent.childCount; i++)
                {
                    child = parent.GetChild(i);
                    if (child.name.CompareTo(name) == 0)
                    {
                        childs.Clear();
                        return child;
                    }
                    else
                        childs.Enqueue(child);
                }
            }
        } while (childs.Count > 0);

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        setPosition();
        move();
    }

    void LateUpdate()
    {
        lookAtPlayer();
    }

    private Vector3 findRandomPos(Vector3 origin, float radius)
    {
        NavMeshHit hit;
        for (int i = 0; i < 30; i++)
            if (NavMesh.SamplePosition(
                    origin + (Random.insideUnitSphere * radius),
                    out hit,
                    5.0f, NavMesh.AllAreas)
                )
                return hit.position;
        return Vector3.zero;
    }

    private void setPosition()
    {
        if (Time.time - stdTime >= timerExpireDuration)
        {
            currentTarget = findRandomPos(Origin, Radius);
            agent.SetDestination(currentTarget);

            stdTime = Time.time;
            timerExpireDuration = float.MaxValue;
        }
    }

    private void move()
    {
        if ((transform.position - agent.steeringTarget).magnitude > 0.001f)
        {
            animator.SetBool(animationParameters.isWalk, true);

            // 회전은 직접 계산하여 적용하도록 변경
            if (agent.steeringTarget != transform.position)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(agent.steeringTarget - transform.position), 10.0f * Time.deltaTime);
        }
        else
            animator.SetBool(animationParameters.isWalk, false);
    }

    private void lookAtPlayer()
    {
        GameObject player = findPlayerInDistance(5.0f);
        if (head != null && player != null)
        {
            Quaternion lookAngle = Quaternion.LookRotation(player.transform.position + PlayerLookOffset - head.transform.position);

            Vector3 angleAmount = lookAngle.eulerAngles - transform.eulerAngles;
            float x = angleAmount.x % 360.0f;
            x = (x >= 180.0f ? x - 360.0f :
                (x <= -180.0f ? x + 360.0f : x)); // -180.0 ~ 180.0 범위로 정규화
            float y = angleAmount.y % 360.0f;
            y = (y >= 180.0f ? y - 360.0f :
                (y <= -180.0f ? y + 360.0f : y)); // -180.0 ~ 180.0 범위로 정규화

            if (player != null 
                && (Mathf.Abs(y) <= 70.0f && x >= -45.0f)) // 좌우 80도, 아래로 45도 까지의 시선만 적용
            {
                lastHeadRotation = Quaternion.Lerp(
                    lastHeadRotation,
                    lookAngle * deltaAngle,
                    1.0f / (0.15f) * Time.deltaTime); // 괄호 내 : a->b로 가는데 걸리는 총 시간(초)
            }
            else
            {
                lastHeadRotation = Quaternion.Lerp(
                       lastHeadRotation,
                       transform.rotation * deltaAngle,
                       1.0f / (0.5f) * Time.deltaTime); // 괄호 내 : a->b로 가는데 걸리는 총 시간(초)
            }

            head.rotation = lastHeadRotation;
        }
    }

    private GameObject findPlayerInDistance(float distance)
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, distance))
            if (c.name.CompareTo("Player") == 0)
                return c.gameObject;
        return null;
    }
}

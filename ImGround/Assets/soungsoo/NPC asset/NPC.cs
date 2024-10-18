using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public Vector3 Origin;
    public bool SetOriginAsStartPos; // ���� True�̸� Origin�� ������Ʈ�� ó�� ��ġ�� �����˴ϴ�.
    public NPCType type;
    public float Radius; // ���� ��ġ ����
    public Vector3 PlayerLookOffset;

    /* ===============================================================================
     *  ���� : �� ������Ʈ�� �ν����Ϳ��� �Է¹��� �ʰ�, ���� �˻��Ͽ� ã���� ������!
    // =============================================================================== */
    private Animator animator; // �̵� �ִϸ��̼� ���� ������Ʈ
    private NavMeshAgent agent; // �̵� AI�� ����� ������Ʈ

    private float timerExpireDuration; // ���� ��ġ �̵��� �ð�����
    private const float MAX_DURATION = 5.0f;
    private const float MIN_DURATION = 30.0f;
    private float stdTime = 0.0f; // Ÿ�̸ӿ� ����
    private Vector3 currentTarget; // ���� ������ġ

    private Transform head;
    private Quaternion lastHeadRotation;
    private Quaternion deltaAngle;

    // �ִϸ��̼� �Ķ���� �̸���
    private class animationParameters
    {
        static public string isWalk = "IsWalking";
    }

    // Start is called before the first frame update
    void Start()
    {
        // �ʿ� ��ü���� ��ũ��Ʈ�� �Է¹��� �ʰ�, ���� ã���� ����
        if ((animator = gameObject.GetComponent<Animator>()) == null)
            Debug.LogErrorFormat("gameObject {0}�� Animator�� ã�� �� �����ϴ�!!", gameObject.name);
        if ((agent = gameObject.GetComponent<NavMeshAgent>()) == null)
            Debug.LogErrorFormat("gameObject {0}�� NavMeshAgent�� ã�� �� �����ϴ�!!", gameObject.name);

        agent.updateRotation = false; // NPC ���� ȸ���� NavMeshAgent�� �̿��ϴ� �ʹ� ���ڿ������� ���� ȸ������ ����ϵ��� ����

        timerExpireDuration = Random.Range(MIN_DURATION, MAX_DURATION);
        stdTime = Time.time - timerExpireDuration; // �ʱ� Ÿ�̸� ���� (���� : ��� �����ϵ��� ���õ�)

        if (SetOriginAsStartPos)
            Origin = transform.position;

        if ((head = findChildRecursively(transform, "Head")) == null)
            Debug.LogErrorFormat("gameObject {0}�� Head�� ã�� �� �����ϴ�!!", gameObject.name);
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

            // ȸ���� ���� ����Ͽ� �����ϵ��� ����
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
                (x <= -180.0f ? x + 360.0f : x)); // -180.0 ~ 180.0 ������ ����ȭ
            float y = angleAmount.y % 360.0f;
            y = (y >= 180.0f ? y - 360.0f :
                (y <= -180.0f ? y + 360.0f : y)); // -180.0 ~ 180.0 ������ ����ȭ

            if (player != null 
                && (Mathf.Abs(y) <= 70.0f && x >= -45.0f)) // �¿� 80��, �Ʒ��� 45�� ������ �ü��� ����
            {
                lastHeadRotation = Quaternion.Lerp(
                    lastHeadRotation,
                    lookAngle * deltaAngle,
                    1.0f / (0.15f) * Time.deltaTime); // ��ȣ �� : a->b�� ���µ� �ɸ��� �� �ð�(��)
            }
            else
            {
                lastHeadRotation = Quaternion.Lerp(
                       lastHeadRotation,
                       transform.rotation * deltaAngle,
                       1.0f / (0.5f) * Time.deltaTime); // ��ȣ �� : a->b�� ���µ� �ɸ��� �� �ð�(��)
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

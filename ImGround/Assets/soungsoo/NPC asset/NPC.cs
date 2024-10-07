using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public Vector3 Origin;
    public bool SetOriginAsStartPos; // ���� True�̸� Origin�� ������Ʈ�� ó�� ��ġ�� �����˴ϴ�.
    public float Radius; // ���� ��ġ ����

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
    }

    // Update is called once per frame
    void Update()
    {
        setPosition();
        move();
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
            timerExpireDuration = Random.Range(MIN_DURATION, MAX_DURATION);
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
}

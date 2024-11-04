using System;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

/// <summary>
/// NPC�� �̵��� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class NpcMover
{
    private GameObject npcObject;
    private NavMeshAgent agent; // �̵� AI�� ����� NavMeshAgent

    private float timerDuration; // Ÿ�̸� �ð�
    private float stdTime; // ���� �ð�
    private float maxDuration;
    private float minDuration;

    private Vector3 currentTarget; // ���� ������ġ
    private static readonly float IN_POSITION_RADIUS_SQUARE = 0.2f;

    private NpcMoveState currentState = NpcMoveState.IDLE;

    /// <summary>
    /// NPC�� �����̱� ������ �� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    public delegate void onMoveStateChangedEvent(NpcMoveState state);
    public onMoveStateChangedEvent onMoveStateChangedEventHandler;

    /// <summary>
    /// NPC�� �̵��� �����ϴ� Ŭ�����Դϴ�.
    /// </summary>
    /// <param name="npcObject">NPC ���ӿ�����Ʈ</param>
    /// <param name="minDuration">������ �̵��� ����� �ּ� �ð��Դϴ�.</param>
    /// <param name="maxDuration">������ �̵��� ����� �ִ� �ð��Դϴ�.</param>
    public NpcMover(GameObject npcObject, float minDuration = 5.0f, float maxDuration = 30.0f)
    {
        if (npcObject == null)
            throw new Exception("NPC ���ӿ�����Ʈ�� null�Դϴ�.");

        if (maxDuration <= 0.0f || minDuration < 0.0f)
            throw new Exception(string.Format("{0}, {1}�� ���� �Ǽ����� �մϴ�.", nameof(minDuration), nameof(maxDuration)));
        if (minDuration > maxDuration)
            throw new Exception(string.Format("{0} <= {1}�̾�� �մϴ�.", nameof(minDuration), nameof(maxDuration)));

        this.agent = npcObject.GetComponent<NavMeshAgent>();
        if (agent == null)
            throw new Exception(string.Format("NPC ���ӿ�����Ʈ {0}�� NavMeshAgent�� ã�� �� �����ϴ�!!", npcObject.name));

        this.npcObject = npcObject;
        this.maxDuration = maxDuration;
        this.minDuration = minDuration;

        agent.updateRotation = false; // NavMeshAgent�� �̿��� ���� ȸ���� ���ڿ������� ���� ȸ���� �����ϵ��� ����

        timerDuration = 0.0f;
        stdTime = Time.time;
        currentTarget = npcObject.transform.position;
    }

    /* ===========================
     *        Util �޼ҵ�
     * ===========================*/

    /// <summary>
    /// �־��� �������� NavMesh �� ���� ��ġ�� ��ȯ�մϴ�.
    /// <br/> ���� ã�� �� ���ٸ� <paramref name="origin"/>�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
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
        return origin;
    }

    /* ===========================
     *        ��� �޼ҵ�
     * ===========================*/

    public void moveRandomPosition(Vector3 origin, float radius)
    {
        if (currentState == NpcMoveState.IDLE)
        {
            // ��� �ð� ����
            if (Time.time - stdTime >= timerDuration)
            {
                currentTarget = findRandomPos(origin, radius);
                agent.SetDestination(currentTarget);

                currentState = NpcMoveState.MOVE;
                onMoveStateChangedEventHandler?.Invoke(currentState);
                return;
            }
        }
        
        if (currentState == NpcMoveState.MOVE) 
        {
            // ���� ����
            if ((currentTarget - npcObject.transform.position).sqrMagnitude < IN_POSITION_RADIUS_SQUARE)
            {
                stdTime = Time.time;
                timerDuration = Random.Range(minDuration, maxDuration);

                currentState = NpcMoveState.IDLE;
                onMoveStateChangedEventHandler?.Invoke(currentState);
                return;
            }

            // �̵� ����
            // ��� �� ���̴� �������� ������ ȸ�� + �̵��߿��� ȸ���� �Ͼ�Բ� �ؼ� �ڿ������� ���̵��� ��.
            if (agent.steeringTarget != npcObject.transform.position)
                npcObject.transform.rotation =
                    Quaternion.Slerp(
                        npcObject.transform.rotation,
                        Quaternion.LookRotation(agent.steeringTarget - npcObject.transform.position),
                        Time.deltaTime / 0.1f);
            return;
        }
    }
}

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
    private const float ERROR_RETRY_DURATION = 0.5f;

    private Vector3 currentTarget; // ���� ������ġ
    private static readonly float IN_POSITION_RADIUS_SQUARE = 0.01f;
    private const string NAV_AREA_NPC_LAYER = "npc";
    private int NPCAreaBitMask = 1 << NavMesh.GetAreaFromName(NAV_AREA_NPC_LAYER);
    private NavMeshPath path = new NavMeshPath();

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
    /// �־��� �������� NavMesh �� ���� ��ġ�� ã���ϴ�.
    /// <br/> ���� ã�� �� ���ٸ� <see cref="false"/>�� ��ȯ�մϴ�.
    /// <br/> ���� ��ġ <paramref name="currentPos"/>�κ��� �ּ� <paramref name="minDistance"/>��ŭ ������ �Ÿ��� ã���ϴ�.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <param name="foundPos"></param>
    /// <returns></returns>
    private bool findRandomPos(Vector3 origin, float radius, out Vector3 foundPos, Vector3 currentPos, float minDistance)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 sample = origin + (Random.insideUnitSphere * radius);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(sample, out hit, 5.0f, NPCAreaBitMask))
            {
                Vector3 distance = hit.position - sample;
                distance.y = 0;
                if (distance.sqrMagnitude < 0.1f
                    && (currentPos - hit.position).sqrMagnitude >= minDistance * minDistance)
                {
                    foundPos = hit.position;
                    return true;
                }
            }
        }
        foundPos = Vector3.zero;
        return false;
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
                for (int tryCnt = 0; ; tryCnt++)
                {
                    Vector3 value = Vector3.zero;
                    if (findRandomPos(origin, radius, out value, currentTarget, 1.0f))
                    {
                        currentTarget = value;
                        if (agent.CalculatePath(currentTarget, path) 
                            && path.status == NavMeshPathStatus.PathComplete)
                        {
                            agent.SetPath(path);
                            break;
                        }
                    }
                    if (tryCnt == 10)
                    {
                        Debug.LogError("NPC " + npcObject.name + " : �̵� ��θ� ã�� ���߽��ϴ�!\n���� : ������ �̵� ������ NavMesh�� ��ġ�� ������ �ʹ� ���� �� �ֽ��ϴ�! �Ǵ�, \nNPC�� �̵��� �� ���� ����(ex : ��/�Ʒ���)���� ���ԵǾ� ���� �� �ֽ��ϴ�!\n�Ǵ�, NavMeshSurface�� " + NAV_AREA_NPC_LAYER + " ���̾ �ƴϰų� ���� ������ NavMesh�� ��ġ�� ����.");
                        stdTime = Time.time;
                        timerDuration = ERROR_RETRY_DURATION;
                        return;
                    }
                }

                currentState = NpcMoveState.MOVE;
                onMoveStateChangedEventHandler?.Invoke(currentState);
                return;
            }
        }
        
        if (currentState == NpcMoveState.MOVE) 
        {
            // ���� ����
            if ((agent.pathEndPosition - npcObject.transform.position).sqrMagnitude < IN_POSITION_RADIUS_SQUARE)
            {
                agent.ResetPath();

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

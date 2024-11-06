using System;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

/// <summary>
/// NPC의 이동을 관리하는 클래스입니다.
/// </summary>
public class NpcMover
{
    private GameObject npcObject;
    private NavMeshAgent agent; // 이동 AI를 담당할 NavMeshAgent

    private float timerDuration; // 타이머 시간
    private float stdTime; // 기준 시각
    private float maxDuration;
    private float minDuration;
    private const float ERROR_RETRY_DURATION = 0.5f;

    private Vector3 currentTarget; // 현재 랜덤위치
    private static readonly float IN_POSITION_RADIUS_SQUARE = 0.01f;
    private const string NAV_AREA_NPC_LAYER = "npc";
    private int NPCAreaBitMask = 1 << NavMesh.GetAreaFromName(NAV_AREA_NPC_LAYER);
    private NavMeshPath path = new NavMeshPath();

    private NpcMoveState currentState = NpcMoveState.IDLE;

    /// <summary>
    /// NPC가 움직이기 시작할 때 발생하는 이벤트입니다.
    /// </summary>
    public delegate void onMoveStateChangedEvent(NpcMoveState state);
    public onMoveStateChangedEvent onMoveStateChangedEventHandler;

    /// <summary>
    /// NPC의 이동을 관리하는 클래스입니다.
    /// </summary>
    /// <param name="npcObject">NPC 게임오브젝트</param>
    /// <param name="minDuration">무작위 이동시 대기할 최소 시간입니다.</param>
    /// <param name="maxDuration">무작위 이동시 대기할 최대 시간입니다.</param>
    public NpcMover(GameObject npcObject, float minDuration = 5.0f, float maxDuration = 30.0f)
    {
        if (npcObject == null)
            throw new Exception("NPC 게임오브젝트가 null입니다.");

        if (maxDuration <= 0.0f || minDuration < 0.0f)
            throw new Exception(string.Format("{0}, {1}은 양의 실수여야 합니다.", nameof(minDuration), nameof(maxDuration)));
        if (minDuration > maxDuration)
            throw new Exception(string.Format("{0} <= {1}이어야 합니다.", nameof(minDuration), nameof(maxDuration)));

        this.agent = npcObject.GetComponent<NavMeshAgent>();
        if (agent == null)
            throw new Exception(string.Format("NPC 게임오브젝트 {0}의 NavMeshAgent를 찾을 수 없습니다!!", npcObject.name));

        this.npcObject = npcObject;
        this.maxDuration = maxDuration;
        this.minDuration = minDuration;

        agent.updateRotation = false; // NavMeshAgent를 이용한 방향 회전이 부자연스러워 직접 회전을 제어하도록 세팅

        timerDuration = 0.0f;
        stdTime = Time.time;
        currentTarget = npcObject.transform.position;
    }

    /* ===========================
     *        Util 메소드
     * ===========================*/

    /// <summary>
    /// 주어진 범위에서 NavMesh 내 랜덤 위치를 찾습니다.
    /// <br/> 만약 찾을 수 없다면 <see cref="false"/>을 반환합니다.
    /// <br/> 기준 위치 <paramref name="currentPos"/>로부터 최소 <paramref name="minDistance"/>만큼 떨어진 거리를 찾습니다.
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
     *        기능 메소드
     * ===========================*/

    public void moveRandomPosition(Vector3 origin, float radius)
    {
        if (currentState == NpcMoveState.IDLE)
        {
            // 대기 시간 종료
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
                        Debug.LogError("NPC " + npcObject.name + " : 이동 경로를 찾지 못했습니다!\n원인 : 무작위 이동 영역과 NavMesh가 겹치는 영역이 너무 좁을 수 있습니다! 또는, \nNPC가 이동할 수 없는 영역(ex : 위/아래층)까지 포함되어 있을 수 있습니다!\n또는, NavMeshSurface가 " + NAV_AREA_NPC_LAYER + " 레이어가 아니거나 랜덤 영역이 NavMesh와 겹치지 않음.");
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
            // 도착 상태
            if ((agent.pathEndPosition - npcObject.transform.position).sqrMagnitude < IN_POSITION_RADIUS_SQUARE)
            {
                agent.ResetPath();

                stdTime = Time.time;
                timerDuration = Random.Range(minDuration, maxDuration);

                currentState = NpcMoveState.IDLE;
                onMoveStateChangedEventHandler?.Invoke(currentState);
                return;
            }

            // 이동 상태
            // 경로 상 꺾이는 지점에서 빠르게 회전 + 이동중에만 회전이 일어나게끔 해서 자연스럽게 보이도록 함.
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

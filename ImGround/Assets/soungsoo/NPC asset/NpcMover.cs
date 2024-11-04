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

    private Vector3 currentTarget; // 현재 랜덤위치
    private static readonly float IN_POSITION_RADIUS_SQUARE = 0.2f;

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
    /// 주어진 범위에서 NavMesh 내 랜덤 위치를 반환합니다.
    /// <br/> 만약 찾을 수 없다면 <paramref name="origin"/>을 반환합니다.
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
     *        기능 메소드
     * ===========================*/

    public void moveRandomPosition(Vector3 origin, float radius)
    {
        if (currentState == NpcMoveState.IDLE)
        {
            // 대기 시간 종료
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
            // 도착 상태
            if ((currentTarget - npcObject.transform.position).sqrMagnitude < IN_POSITION_RADIUS_SQUARE)
            {
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

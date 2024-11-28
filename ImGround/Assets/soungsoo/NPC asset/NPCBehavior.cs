using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    private static readonly string PLAYER_NAME = "Player";
    
    public Vector3 Origin;
    [Tooltip("만약 True이면 Origin 값은 오브젝트의 처음 위치로 덮어씌웁니다.")]
    public bool SetOriginAsStartPos;
    public string NPCName;
    public NPCType type = NPCType.NPC_NORMAL;
    public float Radius; // 랜덤 이동 위치의 범위
    public Vector3 PlayerLookOffset;
    public Vector3 IconOffset = new Vector3(0, 3, 0);

    [Tooltip("무작위 이동 중 대기하는 최소시간입니다.")]
    public float minDuration = 5.0f;
    [Tooltip("무작위 이동 중 대기하는 최대시간입니다.")]
    public float maxDuration = 30.0f;

    private Animator animator; // 이동 애니메이션 관리 컴포넌트 (인스펙터에서 입력받지 않고, 직접 검색합니다)

    private NpcGazer npcGazer; // npc 시선 처리 모듈
    private NpcMover npcMover; // npc 이동 모듈
    private NPCIconController npcIcon; // npc 아이콘 관리 모듈

    private bool isTalkingWithPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        // 필요 컴포넌트 직접 검색
        if ((animator = gameObject.GetComponent<Animator>()) == null)
            Debug.LogErrorFormat("gameObject {0}의 Animator를 찾을 수 없습니다!!", gameObject.name);

        if (SetOriginAsStartPos)
            Origin = transform.position;

        npcGazer = new NpcGazer(transform);
        npcMover = new NpcMover(gameObject, minDuration, maxDuration);
        npcMover.onMoveStateChangedEventHandler += onMoveStart;
        npcIcon = new NPCIconController(transform, IconOffset);
    }

    void onMoveStart(NpcMoveState state)
    {
        if (state == NpcMoveState.MOVE)
            animator.SetBool(NpcAnimationParameter.IS_WALK, true);

        if (state == NpcMoveState.IDLE)
            animator.SetBool(NpcAnimationParameter.IS_WALK, false);
    }

    // Update is called once per frame
    void Update()
    {
        setNpcIcon();
        move();
    }

    void LateUpdate()
    {
        lookAtPlayer();
    }

    public void setSelected(bool isSelected)
    {
        npcIcon.setSelected(isSelected);
    }

    public void setTalkingState(bool isTalking)
    {
        this.isTalkingWithPlayer = isTalking;
    }

    /// <summary>
    /// 주어진 거리 내에서 처음 발견된 플레이어를 찾습니다.
    /// <br/> 없거나 찾지 못한 경우 null을 반환합니다.
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    private GameObject findPlayerInDistance(float distance)
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, distance))
            if (c.name.CompareTo(PLAYER_NAME) == 0)
                return c.gameObject;
        return null;
    }

    private void lookAtPlayer()
    {
        GameObject player = findPlayerInDistance(5.0f);
        if (player == null)
        {
            npcGazer.lookAtFront();
        }
        else 
        {
            npcGazer.lookAtPos(player.transform.position + PlayerLookOffset);
        }
    }

    private void setNpcIcon()
    {
        QuestIdEnum questid = QuestInfoManager.getQuestId(type);
        if (questid != QuestIdEnum.NULL)
        {
            if (QuestManager.isDone(questid))
            {
                if (!QuestManager.hasAccepted(questid))
                {
                    npcIcon.setIconType(NpcIconsSO.REWARD);
                }
                else
                {
                    npcIcon.setIconType(NpcIconsSO.DEFAULT);
                }
            }
            else
            {
                if (!QuestManager.canReward(questid))
                {
                    npcIcon.setIconType(NpcIconsSO.QUEST);
                }
                else
                {
                    npcIcon.setIconType(NpcIconsSO.REWARD);
                }
            }
        }
        else
        {
            npcIcon.setIconType(NpcIconsSO.DEFAULT);
        }
    }

    private void move()
    {
        if (isTalkingWithPlayer)
        {
            GameObject player = findPlayerInDistance(5.0f);
            if (player != null)
            {
                npcMover.talkWithPlayer(player.transform.position);
            }
        }
        else
        {
            npcMover.moveRandomPosition(Origin, Radius);
        }
    }
}

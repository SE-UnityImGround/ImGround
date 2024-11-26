using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    private static readonly string PLAYER_NAME = "Player";
    
    public Vector3 Origin;
    [Tooltip("���� True�̸� Origin ���� ������Ʈ�� ó�� ��ġ�� �����ϴ�.")]
    public bool SetOriginAsStartPos;
    public string NPCName;
    public NPCType type = NPCType.NPC_NORMAL;
    public float Radius; // ���� �̵� ��ġ�� ����
    public Vector3 PlayerLookOffset;
    public Vector3 IconOffset = new Vector3(0, 3, 0);

    [Tooltip("������ �̵� �� ����ϴ� �ּҽð��Դϴ�.")]
    public float minDuration = 5.0f;
    [Tooltip("������ �̵� �� ����ϴ� �ִ�ð��Դϴ�.")]
    public float maxDuration = 30.0f;

    private Animator animator; // �̵� �ִϸ��̼� ���� ������Ʈ (�ν����Ϳ��� �Է¹��� �ʰ�, ���� �˻��մϴ�)

    private NpcGazer npcGazer; // npc �ü� ó�� ���
    private NpcMover npcMover; // npc �̵� ���
    private NPCIconController npcIcon; // npc ������ ���� ���

    private bool isTalkingWithPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        // �ʿ� ������Ʈ ���� �˻�
        if ((animator = gameObject.GetComponent<Animator>()) == null)
            Debug.LogErrorFormat("gameObject {0}�� Animator�� ã�� �� �����ϴ�!!", gameObject.name);

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
    /// �־��� �Ÿ� ������ ó�� �߰ߵ� �÷��̾ ã���ϴ�.
    /// <br/> ���ų� ã�� ���� ��� null�� ��ȯ�մϴ�.
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

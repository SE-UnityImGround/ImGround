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
    public NPCType type;
    public float Radius; // ���� �̵� ��ġ�� ����
    public Vector3 PlayerLookOffset;
    public Vector3 IconOffset = new Vector3(0, 3, 0);

    private Animator animator; // �̵� �ִϸ��̼� ���� ������Ʈ (�ν����Ϳ��� �Է¹��� �ʰ�, ���� �˻��մϴ�)

    private NpcGazer npcGazer; // npc �ü� ó�� ���
    private NpcMover npcMover; // npc �̵� ���

    private NPCIconBehavior defaultIcon;
    private NPCIconBehavior questIcon;
    private NPCIconBehavior RewardIcon;
    private NPCIconBehavior currentIcon;

    // Start is called before the first frame update
    void Start()
    {
        // �ʿ� ������Ʈ ���� �˻�
        if ((animator = gameObject.GetComponent<Animator>()) == null)
            Debug.LogErrorFormat("gameObject {0}�� Animator�� ã�� �� �����ϴ�!!", gameObject.name);

        if (SetOriginAsStartPos)
            Origin = transform.position;

        npcGazer = new NpcGazer(transform);
        npcMover = new NpcMover(gameObject);
        npcMover.onMoveStateChangedEventHandler += onMoveStart;

        defaultIcon = NpcIconsSO.getNPCIcon(NpcIconsSO.DEFAULT);
        questIcon = NpcIconsSO.getNPCIcon(NpcIconsSO.QUEST);
        RewardIcon = NpcIconsSO.getNPCIcon(NpcIconsSO.REWARD);
        currentIcon = defaultIcon;

        currentIcon.show(transform, IconOffset);
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
        npcMover.moveRandomPosition(Origin, Radius);
    }

    void LateUpdate()
    {
        lookAtPlayer();
    }

    public void setSelected(bool isSelected)
    {
        currentIcon.setSelected(isSelected);
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
}

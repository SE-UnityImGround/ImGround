using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private InGameViewBehavior inGameUI;
    [SerializeField]
    private TalkBehavior talkView;
    [SerializeField]
    private PlayerNPCController npcController;

    // Start is called before the first frame update
    void Start()
    {
        if (inGameUI == null)
        {
            Debug.LogError(nameof(PlayerUIController) + "���� " + nameof(inGameUI) + "�� ��ϵ��� �ʾҽ��ϴ�!");
        }
        if (npcController == null)
        {
            Debug.LogError(nameof(PlayerUIController) + "���� " + nameof(npcController) + "�� ��ϵ��� �ʾҽ��ϴ�!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��ȭ �����ϱ� ���� ����
        if (inGameUI.mode == InGameViewMode.DEFAULT
            && Input.GetKeyDown(KeyCode.F) && npcController.selectedNPC != null)
        {
            NPCBehavior npc = npcController.selectedNPC.GetComponent<NPCBehavior>();
            Debug.Log("���õ� npc : " + npc.name + " Ÿ�� : " + npc.type);
            talkView.startTalk(npc);
        }

        // ���� UI ���� ����
        if ((inGameUI.mode == InGameViewMode.DEFAULT || inGameUI.mode == InGameViewMode.MANUFACT)
            && Input.GetKeyDown(KeyCode.M))
        {
            inGameUI.toggleView(InGameViewMode.MANUFACT);
        }
    }
}

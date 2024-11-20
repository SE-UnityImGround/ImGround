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
            Debug.LogError(nameof(PlayerUIController) + "내에 " + nameof(inGameUI) + "가 등록되지 않았습니다!");
        }
        if (npcController == null)
        {
            Debug.LogError(nameof(PlayerUIController) + "내에 " + nameof(npcController) + "가 등록되지 않았습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 대화 시작하기 가능 조건
        if (inGameUI.mode == InGameViewMode.DEFAULT
            && Input.GetKeyDown(KeyCode.F) && npcController.selectedNPC != null)
        {
            NPCBehavior npc = npcController.selectedNPC.GetComponent<NPCBehavior>();
            Debug.Log("선택된 npc : " + npc.name + " 타입 : " + npc.type);
            talkView.startTalk(npc);
        }

        // 제조 UI 열기 조건
        if ((inGameUI.mode == InGameViewMode.DEFAULT || inGameUI.mode == InGameViewMode.MANUFACT)
            && Input.GetKeyDown(KeyCode.M))
        {
            inGameUI.toggleView(InGameViewMode.MANUFACT);
        }
    }
}

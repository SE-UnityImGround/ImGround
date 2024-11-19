using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private InGameViewBehavior inGameUI;
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
        if (Input.GetKeyDown(KeyCode.F) && npcController.selectedNPC != null)
        {
            NPCBehavior npc = npcController.selectedNPC.GetComponent<NPCBehavior>();
            Debug.Log("선택된 npc : " + npc.name + " 타입 : " + npc.type);
            TalkManager.setTalk(npc);
            inGameUI.displayView(InGameViewMode.TALK);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            inGameUI.toggleView(InGameViewMode.MANUFACT);
        }
    }
}

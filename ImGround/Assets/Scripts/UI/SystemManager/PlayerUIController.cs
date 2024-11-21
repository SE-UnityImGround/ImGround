using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private InGameViewBehavior inGameUI;
    [SerializeField]
    private PlayerNPCController npcController;
    [SerializeField]
    private Player playerScript;

    private void checkValue(object v, string name)
    {
        if (v == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        checkValue(inGameUI, nameof(inGameUI));
        checkValue(npcController, nameof(npcController));
        checkValue(playerScript, nameof(playerScript));
    }

    const float TIMER_DURATION = 10.0f;
    float TEMP_TIMER = TIMER_DURATION;
    // Update is called once per frame
    void Update()
    {
        float lifeRatio = playerScript.health / 10.0f; // 최대체력 - 임시값
        float expRatio = 50.0f / playerScript.requiredExp[playerScript.level]; // 현재 경험치 - 임시값
        TEMP_TIMER += Time.deltaTime;
        if (TEMP_TIMER >= TIMER_DURATION)
        {
            TEMP_TIMER = 0.0f;
            Debug.LogError("경험치/최대 체력 로직 수정 필요"); // 플레이어의 최대 체력, 현재 경험치 정보가 숨겨짐
        }
        inGameUI.getUIBehavior<HomeScreen>().setPlayerInfo(lifeRatio, expRatio, playerScript.level);

        // 대화 시작하기 가능 조건
        if (inGameUI.mode == InGameViewMode.DEFAULT
            && Input.GetKeyDown(KeyCode.F) && npcController.selectedNPC != null)
        {
            NPCBehavior npc = npcController.selectedNPC.GetComponent<NPCBehavior>();
            Debug.Log("선택된 npc : " + npc.name + " 타입 : " + npc.type);
            inGameUI.getUIBehavior<TalkBehavior>().startTalk(npc);
        }

        // ESC 키로 UI 조작
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inGameUI.doEscapeProcess();
        }
    }
}

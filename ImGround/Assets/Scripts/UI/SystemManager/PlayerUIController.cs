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
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, name);
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
        float lifeRatio = playerScript.health / 10.0f; // �ִ�ü�� - �ӽð�
        float expRatio = 50.0f / playerScript.requiredExp[playerScript.level]; // ���� ����ġ - �ӽð�
        TEMP_TIMER += Time.deltaTime;
        if (TEMP_TIMER >= TIMER_DURATION)
        {
            TEMP_TIMER = 0.0f;
            Debug.LogError("����ġ/�ִ� ü�� ���� ���� �ʿ�"); // �÷��̾��� �ִ� ü��, ���� ����ġ ������ ������
        }
        inGameUI.getUIBehavior<HomeScreen>().setPlayerInfo(lifeRatio, expRatio, playerScript.level);

        // ��ȭ �����ϱ� ���� ����
        if (inGameUI.mode == InGameViewMode.DEFAULT
            && Input.GetKeyDown(KeyCode.F) && npcController.selectedNPC != null)
        {
            NPCBehavior npc = npcController.selectedNPC.GetComponent<NPCBehavior>();
            Debug.Log("���õ� npc : " + npc.name + " Ÿ�� : " + npc.type);
            inGameUI.getUIBehavior<TalkBehavior>().startTalk(npc);
        }

        // ESC Ű�� UI ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inGameUI.doEscapeProcess();
        }
    }
}

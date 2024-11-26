using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private InGameViewBehavior inGameUI;
    // - ������ ���� ������ ���� �ľ��ϱ� ���� -
    private InGameViewBehavior _inGameUI { get { checkValue(inGameUI, nameof(inGameUI), 0); return inGameUI; } set { inGameUI = value; } }

    private PlayerNPCController npcController; // �� ��ũ��Ʈ�� ���� ���ӿ�����Ʈ�� �����ؾ��մϴ�.
    // - ������ ���� ������ ���� �ľ��ϱ� ���� -
    private PlayerNPCController _npcController { get { checkValue(npcController, nameof(npcController), 1); return npcController; } set { npcController = value; } }
    private Player playerScript; // �� ��ũ��Ʈ�� ���� ���ӿ�����Ʈ�� �����ؾ��մϴ�.
    // - ������ ���� ������ ���� �ľ��ϱ� ���� -
    private Player _playerScript { get { checkValue(playerScript, nameof(playerScript), 1); return playerScript; } set { playerScript = value; } }

    /// <summary>
    /// - ������ ���� ������ ���� �ľ��ϱ� ���� -
    /// </summary>
    /// <param name="v"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    private void checkValue(object v, string name, int type)
    {
        if (v == null)
        {
            if (type == 1)
            {
                Debug.LogErrorFormat("������Ʈ ã�� �� ���� : {0}�� {1}�� ��ϵǾ�� �մϴ�!", gameObject.name, name);
                return;
            }
            // default null value error
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, name);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        npcController = gameObject.GetComponent<PlayerNPCController>();
        playerScript = gameObject.GetComponent<Player>();

        checkValue(inGameUI, nameof(inGameUI), 0);
        checkValue(npcController, nameof(npcController), 1);
        checkValue(playerScript, nameof(playerScript), 2);

        ItemThrowManager.listenItemThrowEvent(onItemThrow);
    }

    private void onItemThrow(GameObject itemObject)
    {
        itemObject.transform.position = transform.position;
        itemObject.GetComponent<FloatingItem>().Initialize(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        updatePlayerInfo();
        uiControllByInput();
    }

    private void updatePlayerInfo()
    {
        _inGameUI.getUIBehavior<HomeScreen>().setPlayerInfo(
            _playerScript.health / (float)_playerScript.MaxHealth,
            _playerScript.Exp / (float)_playerScript.requiredExp[_playerScript.level],
            _playerScript.level);
    }

    private void uiControllByInput()
    {
        // ��ȭ �����ϱ� ���� ����
        if (_inGameUI.mode == InGameViewMode.DEFAULT
            && Input.GetKeyDown(KeyCode.F) && _npcController.selectedNPC != null)
        {
            NPCBehavior npc = _npcController.selectedNPC.GetComponent<NPCBehavior>();
            Debug.Log("���õ� npc : " + npc.name + " Ÿ�� : " + npc.type);
            _inGameUI.getUIBehavior<TalkBehavior>().startTalk(npc);
        }

        // ESC Ű�� UI ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _inGameUI.doEscapeProcess();
        }
    }
}

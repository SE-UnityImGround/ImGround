using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private InGameViewBehavior inGameUI;
    // - 디버깅시 에러 문구를 빨리 파악하기 위함 -
    private InGameViewBehavior _inGameUI { get { checkValue(inGameUI, nameof(inGameUI), 0); return inGameUI; } set { inGameUI = value; } }

    private PlayerNPCController npcController; // 이 스크립트가 붙은 게임오브젝트에 존재해야합니다.
    // - 디버깅시 에러 문구를 빨리 파악하기 위함 -
    private PlayerNPCController _npcController { get { checkValue(npcController, nameof(npcController), 1); return npcController; } set { npcController = value; } }
    private Player playerScript; // 이 스크립트가 붙은 게임오브젝트에 존재해야합니다.
    // - 디버깅시 에러 문구를 빨리 파악하기 위함 -
    private Player _playerScript { get { checkValue(playerScript, nameof(playerScript), 1); return playerScript; } set { playerScript = value; } }

    private const float DEFAULT_BOSS_IDENTIFY_RANGE = 50.0f;
    [SerializeField]
    private float bossIdentifyRange = DEFAULT_BOSS_IDENTIFY_RANGE;
    private const float FINDING_BOSS_DUARTION = 0.25f; // 성능을 고려해 보스를 실시간으로 찾는 대신 일정 시간마다 찾기
    private float findingBossStdTime = 0.0f;

    /// <summary>
    /// - 디버깅시 에러 문구를 빨리 파악하기 위함 -
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
                Debug.LogErrorFormat("컴포넌트 찾을 수 없음 : {0}에 {1}가 등록되어야 합니다!", gameObject.name, name);
                return;
            }
            // default null value error
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
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

        // 오류 회피용 - 기본값 강제 주입
        if (bossIdentifyRange <= 1.0f)
            bossIdentifyRange = DEFAULT_BOSS_IDENTIFY_RANGE;
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
        updateBossHealth();
        uiControllByInput();
    }

    private void updatePlayerInfo()
    {
        float expRate = 1.0f;
        if (_playerScript.level >= 0 && _playerScript.level < _playerScript.requiredExp.Length)
            expRate = _playerScript.Exp / (float)_playerScript.requiredExp[_playerScript.level];
        _inGameUI.getUIBehavior<HomeScreen>().setPlayerInfo(
            _playerScript.health / (float)_playerScript.MaxHealth,
            expRate,
            _playerScript.level);
    }

    private void updateBossHealth()
    {
        if (Time.time - findingBossStdTime < FINDING_BOSS_DUARTION)
            return;
        findingBossStdTime = Time.time;

        Boss boss = FindObjectOfType<Boss>();
        if (boss != null
            && (transform.position - boss.transform.position).sqrMagnitude < bossIdentifyRange * bossIdentifyRange)
        {
            inGameUI.getUIBehavior<BossHealthBehavior>().setHealth(boss.Health / (float)boss.maxHealth);
            inGameUI.getUIBehavior<BossHealthBehavior>().setVisible(true);
        }
        else
        {
            inGameUI.getUIBehavior<BossHealthBehavior>().setVisible(false);
        }
    }

    private void uiControllByInput()
    {
        // 대화 시작하기 가능 조건
        if (_inGameUI.mode == InGameViewMode.DEFAULT
            && Input.GetKeyDown(KeyCode.Q) && _npcController.selectedNPC != null)
        {
            NPCBehavior npc = _npcController.selectedNPC.GetComponent<NPCBehavior>();
            Debug.Log("선택된 npc : " + npc.name + " 타입 : " + npc.type);
            _inGameUI.getUIBehavior<TalkBehavior>().startTalk(npc);
        }

        // ESC 키로 UI 조작
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _inGameUI.doEscapeProcess();
        }
    }
}

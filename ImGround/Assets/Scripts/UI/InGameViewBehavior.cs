using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameViewBehavior : MonoBehaviour
{
    public InGameViewMode mode { get; private set; } = InGameViewMode.DEFAULT;

    [SerializeField]
    private UIBehavior InGameView;
    [SerializeField]
    private GameObject SettingView;
    [SerializeField]
    private UIBehavior QuestView;
    [SerializeField]
    private UIBehavior ManufactView;
    [SerializeField]
    private UIBehavior ShopView;
    [SerializeField]
    private UIBehavior InventoryView;
    [SerializeField]
    private UIBehavior TalkView;
    [SerializeField]
    private BossHealthBehavior BossHealthView;

    // Start is called before the first frame update
    void Start()
    {
        InGameView.initialize();
        QuestView.initialize();
        ManufactView.initialize();
        ShopView.initialize();
        InventoryView.initialize();
        TalkView.initialize();
        BossHealthView.setVisible(false);

        displayView(InGameViewMode.DEFAULT);
    }

    public void onGameCloseButtonClick()
    {
        SaveManager.invokeOnSave();
        PlayerPrefs.Save();
        Debug.LogWarning("프로그램 종료");
        Application.Quit();
    }

    /// <summary>
    /// esc 키 입력에 대한 UI 처리입니다.
    /// </summary>
    public void doEscapeProcess()
    {
        if (mode == InGameViewMode.DEFAULT)
        {
            displayView(InGameViewMode.SETTING);
            return;
        }

        if (mode != InGameViewMode.TALK)
        {
            hideView(mode);
            return;
        }
    }

    /* ====================================== 
     *          UI 구성요소 접근자
     * ====================================== */

    public T getUIBehavior<T>()
    {
        if (InGameView is T)
            return (T)(object)InGameView;
        if (QuestView is T)
            return (T)(object)QuestView;
        if (ManufactView is T)
            return (T)(object)ManufactView;
        if (ShopView is T)
            return (T)(object)ShopView;
        if (InventoryView is T)
            return (T)(object)InventoryView;
        if (TalkView is T)
            return (T)(object)TalkView;
        if (BossHealthView is T)
            return (T)(object)BossHealthView;
        
        return default(T);
    }

    /* ====================================== 
     *          화면 열기/닫기 조작
     * ====================================== */

    public void hideView(InGameViewMode mode)
    {
        if (this.mode == mode)
        {
            this.mode = InGameViewMode.DEFAULT;
            InputManager.onUI = false;
        }
        switch (mode)
        {
            case InGameViewMode.SETTING:
                SettingView.SetActive(false);
                return;
            case InGameViewMode.SHOP:
                ShopView.setActive(false);
                return;
            case InGameViewMode.TALK:
                TalkView.setActive(false);
                return;
            case InGameViewMode.QUEST:
                QuestView.setActive(false);
                return;
            case InGameViewMode.MANUFACT:
                ManufactView.setActive(false);
                return;
            case InGameViewMode.INVENTORY:
                InventoryView.setActive(false);
                return;
        }
    }

    public void displayView(InGameViewMode mode)
    {
        InGameView.setActive(
            mode == InGameViewMode.DEFAULT
            || mode == InGameViewMode.SETTING
            || mode == InGameViewMode.INVENTORY
            || mode == InGameViewMode.QUEST
            || mode == InGameViewMode.SHOP
            || mode == InGameViewMode.MANUFACT
            || mode == InGameViewMode.TALK);
        SettingView.SetActive(mode == InGameViewMode.SETTING);
        QuestView.setActive(mode == InGameViewMode.QUEST);
        ManufactView.setActive(mode == InGameViewMode.MANUFACT);
        ShopView.setActive(mode == InGameViewMode.SHOP);
        InventoryView.setActive(mode == InGameViewMode.INVENTORY);
        TalkView.setActive(mode == InGameViewMode.TALK);

        this.mode = mode;
        InputManager.onUI = (this.mode != InGameViewMode.DEFAULT);
    }

    public void toggleView(InGameViewMode mode)
    {
        if (this.mode == mode)
            displayView(InGameViewMode.DEFAULT);
        else
            displayView(mode);
    }
}

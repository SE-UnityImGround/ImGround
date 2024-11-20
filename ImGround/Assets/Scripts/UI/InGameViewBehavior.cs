using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameViewBehavior : MonoBehaviour
{
    public InGameViewMode mode { get; private set; } = InGameViewMode.DEFAULT;

    [SerializeField]
    private GameObject InGameView;
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

    // Start is called before the first frame update
    void Start()
    {
        QuestView.initialize();
        ManufactView.initialize();
        ShopView.initialize();
        InventoryView.initialize();
        TalkView.initialize();

        displayView(InGameViewMode.DEFAULT);
    }

    public void onGameCloseButtonClick()
    {
        // 추가 처리 가능 : 메인화면으로 다시 돌아간다던지
        Debug.LogWarning("프로그램 종료");
        Application.Quit();
    }

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

    public void hideView(InGameViewMode mode)
    {
        if (this.mode == mode)
            this.mode = InGameViewMode.DEFAULT;
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
        InGameView.SetActive(
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
    }

    public void toggleView(InGameViewMode mode)
    {
        if (this.mode == mode)
            displayView(InGameViewMode.DEFAULT);
        else
            displayView(mode);
    }
}

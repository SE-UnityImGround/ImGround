using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameViewBehavior : MonoBehaviour
{
    private InGameViewMode mode = InGameViewMode.DEFAULT;

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

        ((ShopBehavior)ShopView).setUp("그냥 상점");

        displayView(InGameViewMode.DEFAULT);
    }

    // Update is called once per frame
    void Update()
    {
        
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

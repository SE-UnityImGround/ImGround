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
    private GameObject ShopView;
    [SerializeField]
    private UIBehavior InventoryView;
    [SerializeField]
    private GameObject TalkView;

    // Start is called before the first frame update
    void Start()
    {
        QuestView.initialize();
        ManufactView.initialize();
        InventoryView.initialize();

        displayView(InGameViewMode.DEFAULT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool displayView(InGameViewMode mode)
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
        ShopView.SetActive(mode == InGameViewMode.SHOP);
        InventoryView.setActive(mode == InGameViewMode.INVENTORY);
        TalkView.SetActive(mode == InGameViewMode.TALK);

        this.mode = mode;
        return true;
    }
}

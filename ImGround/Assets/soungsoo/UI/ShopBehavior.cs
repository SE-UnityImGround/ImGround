using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBehavior : UIBehavior
{
    [SerializeField]
    private GameObject ShopItemListView;
    [SerializeField]
    private GameObject ShopItemPrefab;
    [SerializeField]
    private TMPro.TMP_Text ShopNameView;

    public override void initialize()
    {
        if (ShopItemListView == null)
        {
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, ShopItemListView);
        }
        if (ShopItemPrefab == null)
        {
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, ShopItemPrefab);
        }
    }

    public void setUp(string shopName)
    {
        ShopNameView.text = shopName;
        test();
    }

    void test()
    {
        int price = 1000;
        foreach (ItemIdEnum id in System.Enum.GetValues(typeof(ItemIdEnum))) {

            addShopItem(new Item(id), price);
            price += 1000;
        }
    }

    void buyTest(Item item, int price)
    {
        Debug.Log("������ " + item.name + "�� " + price + "���� ��");
    }

    public void addShopItem(Item item, int price)
    {
        ShopItemBehavior newItem = Instantiate(ShopItemPrefab, ShopItemListView.transform).GetComponent<ShopItemBehavior>();
        newItem.initialize(item, price);
        newItem.BuyItemEventHandler += onBuyItem;
    }

    private void onBuyItem(Item item, int price)
    {
        buyTest(item, price);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

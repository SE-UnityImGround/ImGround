using System;
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

    private ShopInfo currentShop;
    private List<ShopItemBehavior> shopItems = new List<ShopItemBehavior>();

    private Action onClose;

    private void checkValue(object v, string name)
    {
        if (v == null)
        {
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, name);
            return;
        }
    }

    public override void initialize()
    {
        checkValue(ShopItemListView, nameof(ShopItemListView));
        checkValue(ShopItemPrefab, nameof(ShopItemPrefab));
        checkValue(ShopNameView, nameof(ShopNameView));

        InventoryManager.onSlotItemChangedHandler += onInventoryChanged;
        InventoryManager.onMoneyChangedHandler += onMoneyChanged;
    }

    /// <summary>
    /// (������ �Ǹ� ��Ȳ���� ������ ������Ʈ ó��)
    /// </summary>
    /// <param name="slotIdx"></param>
    private void onInventoryChanged(int slotIdx)
    {
        if (currentShop != null && !currentShop.isSellingShop)
        {
            clearShopItems();
            foreach (KeyValuePair<ItemIdEnum, int> item in InventoryManager.getInventoryInfo())
            {
                int sellPrice = ItemInfoManager.getItemInfo(item.Key).buyingPrice - 500;
                if (sellPrice <= 0)
                    sellPrice = 500;
                addShopItem(new Item(item.Key), sellPrice);
            }
        }
    }

    /// <summary>
    /// �� ���� ȭ���� �����մϴ�. (ǥ������ ����) ���� ����� ����Ǿ�� �� �޼ҵ带 �Բ� �Է¹޽��ϴ�.
    /// </summary>
    /// <param name="shopInfo"></param>
    /// <param name="onCloseCallBack"></param>
    public void setShopView(ShopInfo shopInfo, Action onCloseCallBack)
    {
        currentShop = shopInfo;
        onClose = onCloseCallBack;

        ShopNameView.text = currentShop.shopName;
        clearShopItems();
        if (currentShop.isSellingShop)
        {
            foreach (ItemBundle item in currentShop.shopItem)
            {
                addShopItem(item.item, ItemInfoManager.getItemInfo(item.item.itemId).buyingPrice);
            }
            displayBuyable(InventoryManager.getMoney());
        }
        else
        {
            foreach (KeyValuePair<ItemIdEnum, int> item in InventoryManager.getInventoryInfo())
            {
                int sellPrice = ItemInfoManager.getItemInfo(item.Key).buyingPrice - 500;
                if (sellPrice <= 0)
                    sellPrice = 500;
                addShopItem(new Item(item.Key), sellPrice);
            }
        }
    }

    private void clearShopItems()
    {
        foreach (ShopItemBehavior item in shopItems)
        {
            Destroy(item.gameObject);
        }
        shopItems.Clear();
    }

    private void addShopItem(Item item, int price)
    {
        ShopItemBehavior newItem = Instantiate(ShopItemPrefab, ShopItemListView.transform).GetComponent<ShopItemBehavior>();
        newItem.initialize(item, price);
        newItem.TradeItemEventHandler += onTradeItem;
        shopItems.Add(newItem);
    }

    private void displayBuyable(int money)
    {
        if (currentShop.isSellingShop)
        {
            foreach (ShopItemBehavior shop in shopItems)
            {
                shop.setBuyable(money);
            }
        }
    }

    /// <summary>
    /// ������ �ŷ��� ���������� ����Ǵ� �̺�Ʈ ó�����Դϴ�.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="price"></param>
    private void onTradeItem(Item item, int price)
    {
        // �� ������ �� ��
        if (!currentShop.isSellingShop)
        {
            InventoryManager.removeItem(item.itemId, 1);
            InventoryManager.changeMoney(price);
            return;
        }

        // ��ǵ� ���� ����
        if (InventoryManager.getMoney() < price)
            return;

        // ���濡 ������ �ֳ�
        if (InventoryManager.addItem(item))
        {
            InventoryManager.changeMoney(-price);
            return;
        }

        // ������ �� á��
        WarningManager.startWarning();
    }

    /// <summary>
    /// �ݱ� �������� �߰� ó���� ���� �������̵� �� �Լ��Դϴ�.
    /// </summary>
    /// <param name="isActive"></param>
    public override void setActive(bool isActive)
    {
        if (!isActive)
        {
            Action closeFunc = onClose;
            onClose = null;
            closeFunc?.Invoke();
        }
        base.setActive(isActive);
    }

    /// <summary>
    /// �÷��̾ ���� ���� ��ȭ�� �� �߻��ϴ� �̺�Ʈ ó�����Դϴ�.
    /// </summary>
    /// <param name="money"></param>
    public void onMoneyChanged(int money)
    {
        if (currentShop != null)
            displayBuyable(money);
    }
}

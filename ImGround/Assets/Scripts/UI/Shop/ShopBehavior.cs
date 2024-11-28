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
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
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
    /// (아이템 판매 상황임을 가정한 업데이트 처리)
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
    /// 새 상점 화면을 구성합니다. (표시하진 않음) 상점 종료시 실행되어야 할 메소드를 함께 입력받습니다.
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
    /// 아이템 거래를 수락했을때 실행되는 이벤트 처리기입니다.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="price"></param>
    private void onTradeItem(Item item, int price)
    {
        // 내 물건을 팔 때
        if (!currentShop.isSellingShop)
        {
            InventoryManager.removeItem(item.itemId, 1);
            InventoryManager.changeMoney(price);
            return;
        }

        // 살건데 돈이 없네
        if (InventoryManager.getMoney() < price)
            return;

        // 가방에 공간이 있네
        if (InventoryManager.addItem(item))
        {
            InventoryManager.changeMoney(-price);
            return;
        }

        // 가방이 꽉 찼네
        WarningManager.startWarning();
    }

    /// <summary>
    /// 닫기 과정에서 추가 처리를 위해 오버라이딩 된 함수입니다.
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
    /// 플레이어가 가진 돈이 변화할 때 발생하는 이벤트 처리기입니다.
    /// </summary>
    /// <param name="money"></param>
    public void onMoneyChanged(int money)
    {
        if (currentShop != null)
            displayBuyable(money);
    }
}

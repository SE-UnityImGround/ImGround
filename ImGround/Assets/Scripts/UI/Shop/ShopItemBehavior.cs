using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemBehavior : MonoBehaviour
{
    [SerializeField]
    private Image itemImg;
    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemPrice;
    [SerializeField]
    private Button buyButton;

    private Item item;
    private int price;

    public delegate void TradeItemEvent(Item item, int price);
    public TradeItemEvent TradeItemEventHandler;

    public void initialize(Item sellingItem, int price)
    {
        this.item = sellingItem;
        itemImg.sprite = sellingItem.image;
        itemName.text = sellingItem.name;
        this.price = price;
        this.itemPrice.text = priceString(price);
    }

    private string priceString(int price)
    {
        return price.ToString() + " $";
    }

    /// <summary>
    /// 주어진 가격에 대해 구매 가능한지를 표시합니다.
    /// </summary>
    /// <param name="money"></param>
    public void setBuyable(int money)
    {
        if (this.price > money)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    public void onTradeBtnClick()
    {
        UISoundManager.playUiSound(UISoundObject.TRADE);
        TradeItemEventHandler.Invoke(item, price);
    }
}

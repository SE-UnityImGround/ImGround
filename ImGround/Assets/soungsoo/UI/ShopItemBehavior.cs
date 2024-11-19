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

    private Item item;
    private int price;

    public delegate void BuyItemEvent(Item item, int price);
    public BuyItemEvent BuyItemEventHandler;

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

    public void onBuyBtnClick()
    {
        BuyItemEventHandler.Invoke(item, price);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

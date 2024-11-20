using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 아이템의 각종 고유정보를 저장하는 클래스입니다.
/// 아이템은 ItemIdEnum으로 구분되며, 고유정보에는 이름, 최대 중첩가능 수 등이 포함됩니다.
/// </summary>
public class ItemInfo
{
    public readonly ItemIdEnum itemId;
    public readonly ImageIdEnum itemImg;
    public readonly string itemName;
    public readonly int maxCount;
    public readonly int buyingPrice;

    public ItemInfo(ItemIdEnum itemId, ImageIdEnum itemImg, string itemName, int maxCount, int buyingPrice)
    {
        this.itemId = itemId;
        this.itemImg = itemImg;
        this.itemName = itemName;
        this.maxCount = maxCount;
        this.buyingPrice = buyingPrice;
    }
}

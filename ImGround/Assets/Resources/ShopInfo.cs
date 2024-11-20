using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 상점에서 판매하는 물품에 관한 정보입니다.
/// </summary>
public class ShopInfo
{
    public readonly string shopName;
    /// <summary>
    /// 이 가게가 판매하는 가게면 true, 매입하는 가게면 false입니다.
    /// </summary>
    public readonly bool isSellingShop;
    public readonly ItemBundle[] shopItem;

    public ShopInfo(string shopName, bool isSellingShop, params ItemBundle[] shopItem)
    {
        this.shopName = shopName;
        this.isSellingShop = isSellingShop;
        this.shopItem = shopItem;
    }
}

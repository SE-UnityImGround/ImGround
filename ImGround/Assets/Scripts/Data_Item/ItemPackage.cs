using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 아이템 보따리 클래스입니다.
/// </summary>
public class ItemPackage : Item
{
    public ItemBundle[] items;

    public ItemPackage(ItemBundle[] items) : base(ItemIdEnum.PACKAGE)
    {
        this.items = items;
    }
}

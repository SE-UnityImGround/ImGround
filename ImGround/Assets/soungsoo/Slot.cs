using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Slot
{
    public Item item = null;

    /// <summary>
    /// 아이템을 슬롯에 추가하려고 시도하며, 한 개 이상의 아이템이 추가되면 true를 반환합니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        if (item == null)
        {
            this.item = item;
            return true;
        }
        else if (this.item.remainCapacity > 0)
        {
            item.count -= this.item.remainCapacity;
            this.item.count += this.item.remainCapacity;
            return true;
        }

        return false;
    }

    public bool hasItem()
    {
        return (item != null && item.count > 0);
    }

    public Item takeItem(int count = -1)
    {
        if (item == null)
            return null;

        Item result;

        if (count < 0 || count >= item.count)
        {
            result = item;
            clear();
        }
        else
        {
            result = item.getDividedItems(count);
            if (item.count == 0)
                clear();
        }

        return result;
    }

    public void clear()
    {
        item = null;
    }
}

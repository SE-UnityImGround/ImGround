using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 인벤토리 내 슬롯의 기본 자료구조입니다.
/// </summary>
public class Slot
{
    public Item item = null;

    /// <summary>
    /// 슬롯의 아이템에 변동이 생길시 발생하는 이벤트입니다.
    /// </summary>
    /// <param name="updatedItem">변동된 이후의 아이템입니다.</param>
    public delegate void itemUpdatedEvent(Item updatedItem);

    /// <summary>
    /// 아이템 변동 이벤트를 처리할 Handler입니다.
    /// </summary>
    public itemUpdatedEvent itemUpdatedEventHandler;

    /// <summary>
    /// 아이템을 슬롯에 추가하려고 시도하며, 한 개 이상의 아이템이 추가되면 true를 반환합니다.
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 item 객체에 반영됩니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        if (this.item == null)
        {
            this.item = new Item(item);
            item.getDividedItems();

            itemUpdatedEventHandler?.Invoke(this.item);
            return true;
        }
        else if (this.item.addItem(item))
        {
            itemUpdatedEventHandler?.Invoke(this.item);
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// 슬롯에 아이템이 존재하는지, 0개 이상인지를 검사합니다.
    /// 만약 아이템이 없거나, 있어도 갯수가 0개이면 false를 반환합니다.
    /// </summary>
    /// <returns>아이템이 1개 이상이면 false를, 아이템이 없거나, 갯수가 0개이면 true를 반환합니다.</returns>
    public bool hasItem()
    {
        return (item != null && item.count > 0);
    }

    /// <summary>
    /// 슬롯에서 아이템을 주어진 갯수만큼 꺼냅니다.
    /// <br/> 갯수가 음수이면 모두 꺼냅니다.
    /// <br/> 아이템이 없다면 null을 반환합니다.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 슬롯을 즉시 비웁니다.
    /// </summary>
    public void clear()
    {
        item = null;
    }
}

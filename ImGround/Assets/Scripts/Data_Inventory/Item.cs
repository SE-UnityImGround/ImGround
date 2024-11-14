using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 아이템을 표현하는 기본 자료구조입니다.
/// </summary>
public class Item
{
    public ItemIdEnum itemId { get; private set; }
    public string name { get; private set; }
    public int count { get; private set; }
    public int maxCount { get; private set; }
    public int remainCapacity { get => (maxCount - count < 0) ? 0 : maxCount - count; }

    /// <summary>
    /// 기존 item 객체로부터 똑같은 Item 객체를 복사합니다.
    /// </summary>
    /// <param name="item">내용을 복사할 Item 객체</param>
    public Item(Item item) : this(item.itemId, item.count)
    {
    }
    
    /// <summary>
    /// 아이템을 생성합니다.
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="count"></param>
    public Item(ItemIdEnum itemId, int count)
    {
        this.itemId = itemId;
        this.count = count;
        this.name = ItemInfoManager.getItemName(itemId);
        this.maxCount = ItemInfoManager.getMaxCount(itemId);
    }

    /// <summary>
    /// 해당 아이템에서 주어진 갯수만큼을 덜어내어 반환합니다.
    /// <br/> 음수를 입력할 경우 모든 아이템을 반환합니다.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public Item getDividedItems(int count = -1)
    {
        if (this.count > count
            || count < 0)
            count = this.count;

        Item divided = new Item(this);
        this.count -= count;
        divided.count = count;
        return divided;
    }

    /// <summary>
    /// 주어진 갯수만큼 아이템을 버립니다.
    /// </summary>
    /// <param name="count"></param>
    public void discardItem(int count)
    {
        if (count <= 0)
            return;

        if (count > this.count)
        {
            this.count = 0;
            return;
        }

        this.count -= count;
    }

    /// <summary>
    /// 아이템을 합치려고 시도합니다. 한 개라도 성공하면 true를 반환합니다.
    /// <br/> 동일한 아이템이 아니거나 최대 중복 갯수만큼 가득 찼다면 아이템을 추가하지 않고 false를 반환합니다.
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 item 객체에 반영됩니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        if (this.remainCapacity > 0 && this.itemId == item.itemId)
        {
            int getAmount = Math.Min(item.count, this.remainCapacity);
            item.discardItem(getAmount);
            this.count += getAmount;
            return true;
        }
        return false;
    }
}

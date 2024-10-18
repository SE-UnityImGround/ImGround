using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Item
{
    public ItemIdEnum itemId { get; private set; }
    public string name { get; private set; }
    public int count;
    public int maxCount { get; private set; }
    public int remainCapacity { get => (maxCount - count < 0) ? 0 : maxCount - count; }

    /// <summary>
    /// 기존 item 객체로부터 똑같은 Item 객체를 복사합니다.
    /// </summary>
    /// <param name="item">내용을 복사할 Item 객체</param>
    public Item(Item item) : this(item.itemId, item.count)
    {
    }
    
    public Item(ItemIdEnum itemId, int count)
    {
        this.itemId = itemId;
        this.count = count;
        this.name = ItemsInfo.getItemName(itemId);
        this.maxCount = ItemsInfo.getMaxCount(itemId);
    }

    private Item getCopy()
    {
        throw new NotImplementedException();
    }

    public Item getDividedItems(int count)
    {
        if (this.count > count
            || count < 0)
            count = this.count;

        Item divided = getCopy();
        this.count -= count;
        divided.count = count;
        return divided;
    }
}

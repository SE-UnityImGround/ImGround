using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 아이템 묶음을 표현하는 기본 자료구조입니다.
/// </summary>
public class ItemBundle
{
    public Item item { get; private set; }
    public int count { get; private set; }
    public int maxCount { get; private set; }
    /// <summary>
    /// 아이템 중복 수량을 최대 중복 수량만큼 제한할지를 표시합니다.
    /// </summary>
    public bool isLimited { get; private set; }

    public ItemBundle(Item item, int count, bool isLimited)
    {
        this.item = item;
        this.maxCount = ItemInfoManager.getItemInfo(item.itemId).maxCount;
        this.isLimited = isLimited;
        this.count = count;
        if (isLimited && count > maxCount)
        {
            this.count = maxCount;
        }
    }

    public ItemBundle(ItemIdEnum itemId, int count, bool isLimited) : this(new Item(itemId), count, isLimited)
    {
    }

    /// <summary>
    /// 기존 ItemBundle과 똑같은 ItemBundle을 복사합니다.
    /// </summary>
    /// <param name="bundle">내용을 복사할 ItemBundle 객체</param>
    public ItemBundle(ItemBundle bundle) : this(bundle.item.itemId, bundle.count, bundle.isLimited)
    {
    }

    /// <summary>
    /// 해당 아이템에서 주어진 갯수만큼을 덜어내어 반환합니다.
    /// <br/> 만약 주어진 갯수가 초과할 경우 존재하는 만큼을 반환합니다.
    /// <br/> 음수를 입력할 경우 모든 아이템을 반환합니다.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public ItemBundle getDividedItems(int count = -1)
    {
        if (this.count < count
            || count < 0)
            count = this.count;

        ItemBundle divided = new ItemBundle(this);
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
    /// 아이템을 주어진 갯수만큼 추가하려고 시도합니다.
    /// <br/> 주어진 수량 중 추가하지 못한 아이템 수가 반환됩니다.
    /// <br/> 음수가 주어지면 추가하지 않고 그대로 반환합니다.
    /// </summary>
    /// <returns></returns>
    public int addItem(int amount)
    {
        if (amount < 0)
            return amount;

        int addValue = Math.Min(amount, getRemainCapacity());
        this.count += addValue;
        return amount - addValue;
    }

    /// <summary>
    /// 아이템을 합치려고 시도합니다. 한 개라도 성공하면 true를 반환합니다.
    /// <br/> 동일한 아이템이 아니거나 최대 중복 갯수만큼 가득 찼다면 아이템을 추가하지 않고 false를 반환합니다.
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 item 객체에 반영됩니다.
    /// </summary>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public bool addItem(ItemBundle bundle)
    {
        if ((this.count > 0 && this.item.itemId != bundle.item.itemId)
            || getRemainCapacity() == 0)
            return false;

        if (this.item.itemId == ItemIdEnum.TEST_NULL_ITEM)
        {
            this.item = bundle.item;
            this.count = 0;
        }

        int getAmount = bundle.count;
        if (isLimited && getAmount > maxCount)
            getAmount = maxCount;
        bundle.discardItem(getAmount);
        this.count += getAmount;
        return true;
    }

    /// <summary>
    /// 아이템을 추가할 수 있는 남은 수량을 표시합니다. 만약 제한이 없다면 -1을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public int getRemainCapacity()
    {
        if (!isLimited)
            return -1;
        if (count <= maxCount)
            return maxCount - count;
        return 0;
    }
}

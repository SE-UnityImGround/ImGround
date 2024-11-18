using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 인벤토리의 기본 자료구조입니다.
/// </summary>
public class Inventory
{
    public int size { get; private set; }
    public Slot[] slots { get; private set; }
    public  int money { get; private set; }

    public Inventory(int size)
    {
        this.size = size;
        this.slots = new Slot[size];
        for (int i = 0; i < slots.Length; i++)
            slots[i] = new Slot();
    }

    /// <summary>
    /// 아이템을 인벤토리 빈 공간에 추가하려고 시도하며, 한 개 이상의 아이템이 추가되면 true를 반환합니다.
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 item 객체에 반영됩니다.
    /// </summary>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public bool addItem(ItemBundle bundle)
    {
        bool added = false;
        foreach (Slot s in slots)
            if (s.addItem(bundle))
            {
                added = true;
                if (bundle.count == 0)
                    break;
            }
        return added;
    }

    /// <summary>
    /// 모든 슬롯의 아이템을 꺼냅니다.
    /// </summary>
    /// <returns>꺼낸 모든 인벤토리의 아이템</returns>
    public ItemBundle[] popAllItems()
    {
        List<ItemBundle> items = new List<ItemBundle>(size);
        foreach (Slot s in slots)
        {
            if (s.hasItem())
                items.Add(s.bundle);
            s.clear();
        }
        return items.ToArray();
    }
}

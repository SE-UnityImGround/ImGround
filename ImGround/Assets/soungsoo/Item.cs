using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Item
{
    public int count;
    public int maxCount = int.MaxValue;
    public int remainCapacity { get => (count - maxCount < 0) ? 0 : count - maxCount; }

    public bool isSame(Item item)
    {
        throw new NotImplementedException();
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

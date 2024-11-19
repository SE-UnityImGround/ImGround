using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ManufactInfo
{
    public ItemBundle[] inputItems;
    public ItemBundle outputItem;

    public ManufactInfo(ItemBundle[] inputItems, ItemBundle outputItem)
    {
        this.inputItems = inputItems;
        this.outputItem = outputItem;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Quest
{
    public QuestIdEnum id { get; private set; }
    public ItemBundle[] requestItems { get; private set; }
    public ItemBundle[] rewardItems { get; private set; }
    public int rewardMoney { get; private set; }

    public Quest (QuestIdEnum id, ItemBundle[] requestItems, ItemBundle[] rewardItems, int rewardMoney)
    {
        this.id = id;
        this.requestItems = requestItems;
        this.rewardItems = rewardItems;
        this.rewardMoney = rewardMoney;
    }
}

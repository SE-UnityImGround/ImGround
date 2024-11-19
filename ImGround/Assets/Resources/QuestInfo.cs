using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class QuestInfo
{
    public QuestIdEnum questId { get; private set; }
    public Quest quest { get; private set; }
    public UnityEngine.GameObject UIPrefab { get; private set; }

    public QuestInfo(QuestIdEnum id, Quest quest, UnityEngine.GameObject UIPrefab)
    {
        this.questId = id;
        this.quest = quest;
        this.UIPrefab = UIPrefab;
    }
}

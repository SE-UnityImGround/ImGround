using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListBehavior : UIBehavior
{
    [SerializeField]
    private GameObject QuestListView;

    private List<QuestBehavior> currentQuests = new List<QuestBehavior>();

    public override void initialize()
    {
        QuestManager.onQuestAddedHandler += addQuest;
        InventoryManager.onSlotItemChangedHandler += onInventoryUpdate;
    }

    private void addQuest(QuestIdEnum id)
    {
        QuestBehavior quest = Instantiate(QuestInfoManager.getQuestUIPrefab(id), QuestListView.transform).GetComponent<QuestBehavior>();
        quest.initialize();
        quest.onQuestRewardClickHandler += onQuestRewardStart;
        currentQuests.Add(quest);
    }

    private void removeQuest(QuestBehavior q)
    {
        if (currentQuests.Contains(q))
        {
            currentQuests.Remove(q);
            Destroy(q.gameObject);
        }
    }

    private void onQuestRewardStart(QuestBehavior questUI)
    {
        QuestManager.doneQuest(questUI.questID);
        removeQuest(questUI);
    }

    private void onInventoryUpdate(int slotIdx)
    {
        foreach (QuestBehavior quest in currentQuests)
        {
            quest.updateProcess();
        }
    }
}

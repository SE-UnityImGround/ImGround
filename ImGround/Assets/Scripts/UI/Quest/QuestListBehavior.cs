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
        QuestManager.onQuestDoneHandler += removeQuest;
        InventoryManager.onSlotItemChangedHandler += onInventoryUpdate;
    }

    private void addQuest(QuestIdEnum id)
    {
        QuestBehavior quest = Instantiate(QuestInfoManager.getQuestUIPrefab(id), QuestListView.transform).GetComponent<QuestBehavior>();
        quest.initialize();
        quest.onQuestRewardClickHandler += onQuestRewardStart;
        currentQuests.Add(quest);
    }

    private void removeQuest(QuestIdEnum qid)
    {
        for (int i = currentQuests.Count - 1; i >= 0; i--)
        {
            if (currentQuests[i].questID == qid)
            {
                Destroy(currentQuests[i].gameObject);
                currentQuests.RemoveAt(i);
            }
        }
    }

    private void onQuestRewardStart(QuestBehavior questUI)
    {
        QuestManager.rewardQuest(questUI.questID);
    }

    private void onInventoryUpdate(int slotIdx)
    {
        foreach (QuestBehavior quest in currentQuests)
        {
            quest.updateProcess();
        }
    }
}

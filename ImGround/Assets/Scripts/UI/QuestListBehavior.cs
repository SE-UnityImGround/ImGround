using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListBehavior : UIBehavior
{
    [SerializeField]
    private GameObject QuestListView;

    private List<GameObject> currentQuests = new List<GameObject>();

    public override void initialize()
    {
        test();
    }

    private void test()
    {
        foreach (QuestIdEnum id in Enum.GetValues(typeof(QuestIdEnum)))
        {
            addQuest(id);
        }
    }

    private void addQuest(QuestIdEnum id)
    {
        QuestBehavior quest = Instantiate(QuestInfoManager.getQuestUIPrefab(id), QuestListView.transform).GetComponent<QuestBehavior>();
        quest.initialize();
        quest.onQuestRewardClickHandler += onQuestRewardStart;
        currentQuests.Add(quest.gameObject);
    }

    private void removeQuest(QuestBehavior q)
    {
        if (currentQuests.Contains(q.gameObject))
        {
            currentQuests.Remove(q.gameObject);
            Destroy(q.gameObject);
        }
    }

    private void onQuestRewardStart(QuestBehavior questUI)
    {
        QuestManager.doneQuest(questUI.questID);
        removeQuest(questUI);
    }
}

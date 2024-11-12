using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListBehavior : UIBehavior
{
    public GameObject questsListView;
    public GameObject questPreFab;

    public override void initialize()
    {
        testQuest();
    }

    private void testQuest()
    {
        addQuest(new Quest(ImageIdEnum.ICON_ATTACK, "Lorem ipsum dolor sit amet consetetur", ImageIdEnum.ICON_GEM, 2000, 0));
        addQuest(new Quest(ImageIdEnum.ICON_ARCHER, "Lorem ipsum dolor sit amet consetetur", ImageIdEnum.ICON_MEAT, 30, 9, 5));
        addQuest(new Quest(ImageIdEnum.ICON_ATTACK, "test1", ImageIdEnum.ICON_COIN, 512, 10));
        addQuest(new Quest(ImageIdEnum.ICON_GEM, "test2", ImageIdEnum.ICON_COIN, 128, 2));
    }

    /// <summary>
    /// 퀘스트를 추가합니다.
    /// </summary>
    /// <param name="q"></param>
    public void addQuest(Quest q)
    {
        QuestBehavior newQuest = Instantiate(questPreFab, questsListView.transform).GetComponent<QuestBehavior>();
        newQuest.initialize(q);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    private static List<QuestIdEnum> doneList = new List<QuestIdEnum>();

    /// <summary>
    /// 현재 특정 퀘스트가 이미 수행되었는지 여부를 반환합니다.
    /// </summary>
    /// <param name="questid"></param>
    /// <returns></returns>
    public static bool isDone(QuestIdEnum questid)
    {
        foreach (QuestIdEnum done in doneList)
        {
            if (done == questid)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 퀘스트 완료 처리를 진행합니다.
    /// </summary>
    /// <param name="questId"></param>
    public static void doneQuest(QuestIdEnum questId)
    {
        Quest questInfo = QuestInfoManager.getQuestInfo(questId);

        InventoryManager.changeMoney(questInfo.rewardMoney);
        foreach (ItemBundle bundle in questInfo.rewardItems)
        {
            ItemBundle insert = new ItemBundle(bundle);
            InventoryManager.addItems(insert);
            if (insert.count > 0)
            {
                Debug.LogError("퀘스트 아이템을 더 이상 추가하지 못할 때의 처리 로직이 아직 없어요...");
            }
        }

        doneList.Add(questId);
    }
}

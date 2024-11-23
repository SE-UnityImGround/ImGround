using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    // Quest, isDone
    private static Dictionary<QuestIdEnum, bool> questState = new Dictionary<QuestIdEnum, bool>();

    public delegate void onQuestAdded(QuestIdEnum questId);
    public static onQuestAdded onQuestAddedHandler;

    /// <summary>
    /// 퀘스트가 아직 등록되지 않았다면 추가합니다.
    /// </summary>
    /// <param name="questId"></param>
    public static void addQuest(QuestIdEnum questId)
    {
        if (!questState.ContainsKey(questId))
        {
            questState.Add(questId, false);
            onQuestAddedHandler?.Invoke(questId);
        }
    }

    /// <summary>
    /// 특정 퀘스트가 완료되었는지를 확인합니다.
    /// </summary>
    /// <param name="questId"></param>
    /// <returns></returns>
    public static bool isDone(QuestIdEnum questId)
    {
        if (questState.ContainsKey(questId))
        {
            return questState[questId];
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 퀘스트 완료 처리를 진행합니다.
    /// </summary>
    /// <param name="questId"></param>
    public static void rewardQuest(QuestIdEnum questId)
    {
        if (!questState.ContainsKey(questId))
        {
            Debug.LogError("아직 시작되지 않은 퀘스트를 완료처리하려고 시도함.");
            return;
        }

        Quest questInfo = QuestInfoManager.getQuestInfo(questId);

        foreach (ItemBundle item in questInfo.requestItems)
        {
            InventoryManager.removeItem(item.item.itemId, item.count);
        }

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

        questState[questId] = true;
    }
}

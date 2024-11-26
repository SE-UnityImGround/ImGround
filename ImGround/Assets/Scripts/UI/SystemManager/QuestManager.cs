using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    // Quest, isDone, canReward, hasAccepted(완료 후 NPC 상호작용 했는지)
    private static Dictionary<QuestIdEnum, (bool isDone, bool canReward, bool hasAccepted)> questState = new Dictionary<QuestIdEnum, (bool, bool, bool)>();

    public delegate void onQuestEvent(QuestIdEnum questId);
    public static onQuestEvent onQuestAddedHandler;

    public static onQuestEvent onQuestDoneHandler;

    private static bool hasAssignedInventoryEvent = false;

    /// <summary>
    /// 퀘스트가 아직 등록되지 않았다면 추가합니다.
    /// </summary>
    /// <param name="questId"></param>
    public static void addQuest(QuestIdEnum questId)
    {
        if (!questState.ContainsKey(questId))
        {
            questState.Add(questId, (false, false, false));
            onQuestAddedHandler?.Invoke(questId);
        }

        if (!hasAssignedInventoryEvent) 
        {
            InventoryManager.onSlotItemChangedHandler += updateQuestProgress;
            hasAssignedInventoryEvent = true;
        }
    }

    /// <summary>
    /// 퀘스트에 영향을 미치는 요소 (인벤토리 아이템 항목) 에 따라 퀘스트 진행상태를 업데이트합니다.
    /// </summary>
    public static void updateQuestProgress(int slotIdx)
    {
        Dictionary<ItemIdEnum, int> inventoryInfo = InventoryManager.getInventoryInfo();

        QuestIdEnum[] qids = new QuestIdEnum[questState.Keys.Count];
        questState.Keys.CopyTo(qids, 0);
        foreach (QuestIdEnum qid in qids)
        {
            ItemBundle[] requestItems = QuestInfoManager.getQuestInfo(qid).requestItems;
            (bool isDone, bool canReward, bool hasAccepted) questStateInfo = questState[qid];
            questStateInfo.canReward = true;
            for (int i = 0; i < requestItems.Length; i++)
            {
                ItemIdEnum item = requestItems[i].item.itemId;
                if (!inventoryInfo.ContainsKey(item) || inventoryInfo[item] < requestItems[i].count)
                {
                    questStateInfo.canReward = false;
                    break;
                }
            }
            questState[qid] = questStateInfo;
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
            return questState[questId].isDone;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 퀘스트 완료 후 대화를 마쳤음을 표시합니다.
    /// </summary>
    /// <param name="questId"></param>
    public static void setAccepted(QuestIdEnum questId)
    {
        (bool isDone, bool canReward, bool hasAccepted) questStateInfo = questState[questId];
        questStateInfo.hasAccepted = true;
        questState[questId] = questStateInfo;
    }

    /// <summary>
    /// 퀘스트 완료 후 대화를 마쳤는지 여부를 반환합니다.
    /// <br/> 없는 퀘스트라면 false를 반환합니다.
    /// </summary>
    /// <param name="questId"></param>
    /// <returns></returns>
    public static bool hasAccepted(QuestIdEnum questId)
    {
        if (!questState.ContainsKey(questId))
            return false;
        return questState[questId].hasAccepted;
    }

    /// <summary>
    /// 특정 퀘스트의 보상받기 가능 상태를 반환합니다.
    /// <br/> 없는 퀘스트라면 false를 반환합니다.
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="canReward"></param>
    /// <returns></returns>
    public static bool canReward(QuestIdEnum questId)
    {
        if (!questState.ContainsKey(questId))
            return false;
        return questState[questId].isDone || questState[questId].canReward;
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

        (bool isDone, bool canReward, bool hasAccepted) questStateInfo = questState[questId];
        questStateInfo.isDone = true;
        questState[questId] = questStateInfo;
        onQuestDoneHandler?.Invoke(questId);
    }
}

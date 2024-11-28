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
            questState.Add(questId, (false, updateQuestProgress(questId, InventoryManager.getInventoryInfo()), false));
            onQuestAddedHandler?.Invoke(questId);
        }

        if (!hasAssignedInventoryEvent) 
        {
            InventoryManager.onSlotItemChangedHandler += onInventoryChanged;
            hasAssignedInventoryEvent = true;
        }
    }

    public static void onInventoryChanged(int slotIdx)
    {
        Dictionary<ItemIdEnum, int> inventoryInfo = InventoryManager.getInventoryInfo();

        QuestIdEnum[] qids = new QuestIdEnum[questState.Keys.Count];
        questState.Keys.CopyTo(qids, 0);
        foreach (QuestIdEnum qid in qids)
        {
            (bool isDone, bool canReward, bool hasAccepted) questStateInfo = questState[qid];
            questStateInfo.canReward = updateQuestProgress(qid, inventoryInfo);
            questState[qid] = questStateInfo;
        }
    }

    /// <summary>
    /// 퀘스트에 영향을 미치는 요소 (인벤토리 아이템 항목) 에 따라 퀘스트 진행상태를 업데이트합니다.
    /// </summary>
    private static bool updateQuestProgress(QuestIdEnum qid, Dictionary<ItemIdEnum, int> inventoryInfo)
    {
        ItemBundle[] requestItems = QuestInfoManager.getQuestInfo(qid).requestItems;
        bool canReward = true;
        for (int i = 0; i < requestItems.Length; i++)
        {
            ItemIdEnum item = requestItems[i].item.itemId;
            if (!inventoryInfo.ContainsKey(item) || inventoryInfo[item] < requestItems[i].count)
            {
                canReward = false;
                break;
            }
        }
        return canReward;
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
                WarningManager.startWarning();
                ItemThrowManager.throwItem(insert);
            }
        }

        (bool isDone, bool canReward, bool hasAccepted) questStateInfo = questState[questId];
        questStateInfo.isDone = true;
        questState[questId] = questStateInfo;
        onQuestDoneHandler?.Invoke(questId);
    }
}

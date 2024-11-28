using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    // Quest, isDone, canReward, hasAccepted(�Ϸ� �� NPC ��ȣ�ۿ� �ߴ���)
    private static Dictionary<QuestIdEnum, (bool isDone, bool canReward, bool hasAccepted)> questState = new Dictionary<QuestIdEnum, (bool, bool, bool)>();

    public delegate void onQuestEvent(QuestIdEnum questId);
    public static onQuestEvent onQuestAddedHandler;

    public static onQuestEvent onQuestDoneHandler;

    private static bool hasAssignedInventoryEvent = false;

    /// <summary>
    /// ����Ʈ�� ���� ��ϵ��� �ʾҴٸ� �߰��մϴ�.
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
    /// ����Ʈ�� ������ ��ġ�� ��� (�κ��丮 ������ �׸�) �� ���� ����Ʈ ������¸� ������Ʈ�մϴ�.
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
    /// Ư�� ����Ʈ�� �Ϸ�Ǿ������� Ȯ���մϴ�.
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
    /// ����Ʈ �Ϸ� �� ��ȭ�� �������� ǥ���մϴ�.
    /// </summary>
    /// <param name="questId"></param>
    public static void setAccepted(QuestIdEnum questId)
    {
        (bool isDone, bool canReward, bool hasAccepted) questStateInfo = questState[questId];
        questStateInfo.hasAccepted = true;
        questState[questId] = questStateInfo;
    }

    /// <summary>
    /// ����Ʈ �Ϸ� �� ��ȭ�� ���ƴ��� ���θ� ��ȯ�մϴ�.
    /// <br/> ���� ����Ʈ��� false�� ��ȯ�մϴ�.
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
    /// Ư�� ����Ʈ�� ����ޱ� ���� ���¸� ��ȯ�մϴ�.
    /// <br/> ���� ����Ʈ��� false�� ��ȯ�մϴ�.
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
    /// ����Ʈ �Ϸ� ó���� �����մϴ�.
    /// </summary>
    /// <param name="questId"></param>
    public static void rewardQuest(QuestIdEnum questId)
    {
        if (!questState.ContainsKey(questId))
        {
            Debug.LogError("���� ���۵��� ���� ����Ʈ�� �Ϸ�ó���Ϸ��� �õ���.");
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

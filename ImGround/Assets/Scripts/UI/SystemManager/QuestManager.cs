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
    /// ����Ʈ�� ���� ��ϵ��� �ʾҴٸ� �߰��մϴ�.
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
    /// Ư�� ����Ʈ�� �Ϸ�Ǿ������� Ȯ���մϴ�.
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
                Debug.LogError("����Ʈ �������� �� �̻� �߰����� ���� ���� ó�� ������ ���� �����...");
            }
        }

        questState[questId] = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    private static List<QuestIdEnum> foundQuest = new List<QuestIdEnum>();

    public delegate void onQuestAdded(QuestIdEnum questId);
    public static onQuestAdded onQuestAddedHandler;

    /// <summary>
    /// ����Ʈ�� ���� �Ϸ���� �ʾҴٸ� �߰��մϴ�.
    /// </summary>
    /// <param name="questId"></param>
    public static void addQuest(QuestIdEnum questId)
    {
        if (!isFound(questId))
        {
            foundQuest.Add(questId);
            onQuestAddedHandler?.Invoke(questId);
        }
    }

    /// <summary>
    /// ���� Ư�� ����Ʈ�� �̹� ����Ǿ����� ���θ� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="questid"></param>
    /// <returns></returns>
    public static bool isFound(QuestIdEnum questid)
    {
        foreach (QuestIdEnum done in foundQuest)
        {
            if (done == questid)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ����Ʈ �Ϸ� ó���� �����մϴ�.
    /// </summary>
    /// <param name="questId"></param>
    public static void doneQuest(QuestIdEnum questId)
    {
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
    }
}

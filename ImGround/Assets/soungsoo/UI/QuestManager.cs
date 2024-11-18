using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    private static List<QuestIdEnum> doneList = new List<QuestIdEnum>();

    /// <summary>
    /// ���� Ư�� ����Ʈ�� �̹� ����Ǿ����� ���θ� ��ȯ�մϴ�.
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
    /// ����Ʈ �Ϸ� ó���� �����մϴ�.
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
                Debug.LogError("����Ʈ �������� �� �̻� �߰����� ���� ���� ó�� ������ ���� �����...");
            }
        }

        doneList.Add(questId);
    }
}

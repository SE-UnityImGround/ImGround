using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBehavior : MonoBehaviour
{
    private const string TEXT_COMPLETE = "done";

    [SerializeField]
    private Text[] UI_progressText;
    [SerializeField]
    private Button UI_Claim;
    [SerializeField]
    public QuestIdEnum questID;

    public delegate void onQuestRewardClick(QuestBehavior questUI);
    public onQuestRewardClick onQuestRewardClickHandler;

    /// <summary>
    /// ����Ʈ ��ü ��Ͻ� �ʱ�ȭ!
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="slotIdx"></param>
    public void initialize()
    {
        updateProcess();
    }

    /// <summary>
    /// ����Ʈ ���� ��Ȳ�� �����մϴ�.
    /// </summary>
    public void updateProcess()
    {
        bool done = true;
        ItemBundle[] requestItems = QuestInfoManager.getQuestInfo(questID).requestItems;
        for (int i = 0; i < requestItems.Length; i++)
        {
            int itemCount = InventoryManager.getItemAmount(requestItems[i].item.itemId);
            int reqestCount = requestItems[i].count;
            if (reqestCount <= itemCount)
            {
                UI_progressText[i].text = TEXT_COMPLETE;
            }
            else
            {
                UI_progressText[i].text = string.Format("({0}/{1})", itemCount, reqestCount);
                done = false;
            }
        }

        UI_Claim.interactable = done;
    }

    public void onClickReward()
    {
        onQuestRewardClickHandler.Invoke(this);
    }
}

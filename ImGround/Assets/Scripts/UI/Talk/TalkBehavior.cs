using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ȭâ UI�� �����ϴ� ��ũ��Ʈ�Դϴ�.
/// </summary>
public class TalkBehavior : UIBehavior
{
    [SerializeField]
    private InGameViewBehavior inGameUI;
    [SerializeField]
    private Image TalkerCircle;
    [SerializeField]
    private Text TalkerName;
    [SerializeField]
    private Text TalkText;
    [SerializeField]
    private GameObject AnswerView;
    [SerializeField]
    private GameObject AnswerList;
    [SerializeField]
    private GameObject AnswerItemPrefab;

    private NPCBehavior talkingNPC;
    private Sprite talkerBackground;
    private TalkInfo currentTalk;
    private List<AnswerBehavior> currentAnswers = new List<AnswerBehavior>();
    private AnswerItemManager answersManager;

    private void checkValue(object v, string name)
    {
        if (v == null)
        {
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, name);
            return;
        }
    }

    public override void initialize()
    {
        checkValue(inGameUI, nameof(inGameUI));
        checkValue(TalkerCircle, nameof(TalkerCircle));
        checkValue(TalkerName, nameof(TalkerName));
        checkValue(TalkText, nameof(TalkText));
        checkValue(AnswerView, nameof(AnswerView));
        checkValue(AnswerList, nameof(AnswerList));
        checkValue(AnswerItemPrefab, nameof(AnswerItemPrefab));

        answersManager = new AnswerItemManager(AnswerItemPrefab, nextTalk);
    }

    /// <summary>
    /// ��ȭ�� �����մϴ�.
    /// </summary>
    public void startTalk(NPCBehavior talker)
    {
        if (talkingNPC != null)
        {
            talkingNPC.setTalkingState(false);
        }

        if (talker != null && talker.type != NPCType.NPC_NORMAL)
        {
            talkingNPC = talker;
            talkingNPC.setTalkingState(true);
            talkerBackground = ImageManager.getImage(TalkInfoManager.getTalkerBackground(talkingNPC.type));

            QuestIdEnum qid = QuestInfoManager.getQuestId(talker.type);
            if (qid == QuestIdEnum.NULL)
                currentTalk = TalkInfoManager.getTalkInfo(talkingNPC.type);
            else
            {
                if (!QuestManager.isDone(qid))
                {
                    if (QuestManager.canReward(qid))
                    {
                        currentTalk = TalkInfoManager.getQuestDoneTalkInfo(talkingNPC.type);
                        QuestManager.setAccepted(qid);
                        QuestManager.rewardQuest(qid);
                    }
                    else
                    {
                        currentTalk = TalkInfoManager.getTalkInfo(talkingNPC.type);
                    }
                }
                else
                {
                    if (QuestManager.hasAccepted(qid))
                        currentTalk = TalkInfoManager.getSmallTalkInfo();
                    else
                    {
                        currentTalk = TalkInfoManager.getQuestDoneTalkInfo(talkingNPC.type);
                        QuestManager.setAccepted(qid);
                    }
                }
            }
            updateTalkView();
            UISoundManager.playUiSound(UISoundObject.START_TALKING);
            inGameUI.displayView(InGameViewMode.TALK);
        }
        else
        {
            talkingNPC = null;
            talkerBackground = null;
            currentTalk = null;
        }
    }

    /// <summary>
    /// ���� ��ȭ���� Ư�� ������� ��ȭ�� �����մϴ�.
    /// </summary>
    /// <param name="answerIdx"></param>
    private void nextTalk(int answerIdx)
    {
        TalkEventEnum eventType = currentTalk.answerData[answerIdx].Item2;
        TalkInfo[] nextTalks = currentTalk.answerData[answerIdx].Item3;
        
        // ����� �̺�Ʈ ó��
        if (eventType == TalkEventEnum.END)
        {
            nextTalks = null;
        }
        if (eventType == TalkEventEnum.NEW_QUEST)
        {
            QuestManager.addQuest(QuestInfoManager.getQuestId(talkingNPC.type));
        }
        if (eventType == TalkEventEnum.OPEN_SHOP)
        {
            inGameUI.getUIBehavior<ShopBehavior>().setShopView(ShopInfoManager.getShopInfo(talkingNPC.type), onChildViewClose);
            inGameUI.displayView(InGameViewMode.SHOP);
            inGameUI.hideView(InGameViewMode.TALK);
        }
        if (eventType == TalkEventEnum.OPEN_MANUFACT)
        {
            inGameUI.getUIBehavior<ManufactListBehavior>().setManufact(onChildViewClose);
            inGameUI.displayView(InGameViewMode.MANUFACT);
            inGameUI.hideView(InGameViewMode.TALK);
        }

        // ���� ��ȭ ó��
        if (nextTalks == null)
        {
            talkingNPC.setTalkingState(false);
            talkingNPC = null;
            currentTalk = null;
            inGameUI.hideView(InGameViewMode.TALK);
        }
        else
        {
            currentTalk = nextTalks[Random.Range(0, nextTalks.Length)];
        }
        updateTalkView();
    }

    /// <summary>
    /// ��ȭ �����͸� UI�� �����մϴ�.
    /// </summary>
    private void updateTalkView()
    {
        removeAnswerItems();
        if (currentTalk == null)
        {
            TalkerName.text = "";
            TalkText.text = "";
        }
        else
        {
            TalkerName.text = talkingNPC.NPCName;
            TalkerCircle.sprite = talkerBackground;
            TalkText.text = currentTalk.context;

            int idx = 0;
            foreach ((string, TalkEventEnum, TalkInfo[]) data in currentTalk.answerData)
            {
                attachAnswerItem(data.Item1, idx++);
            }
        }
    }

    private void removeAnswerItems()
    {
        foreach (AnswerBehavior answer in currentAnswers)
        {
            answer.transform.SetParent(null, false);
            answersManager.recycleAnswerItem(answer);
        }
        currentAnswers.Clear();
    }

    private void attachAnswerItem(string answer, int idx)
    {
        AnswerBehavior newitem = answersManager.getAnswerItem();
        newitem.gameObject.transform.SetParent(AnswerList.transform, false);
        newitem.setUp(answer, idx);
        currentAnswers.Add(newitem);
    }

    /// <summary>
    /// ��ȭ �� ���� â�� ������ ó���ϴ� �۾��Դϴ�.
    /// </summary>
    private void onChildViewClose()
    {
        if (currentTalk != null)
        {
            inGameUI.displayView(InGameViewMode.TALK);
        }
    }
}

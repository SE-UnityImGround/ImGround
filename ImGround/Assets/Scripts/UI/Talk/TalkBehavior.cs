using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 대화창 UI를 관리하는 스크립트입니다.
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
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
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
    /// 대화를 시작합니다.
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
    /// 현재 대화에서 특정 대답으로 대화를 진행합니다.
    /// </summary>
    /// <param name="answerIdx"></param>
    private void nextTalk(int answerIdx)
    {
        TalkEventEnum eventType = currentTalk.answerData[answerIdx].Item2;
        TalkInfo[] nextTalks = currentTalk.answerData[answerIdx].Item3;
        
        // 대답의 이벤트 처리
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

        // 다음 대화 처리
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
    /// 대화 데이터를 UI에 갱신합니다.
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
    /// 대화 중 보조 창이 닫히면 처리하는 작업입니다.
    /// </summary>
    private void onChildViewClose()
    {
        if (currentTalk != null)
        {
            inGameUI.displayView(InGameViewMode.TALK);
        }
    }
}

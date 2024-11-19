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
    private InGameViewBehavior inGameView;
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
        checkValue(inGameView, nameof(inGameView));
        checkValue(TalkerCircle, nameof(TalkerCircle));
        checkValue(TalkerName, nameof(TalkerName));
        checkValue(TalkText, nameof(TalkText));
        checkValue(AnswerView, nameof(AnswerView));
        checkValue(AnswerList, nameof(AnswerList));
        checkValue(AnswerItemPrefab, nameof(AnswerItemPrefab));

        answersManager = new AnswerItemManager(AnswerItemPrefab, onAnswerSelected);
        TalkManager.onTalkChangedHandler += onTalkDataUpdated;
    }

    /// <summary>
    /// ��ȭ �����Ͱ� ������Ʈ�Ǹ� UI�� �����ϴ� �̺�Ʈ ó���� �Դϴ�.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="talkInfo"></param>
    private void onTalkDataUpdated(string name, ImageIdEnum talkerBackground, TalkInfo talkInfo)
    {
        removeAnswerItems();
        currentTalk = talkInfo;
        if (talkInfo == null)
        {
            TalkerName.text = "";
            TalkText.text = "";
            inGameView.displayView(InGameViewMode.DEFAULT);
        }
        else
        {
            TalkerName.text = name;
            TalkerCircle.sprite = ImageManager.getImage(talkerBackground);
            TalkText.text = talkInfo.context;

            int idx = 0;
            foreach ((string, TalkEventEnum, TalkInfo[]) data in talkInfo.answerData)
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
    /// ����� ���õǾ��� �� �߻��ϴ� �̺�Ʈ�� ó�����Դϴ�.
    /// </summary>
    /// <param name="answerIdx"></param>
    private void onAnswerSelected(int answerIdx)
    {
        Debug.Log("�亯 " + answerIdx + " Ŭ��! (�̺�Ʈ : " + currentTalk.answerData[answerIdx].Item2.ToString() + ")");
        TalkManager.nextTalk(answerIdx);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkBehavior : UIBehavior
{
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
        checkValue(TalkerCircle, nameof(TalkerCircle));
        checkValue(TalkerName, nameof(TalkerName));
        checkValue(TalkText, nameof(TalkText));
        checkValue(AnswerView, nameof(AnswerView));
        checkValue(AnswerList, nameof(AnswerList));
        checkValue(AnswerItemPrefab, nameof(AnswerItemPrefab));

        answersManager = new AnswerItemManager(AnswerItemPrefab, onAnswerSelected);
    }

    public void setUp()
    {
        // test
        attachAnswerItem("답변1", 1);
        attachAnswerItem("답변2", 2);
    }

    private void attachAnswerItem(string answer, int idx)
    {
        AnswerBehavior newitem = answersManager.getAnswerItem();
        newitem.gameObject.transform.SetParent(AnswerList.transform, false);
        newitem.setUp(answer, idx);
    }

    private void onAnswerSelected(int answerIdx)
    {
        Debug.Log("답변 " + answerIdx + " 클릭!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

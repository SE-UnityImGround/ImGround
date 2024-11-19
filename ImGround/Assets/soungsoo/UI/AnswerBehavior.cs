using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerBehavior : MonoBehaviour
{
    private const string POINTED = "->";

    [SerializeField]
    private Text textView;
    [SerializeField]
    private string answerText;

    private int answerIdx;

    public delegate void AnswerSelectedEvent(int answerIdx);
    public AnswerSelectedEvent AnswerSelectedEventHandler;

    public void initialize()
    {
        if (textView == null)
        {
            Debug.LogError(gameObject.name + " UI에 텍스트 컴포넌트가 등록되지 않음!");
        }
    }

    public void setUp(string answer, int answerIdx)
    {
        this.answerText = answer;
        textView.text = answer;
        this.answerIdx = answerIdx;
    }

    public void onClick()
    {
        AnswerSelectedEventHandler.Invoke(answerIdx);
    }

    public void onPointerEnter()
    {
        textView.text = POINTED + answerText;
    }

    public void onPointerExit()
    {
        textView.text = answerText;
    }
}

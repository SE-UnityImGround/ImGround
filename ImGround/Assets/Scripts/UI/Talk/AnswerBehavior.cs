using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ȭâ �� ���� ��� �ؽ�Ʈ UI�� �����ϴ� ��ũ��Ʈ�Դϴ�.
/// </summary>
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
            Debug.LogError(gameObject.name + " UI�� �ؽ�Ʈ ������Ʈ�� ��ϵ��� ����!");
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
        AnswerSelectedEventHandler?.Invoke(answerIdx);
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

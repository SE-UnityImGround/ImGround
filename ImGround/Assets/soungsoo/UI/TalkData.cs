
using System;

public class TalkData
{
    /// <summary>
    /// 표시할 대화 본문입니다.
    /// </summary>
    public string text;
    /// <summary>
    /// 대화의 대답 별 (대답, 대답시 수행할 함수, 대답 이후 연결되는 대화) 쌍입니다.
    /// </summary>
    public (string, Action, TalkData)[] answers;

    public TalkData(string text, params (string, Action, TalkData)[] answers)
    {
        this.text = text;
        this.answers = answers;
    }
}

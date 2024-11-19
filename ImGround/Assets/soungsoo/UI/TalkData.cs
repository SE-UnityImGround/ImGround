
using System;

public class TalkData
{
    /// <summary>
    /// ǥ���� ��ȭ �����Դϴ�.
    /// </summary>
    public string text;
    /// <summary>
    /// ��ȭ�� ��� �� (���, ���� ������ �Լ�, ��� ���� ����Ǵ� ��ȭ) ���Դϴ�.
    /// </summary>
    public (string, Action, TalkData)[] answers;

    public TalkData(string text, params (string, Action, TalkData)[] answers)
    {
        this.text = text;
        this.answers = answers;
    }
}

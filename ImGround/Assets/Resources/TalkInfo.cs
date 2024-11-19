using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 대화 정보입니다.
/// </summary>
public class TalkInfo
{
    public string context;
    /// <summary>
    /// 대답 문구, 대답시 진행 이벤트, 이벤트 이후 넘어갈 대화(1개 랜덤 택)
    /// </summary>
    public (string, TalkEventEnum, TalkInfo[])[] answerData;

    /// <summary>
    /// 본문, 
    /// <br/>대답 문구, 대답시 진행 이벤트, 이벤트 이후 넘어갈 대화(1개 랜덤 택)
    /// </summary>
    /// <param name="context"></param>
    /// <param name="answerData"></param>
    public TalkInfo(string context, params (string, TalkEventEnum, TalkInfo[])[] answerData)
    {
        this.context = context;
        this.answerData = answerData;
    }
}

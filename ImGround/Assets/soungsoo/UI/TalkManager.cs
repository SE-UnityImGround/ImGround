using UnityEngine;

public class TalkManager
{
    private static NPCBehavior talkingNPC;
    private static TalkInfo talk;

    public delegate void onTalkChanged(string name, ImageIdEnum talkerBackground, TalkInfo talkInfo);
    public static onTalkChanged onTalkChangedHandler;

    /// <summary>
    /// 대화를 처음 설정합니다.
    /// </summary>
    /// <param name="npcName"></param>
    /// <param name="type"></param>
    public static void setTalk(NPCBehavior talker)
    {
        if (talkingNPC != null)
        {
            talkingNPC.setTalkingState(false);
        }
        talkingNPC = talker;
        TalkManager.talk = TalkInfoManager.getTalkInfo(talkingNPC.type);
        onTalkChangedHandler?.Invoke(talkingNPC.NPCName, TalkInfoManager.getTalkerBackground(talkingNPC.type), talk);
        talkingNPC.setTalkingState(true);
    }

    /// <summary>
    /// 현재 대화에서 특정 대답으로 대화를 진행합니다.
    /// </summary>
    /// <param name="answerIdx"></param>
    public static void nextTalk(int answerIdx)
    {
        TalkEventEnum eventType = talk.answerData[answerIdx].Item2;
        if (eventType == TalkEventEnum.END)
        {
            talk = null;
            talkingNPC.setTalkingState(false);
            talkingNPC = null;
        }
        else
        {
            if (eventType == TalkEventEnum.NEW_QUEST)
            {
                QuestManager.addQuest(TalkInfoManager.getQuestId(talkingNPC.type));
            }
            if (eventType == TalkEventEnum.OPEN_SHOP)
            {
                // 아직 구현 안됨.
            }

            TalkInfo[] infos = talk.answerData[answerIdx].Item3;
            if (infos == null)
            {
                talk = null;
                talkingNPC.setTalkingState(false);
                talkingNPC = null;
            }
            else
            {
                talk = infos[Random.Range(0, infos.Length)];
            }
        }
        onTalkChangedHandler?.Invoke(talkingNPC?.NPCName, TalkInfoManager.getTalkerBackground(talkingNPC == null ? NPCType.NPC_1 : talkingNPC.type), talk);
    }
}

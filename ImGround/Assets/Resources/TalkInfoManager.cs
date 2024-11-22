using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TalkInfoManager
{
    /*=======================================
     *            상점 대화 데이터
     *=======================================*/

    // 가공 상점
    private static TalkInfo INDUSTRY_2 = new TalkInfo(
        "넵! 감사합니다!",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo INDUSTRY_1 = new TalkInfo(
        "응? 다들 모르는 건 나한테 물어보라고 했다고? ······. 아, 이 문제는······.",
        ("확인", TalkEventEnum.OPEN_MANUFACT, new TalkInfo[] { INDUSTRY_2 }));

    // 생선 상점
    private static TalkInfo FISH_3 = new TalkInfo(
        "음... 아무튼 찾으면 꼭 얘기해주세요!\n별 사람이 다 있네 흠흠",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo FISH_2b = new TalkInfo(
        "(누군가 우리 가게의 청새치를 가져가서 무기로 쓴다는 얘기를 들었어요. 혹시 보신 적 있나요…?)",
        ("엥? 진짜요?", TalkEventEnum.NONE, new TalkInfo[] { FISH_3 }),
        ("아.. 설마요", TalkEventEnum.NONE, new TalkInfo[] { FISH_3 }));
    private static TalkInfo FISH_2a = new TalkInfo(
        "좋습니닷! 필요한 게 있으시면 언제든지 들러 주세요!",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo FISH_1 = new TalkInfo(
        "오셨네요! ㅎㅎ 편하게 둘러보고 가세요~",
        ("확인", TalkEventEnum.OPEN_SHOP, new TalkInfo[] { FISH_2a, FISH_2b }));

    // 과일 상점
    private static TalkInfo FRUIT_2 = new TalkInfo(
        "다음에 또 올 거지? 꼭 와야 돼! 기다릴게!!",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo FRUIT_1 = new TalkInfo(
        "우왓, 잘 왔어~! 내가 도울 만한 게 있을까??",
        ("확인", TalkEventEnum.OPEN_SHOP, new TalkInfo[] { FRUIT_2 }));

    // 빵 상점
    private static TalkInfo BREAD_2 = new TalkInfo(
        "잘 가~~~",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo BREAD_1 = new TalkInfo(
        "엇, 반가워! 여기는 어쩐 일이야?",
        ("확인", TalkEventEnum.OPEN_SHOP, new TalkInfo[] { BREAD_2 }));

    // 고기 상점
    private static TalkInfo MEAT_2 = new TalkInfo(
        "도움이 됐으면 좋겠어. 안녕!",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo MEAT_1 = new TalkInfo(
        "안녕, 스테이크가 필요하다고?",
        ("확인", TalkEventEnum.OPEN_SHOP, new TalkInfo[] { MEAT_2 }));

    // 판매 상점
    private static TalkInfo SELLER_2 = new TalkInfo(
        "안녕히 가세요.",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo SELLER_1 = new TalkInfo(
        "······. 안녕하세요.",
        ("확인", TalkEventEnum.OPEN_SHOP, new TalkInfo[] { SELLER_2 }));

    // 도구 상점
    private static TalkInfo TOOL_2 = new TalkInfo(
        "좋은 하루 되세요~",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo TOOL_1 = new TalkInfo(
        "좋은 하루에요~",
        ("확인", TalkEventEnum.OPEN_SHOP, new TalkInfo[] { TOOL_2 }));

    /*=======================================
     *            퀘스트 대화 데이터
     *=======================================*/

    // 퀘스트 1
    private static TalkInfo QUEST1_4b = new TalkInfo(
        "힝..",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST1_4a = new TalkInfo(
        "고마워! 프로포즈 성공하면 꼭 한 턱 쏠게!!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST1_3 = new TalkInfo(
        "내가 곧 여자 친구랑 7주년이라 프로포즈를 할 계획인데······, 아직 준비를 덜 했어!! \n내가 꽃다발을 준비할 테니, 너는 금목걸이를 만들어 가져다 주겠니 ?",
        ("맡겨만 줘!", TalkEventEnum.NONE, new TalkInfo[] { QUEST1_4a }),
        ("너 알아서 해", TalkEventEnum.NONE, new TalkInfo[] { QUEST1_4b }));
    private static TalkInfo QUEST1_2 = new TalkInfo(
        "어, 안녕! 혹시 시간 있으면 나 좀 도와줄래?",
        ("도와줄게", TalkEventEnum.NONE, new TalkInfo[] { QUEST1_3 }),
        ("무시하기", TalkEventEnum.END, null));
    private static TalkInfo QUEST1_1 = new TalkInfo(
        "아, 어쩌지? 큰일이네······.",
        ("무슨 일이야?", TalkEventEnum.NONE, new TalkInfo[] { QUEST1_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 2
    private static TalkInfo QUEST2_4b = new TalkInfo(
        "싫으면 말아라 뭐. (히히 이따가 배달시켜먹을까?)",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST2_4a = new TalkInfo(
        "우왓, 고마워! 그나저나 나 혼자 이걸 다 먹은 건······ 평생 우리 둘만 아는 비밀인 거야!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST2_3 = new TalkInfo(
        "역시 둘 다 먹는 게 좋겠지? 배고픈 나를 위해 피자와 햄버거를 사다 줘!",
        ("응? 응···.", TalkEventEnum.NONE, new TalkInfo[] { QUEST2_4a }),
        ("둘 다 안 먹는 게···.", TalkEventEnum.NONE, new TalkInfo[] { QUEST2_4b }));
    private static TalkInfo QUEST2_2 = new TalkInfo(
        "나 오늘 치팅데이거든! 피자랑 햄버거가 너무 먹고 싶었어. 둘 중 뭘 먹는 게 좋을까? 음···.",
        ("피자", TalkEventEnum.NONE, new TalkInfo[] { QUEST2_3 }),
        ("햄버거", TalkEventEnum.NONE, new TalkInfo[] { QUEST2_3 }),
        ("무시하기", TalkEventEnum.END, null));
    private static TalkInfo QUEST2_1 = new TalkInfo(
        "뭘 먹는 게 좋을까······.",
        ("뭐 고민해?", TalkEventEnum.NONE, new TalkInfo[] { QUEST2_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 3
    private static TalkInfo QUEST3_3b = new TalkInfo(
        "뭐래. (쟤 오늘 기분 나쁜가...?)",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST3_3a = new TalkInfo(
        "고마워! ······. 아, 그런데 이걸로는 배가 안 차네. 아무래도 치킨을 시켜야······.",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST3_2 = new TalkInfo(
        "그래서 이번에는 작심삼일 말고, 진짜 다이어트 도전할 거야. 아, 그나저나 못 먹어서 힘이 없는데···. 나 대신 과일샐러드 좀 구해다 줄 수 있니?",
        ("조금만 기다려", TalkEventEnum.NONE, new TalkInfo[] { QUEST3_3a }),
        ("작심하루겠지", TalkEventEnum.NONE, new TalkInfo[] { QUEST3_3b }));
    private static TalkInfo QUEST3_1 = new TalkInfo(
        "나 있지, 오늘 몸무게 재고 충격받았어···.",
        ("저런······.", TalkEventEnum.NONE, new TalkInfo[] { QUEST3_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 4
    private static TalkInfo QUEST4_4b = new TalkInfo(
        "아유.. 그래 미안하구나...",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST4_4a = new TalkInfo(
        "아유 고마워 학생~!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST4_3 = new TalkInfo(
        "이를 어쩌나······. 저기 학생, 급한 대로 치즈와 빵이라도 좀 구해다 줄 수 있니?",
        ("네 그러죠", TalkEventEnum.NONE, new TalkInfo[] { QUEST4_4a }),
        ("좀 바빠요", TalkEventEnum.NONE, new TalkInfo[] { QUEST4_4b }));
    private static TalkInfo QUEST4_2 = new TalkInfo(
        "아휴, 우리 딸이 치즈 케이크를 먹고 싶다고 울어. 학생, 혹시 구할 곳이 있을까?",
        ("저희 동네에 없어요", TalkEventEnum.NONE, new TalkInfo[] { QUEST4_3 }),
        ("무시하기", TalkEventEnum.END, null));
    private static TalkInfo QUEST4_1 = new TalkInfo(
        "아이고, 이를 어디서 구하나······.",
        ("무슨 일이세요?", TalkEventEnum.NONE, new TalkInfo[] { QUEST4_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 5
    private static TalkInfo QUEST5_3b = new TalkInfo(
        "으잉? 어른 말씀하는데 무시하고 저저....",
        ("얼른 도망치자", TalkEventEnum.END, null));
    private static TalkInfo QUEST5_3a = new TalkInfo(
        "맛있는걸로 부탁하마!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST5_2 = new TalkInfo(
        "내가 말이야! 매일 아침 사과주스를 마셔야 하는데, 나참··· 오늘은 바빠서 그럴 시간이 없더라고. 나는 가게를 지켜야 하니 대신 사과주스 좀 구해다 주겠나?",
        ("네..", TalkEventEnum.NONE, new TalkInfo[] { QUEST5_3a }),
        ("무시하기", TalkEventEnum.NONE, new TalkInfo[] { QUEST5_3b }));
    private static TalkInfo QUEST5_1 = new TalkInfo(
        "어이, 거기!!",
        ("네, 네? 저요?", TalkEventEnum.NONE, new TalkInfo[] { QUEST5_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 6
    private static TalkInfo QUEST6_3 = new TalkInfo(
        "오! 고맙구나! 잘 마실게!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST6_2 = new TalkInfo(
        "뭐야! 품절이라고? 어이 자네, 혹시 바나나우유 좀 가지고 있나?",
        ("있긴 한데······.", TalkEventEnum.NONE, new TalkInfo[] { QUEST6_3 }),
        ("으아 내꺼야 도망쳐", TalkEventEnum.END, null));
    private static TalkInfo QUEST6_1 = new TalkInfo(
        "어으~ 사우나 후에는 역시 바나나우유지. 오늘도 마셔 볼까~",
        ("그거 품절이에요", TalkEventEnum.NONE, new TalkInfo[] { QUEST6_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 7
    private static TalkInfo QUEST7_3b = new TalkInfo(
        "응~ 싫어~",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST7_3a = new TalkInfo(
        "좋아! 수박주스 2잔 부탁해~",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST7_2 = new TalkInfo(
        "너무 더워서 수박이랑 수박주스 좀 먹어야겠어! 어때? 같이 먹을래?",
        ("응··· 설마 내가 다 준비해?", TalkEventEnum.NONE, new TalkInfo[] { QUEST7_3a }),
        ("귀찮아 네가 가져와", TalkEventEnum.NONE, new TalkInfo[] { QUEST7_3b }));
    private static TalkInfo QUEST7_1 = new TalkInfo(
        "흐아, 이번 여름은 유독 긴 것 같아······.",
        ("인정해", TalkEventEnum.NONE, new TalkInfo[] { QUEST7_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 8
    private static TalkInfo QUEST8_3 = new TalkInfo(
        "우와 고마워! 기대할게!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST8_2 = new TalkInfo(
        "그렇지만 다음에도 수업이 있어서 매점을 갈 수가 없어. ㅠㅠ 아, 누가 달달한 걸 좀 사다 줬으면 좋겠는데······.",
        ("여기 나밖에 없는데?", TalkEventEnum.NONE, new TalkInfo[] { QUEST8_3 }),
        ("난 모르겠네 그럼 이만···", TalkEventEnum.END, null));
    private static TalkInfo QUEST8_1 = new TalkInfo(
        "우리 과학 선생님은 말이 너무 많아. 현기증 날 것 같아······.",
        ("안타깝게 됐네", TalkEventEnum.NONE, new TalkInfo[] { QUEST8_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 9
    private static TalkInfo QUEST9_3b = new TalkInfo(
        "오.. 차가운 녀석...",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST9_3a = new TalkInfo(
        "고마워! 꼭 보답할게!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST9_2 = new TalkInfo(
        "닭가슴살 샐러드? 오, 좋은데? 그거 어디서 파는지 알아? ······. 미안한데, 나 지금 당장 운동하러 가야 하거든? 그동안 좀 사다 줄 수 있어?",
        ("이번만이야", TalkEventEnum.NONE, new TalkInfo[] { QUEST9_3a }),
        ("미안 나도 시간이 없어서", TalkEventEnum.NONE, new TalkInfo[] { QUEST9_3b }));
    private static TalkInfo QUEST9_1 = new TalkInfo(
        "아, 근손실은 안 돼!! 그렇지만 오늘은 닭가슴살만 먹고 싶지 않아···. 뭔가 새로운 게 없을까?",
        ("닭가슴살 샐러드?", TalkEventEnum.NONE, new TalkInfo[] { QUEST9_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 10
    private static TalkInfo QUEST10_3b = new TalkInfo(
        "잉.. 생선...",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST10_3a = new TalkInfo(
        "나 고등어랑 연어 좀 갖다줘! 한번 멋있어보자!",
        ("엥 내가..?", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST10_2 = new TalkInfo(
        "좀 무섭긴 한데, 탐나는 것 같기도 하고? 음···, 나도 생선 같은 걸 들고 다니면 어떨까?",
        ("한번 해 봐", TalkEventEnum.NONE, new TalkInfo[] { QUEST10_3a }),
        ("그건 그 사람만 소화 가능해", TalkEventEnum.NONE, new TalkInfo[] { QUEST10_3b }));
    private static TalkInfo QUEST10_1 = new TalkInfo(
        "아니 글쎄, 마을에 청새치를 들고 다니는 사람이 있대. 너도 알아?",
        ("어어, 너무 잘 알지. 하하.", TalkEventEnum.NONE, new TalkInfo[] { QUEST10_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 11
    private static TalkInfo QUEST11_3b = new TalkInfo(
        "... 너무해. 너 잘났다 그래.",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST11_3a = new TalkInfo(
        "우와 고마워! 기대할게!",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST11_2 = new TalkInfo(
        "우와아······. 나도 양꼬치 먹어 보고 싶어! 양꼬치··· 양꼬치······.",
        ("내가 구해 올게", TalkEventEnum.NONE, new TalkInfo[] { QUEST11_3a }),
        ("언젠가는 먹겠지", TalkEventEnum.NONE, new TalkInfo[] { QUEST11_3b }));
    private static TalkInfo QUEST11_1 = new TalkInfo(
        "저기! 있잖아, 이 세상에 양꼬치라는 것도 있대···. 너도 먹어 봤어??",
        ("당연하지", TalkEventEnum.NONE, new TalkInfo[] { QUEST11_2 }),
        ("무시하기", TalkEventEnum.END, null));

    // 퀘스트 12
    private static TalkInfo QUEST12_3b = new TalkInfo(
        "엥 누가 퍼뜨렸대니",
        ("확인", TalkEventEnum.END, null));
    private static TalkInfo QUEST12_3a = new TalkInfo(
        "우와 역시! 소문대로 잘하나봐!! 기대할게~",
        ("확인", TalkEventEnum.NEW_QUEST, null));
    private static TalkInfo QUEST12_2 = new TalkInfo(
        "네가 이 마을에서 요리를 가장 잘한다는 소문을 들었어. 나를 도와 음식을 좀 만들어 줄 수 있을까?",
        ("잘 찾아왔어! 나만 믿어", TalkEventEnum.NONE, new TalkInfo[] { QUEST12_3a }),
        ("처음 듣는 소문인데······.", TalkEventEnum.NONE, new TalkInfo[] { QUEST12_3b }));
    private static TalkInfo QUEST12_1 = new TalkInfo(
        "내가 오늘 친구들이랑 파티를 하기로 했거든~",
        ("그래서?", TalkEventEnum.NONE, new TalkInfo[] { QUEST12_2 }),
        ("무시하기", TalkEventEnum.END, null));

    /*=======================================
     *          npc type 매핑 데이터
     *=======================================*/

    private static Dictionary<NPCType, TalkInfo> talkInfos = new Dictionary<NPCType, TalkInfo>()
        {
            { NPCType.SHOP_INDUSTRY, INDUSTRY_1 },
            { NPCType.SHOP_FISH, FISH_1},
            { NPCType.SHOP_FRUIT, FRUIT_1},
            { NPCType.SHOP_BREAD, BREAD_1},
            { NPCType.SHOP_MEAT, MEAT_1},
            { NPCType.SHOP_SELLER, SELLER_1},
            { NPCType.SHOP_TOOL, TOOL_1},

            { NPCType.NPC_1, QUEST1_1},
            { NPCType.NPC_2, QUEST2_1},
            { NPCType.NPC_3, QUEST3_1},
            { NPCType.NPC_4, QUEST4_1},
            { NPCType.NPC_5, QUEST5_1},
            { NPCType.NPC_6, QUEST6_1},
            { NPCType.NPC_7, QUEST7_1},
            { NPCType.NPC_8, QUEST8_1},
            { NPCType.NPC_9, QUEST9_1},
            { NPCType.NPC_10, QUEST10_1},
            { NPCType.NPC_11, QUEST11_1},
            { NPCType.NPC_12, QUEST12_1 }
        };

    /*=======================================
     *          외부 지원 메소드
     *=======================================*/

    public static TalkInfo getTalkInfo(NPCType type)
    {
        if (talkInfos.ContainsKey(type))
        {
            return talkInfos[type];
        }
        throw new Exception("NPC " + type.ToString() + "에 대한 대화 데이터가 없습니다!");
    }

    public static QuestIdEnum getQuestId(NPCType type)
    {
        switch (type)
        {
            case NPCType.NPC_1:
                return QuestIdEnum.Q_1;
            case NPCType.NPC_2:
                return QuestIdEnum.Q_2;
            case NPCType.NPC_3:
                return QuestIdEnum.Q_3;
            case NPCType.NPC_4:
                return QuestIdEnum.Q_4;
            case NPCType.NPC_5:
                return QuestIdEnum.Q_5;
            case NPCType.NPC_6:
                return QuestIdEnum.Q_6;
            case NPCType.NPC_7:
                return QuestIdEnum.Q_7;
            case NPCType.NPC_8:
                return QuestIdEnum.Q_8;
            case NPCType.NPC_9:
                return QuestIdEnum.Q_9;
            case NPCType.NPC_10:
                return QuestIdEnum.Q_10;
            case NPCType.NPC_11:
                return QuestIdEnum.Q_11;
            case NPCType.NPC_12:
                return QuestIdEnum.Q_12;
        }
        throw new Exception("NPC " + type.ToString() + "에 대한 퀘스트 데이터가 없습니다!");
    }

    public static ImageIdEnum getTalkerBackground(NPCType type)
    {
        switch(type)
        {
            case NPCType.SHOP_INDUSTRY:
                return ImageIdEnum.UI_TALK_GC;
            case NPCType.SHOP_BREAD:
                return ImageIdEnum.UI_TALK_MJ;
            case NPCType.SHOP_FISH:
                return ImageIdEnum.UI_TALK_SS;
            case NPCType.SHOP_FRUIT:
                return ImageIdEnum.UI_TALK_YJ;
            case NPCType.SHOP_MEAT:
                return ImageIdEnum.UI_TALK_EJ;
            case NPCType.SHOP_SELLER:
                return ImageIdEnum.UI_TALK_JY;
            default:
                return (ImageIdEnum)UnityEngine.Random.Range((int)ImageIdEnum.UI_TALK_GC, (int)ImageIdEnum.UI_TALK_JY + 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private QuestListBehavior questUI;

    private static QuestManager instance = null;
    public static QuestManager getInstance()
    {
        if (instance == null)
        {
            throw new System.Exception(nameof(QuestManager) + "가 게임 내에 사용되지 않았습니다!");
        }
        return instance;
    }

    private List<QuestIdEnum> doneList = new List<QuestIdEnum>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception(nameof(QuestManager) + "는 하나만 존재해야합니다!");
        }
        instance = this;

        if (questUI == null)
        {
            throw new System.Exception("퀘스트 UI가 등록되지 않았습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 현재 특정 퀘스트가 이미 수행되었는지 여부를 반환합니다.
    /// </summary>
    /// <param name="questid"></param>
    /// <returns></returns>
    public bool isDone(QuestIdEnum questid)
    {
        foreach (QuestIdEnum done in doneList)
        {
            if (done == questid)
            {
                return true;
            }
        }
        return false;
    }

    public void doneQuest(QuestIdEnum questId)
    {
        doneList.Add(questId);
    }
}

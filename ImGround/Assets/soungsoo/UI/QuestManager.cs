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

    public void doneQuest(QuestIdEnum questId)
    {
        Debug.LogError("퀘스트 완료 로직 미구현");
        throw new System.NotImplementedException("퀘스트 로직 미구현");
    }
}

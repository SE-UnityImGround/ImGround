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
            throw new System.Exception(nameof(QuestManager) + "�� ���� ���� ������ �ʾҽ��ϴ�!");
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception(nameof(QuestManager) + "�� �ϳ��� �����ؾ��մϴ�!");
        }
        instance = this;

        if (questUI == null)
        {
            throw new System.Exception("����Ʈ UI�� ��ϵ��� �ʾҽ��ϴ�!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doneQuest(QuestIdEnum questId)
    {
        Debug.LogError("����Ʈ �Ϸ� ���� �̱���");
        throw new System.NotImplementedException("����Ʈ ���� �̱���");
    }
}

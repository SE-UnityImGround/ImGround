using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private QuestListBehavior questUI;

    // Start is called before the first frame update
    void Start()
    {
        if (questUI == null)
        {
            throw new System.Exception("퀘스트 UI가 등록되지 않았습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

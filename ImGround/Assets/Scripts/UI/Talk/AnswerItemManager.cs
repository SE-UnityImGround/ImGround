using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대화 중 대답 텍스트 UI 오브젝트를 관리하는 자원 관리 클래스입니다.
/// </summary>
public class AnswerItemManager
{
    private GameObject AnswerItemPrefab;
    private Queue<AnswerBehavior> storage;
    private AnswerBehavior.AnswerSelectedEvent handler;

    public AnswerItemManager(GameObject answerPrefab, AnswerBehavior.AnswerSelectedEvent handler)
    {
        this.AnswerItemPrefab = answerPrefab;
        this.storage = new Queue<AnswerBehavior>();
        this.handler = handler;
    }

    private AnswerBehavior makeAnswerItem()
    {
        AnswerBehavior newItem = Object.Instantiate(AnswerItemPrefab, null).GetComponent<AnswerBehavior>();
        newItem.initialize();
        newItem.AnswerSelectedEventHandler += handler;
        return newItem;
    }

    public void recycleAnswerItem(AnswerBehavior answer)
    {
        storage.Enqueue(answer);
    }

    public AnswerBehavior getAnswerItem()
    {
        if (storage.Count == 0)
            return makeAnswerItem();
        else
            return storage.Dequeue();
    }
}

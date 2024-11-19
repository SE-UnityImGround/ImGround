using System.Collections.Generic;
using UnityEngine;

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

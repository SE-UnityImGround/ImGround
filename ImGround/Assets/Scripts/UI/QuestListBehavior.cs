using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListBehavior : MonoBehaviour
{
    public GameObject questsListView;
    public GameObject questPreFab;

    // Start is called before the first frame update
    void Start()
    {
        testQuest();

        setActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void testQuest()
    {
        addQuest(new Quest(ImageIdEnum.ICON_ATTACK, "Lorem ipsum dolor sit amet consetetur", ImageIdEnum.ICON_GEM, 2000, 0));
        addQuest(new Quest(ImageIdEnum.ICON_ARCHER, "Lorem ipsum dolor sit amet consetetur", ImageIdEnum.ICON_MEAT, 30, 9, 5));
        addQuest(new Quest(ImageIdEnum.ICON_ATTACK, "test1", ImageIdEnum.ICON_COIN, 512, 10));
        addQuest(new Quest(ImageIdEnum.ICON_GEM, "test2", ImageIdEnum.ICON_COIN, 128, 2));
    }

    /// <summary>
    /// 퀘스트를 추가합니다.
    /// </summary>
    /// <param name="q"></param>
    public void addQuest(Quest q)
    {
        QuestBehavior newQuest = Instantiate(questPreFab, questsListView.transform).GetComponent<QuestBehavior>();
        newQuest.initialize(q);
    }

    /// <summary>
    /// 인벤토리의 표시/숨김을 관리합니다.
    /// </summary>
    /// <param name="isActive"></param>
    public void setActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 현재 인벤토리가 표시되는지를 반환합니다.
    /// </summary>
    /// <returns>인벤토리가 표시중이면 true, 그렇지 않으면 false를 반환합니다.</returns>
    public bool getActive()
    {
        return gameObject.activeSelf;
    }
}

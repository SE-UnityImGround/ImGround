using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private InventoryBehavior inventoryUI;
    [SerializeField]
    private QuestListBehavior questUI;

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryUI == null)
        {
            throw new System.Exception("인벤토리 UI가 등록되지 않았습니다!");
        }
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
    /// 현재 인벤토리에서 선택된 아이템을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public Item getSelectedItem()
    {
        return inventoryUI.getSelectedItem();
    }

    /// <summary>
    /// 아이템을 인벤토리 빈 공간에 추가하려고 시도하며, 한 개 이상의 아이템이 추가되면 true를 반환합니다.
    /// <br/>주의 : 아이템을 추가한 후 남은 수량이 입력된 item 객체에 남아있습니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        return inventoryUI.addItem(item);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBehavior : MonoBehaviour
{
    public GameObject slotPrefab = null;
    public GameObject SlotList = null;

    private Inventory inventory = new Inventory(20);

    // Start is called before the first frame update
    void Start()
    {
        if (slotPrefab == null)
        {
            Debug.LogErrorFormat("{0}에 슬롯 프리팹 {1}가 입력되지 않았습니다!", nameof(InventoryBehavior), nameof(slotPrefab));
            return;
        }

        for (int i = 1; i <= inventory.size; i++)
        {
            SlotBehavior newSlotScript = Instantiate(slotPrefab, SlotList.transform).GetComponent<SlotBehavior>();
            newSlotScript.initialize(this, inventory.slots[i - 1], i);
        }

        setActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    /// <summary>
    /// 슬롯 클릭 발생을 테스트하는 로직입니다.
    /// </summary>
    /// <param name="idx"></param>
    public void testSlotClicked(int slotIdx)
    {
        Debug.Log(slotIdx);
        Debug.Log(inventory.slots[slotIdx-1].item.name);
    }

    /// <summary>
    /// 아이템 입력을 테스트하는 로직입니다.
    /// </summary>
    public void testAddItem()
    {
        Debug.Log(inventory.addItem(new Item(ItemIdEnum.TEST_NULL_ITEM, 2)));
    }
}

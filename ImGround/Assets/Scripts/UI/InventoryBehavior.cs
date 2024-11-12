using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 내에서 인벤토리 UI의 컨트롤을 담당하는 컨트롤 클래스입니다.
/// </summary>
public class InventoryBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab = null;

    [SerializeField]
    private GameObject SlotList = null;

    private Inventory inventory = new Inventory(20);

    private ItemBundle selectedItem = null;

    // Start is called before the first frame update
    void Start()
    {
        if (slotPrefab == null)
        {
            Debug.LogErrorFormat("{0}에 슬롯 프리팹 {1}가 입력되지 않았습니다!", nameof(InventoryBehavior), nameof(slotPrefab));
            return;
        }

        generateSlots();
        setActive(false);
    }

    private void generateSlots()
    {
        for (int slotNum = 1; slotNum <= inventory.size; slotNum++)
        {
            SlotBehavior newSlotScript = Instantiate(slotPrefab, SlotList.transform).GetComponent<SlotBehavior>();
            newSlotScript.initialize(inventory.slots[slotNum - 1]);
            newSlotScript.itemSelectedEventHandler += onItemSelected;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*========================================================
     *                     이벤트 처리
     *========================================================*/

    private void onItemSelected(ItemBundle selection)
    {
        this.selectedItem = selection;
    }

    /*========================================================
     *                     외부 지원 메소드
     *========================================================*/

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
    /// 현재 선택된 아이템을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public ItemBundle getSelectedItem()
    {
        return selectedItem;
    }

    /// <summary>
    /// 아이템을 인벤토리 빈 공간에 추가하려고 시도하며, 한 개 이상의 아이템이 추가되면 true를 반환합니다.
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 item 객체에 반영됩니다.
    /// </summary>
    /// <param name="item">인벤토리에 추가할 아이템</param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        return inventory.addItem(new ItemBundle(item, 1, true));
    }

    /// <summary>
    /// 아이템을 인벤토리 빈 공간에 추가하려고 시도하며, 한 개 이상의 아이템이 추가되면 true를 반환합니다.
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 items의 각 아이템에 반영됩니다.
    /// </summary>
    /// <param name="items">인벤토리에 추가할 아이템들</param>
    /// <returns></returns>
    public bool addItems(Item[] items)
    {
        bool result = false;
        foreach (Item item in items)
        {
            result = result || inventory.addItem(new ItemBundle(item, 1, true));
        }
        return result;
    }
}

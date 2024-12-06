using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 내에서 인벤토리 UI의 컨트롤을 담당하는 컨트롤 클래스입니다.
/// </summary>
public class InventoryBehavior : UIBehavior
{
    [SerializeField]
    private GameObject slotPrefab = null;
    [SerializeField]
    private GameObject SlotList = null;

    private SlotBehavior[] slots;

    public override void initialize()
    {
        if (slotPrefab == null)
        {
            Debug.LogErrorFormat("{0}에 슬롯 프리팹 {1}가 입력되지 않았습니다!", nameof(InventoryBehavior), nameof(slotPrefab));
            return;
        }

        generateSlots();
        InventoryManager.onSlotItemChangedHandler += onSlotChanged;
        InventoryManager.initialize();
    }

    private void generateSlots()
    {
        slots = new SlotBehavior[InventoryManager.getInventorySize()];
        for (int slotNum = 0; slotNum < slots.Length; slotNum++)
        {
            SlotBehavior newSlotScript = Instantiate(slotPrefab, SlotList.transform).GetComponent<SlotBehavior>();
            newSlotScript.initialize(slotNum);
            newSlotScript.slotSelectedEventHandler += InventoryManager.selectSlot;
            slots[slotNum] = newSlotScript;
        }
    }

    private void onSlotChanged(int slotIdx)
    {
        slots[slotIdx].updateItemInfo(
            InventoryManager.getItemId(slotIdx),
            InventoryManager.getItemAmount(slotIdx));
    }
}

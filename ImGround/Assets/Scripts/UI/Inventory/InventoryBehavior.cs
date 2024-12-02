using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������ �κ��丮 UI�� ��Ʈ���� ����ϴ� ��Ʈ�� Ŭ�����Դϴ�.
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
            Debug.LogErrorFormat("{0}�� ���� ������ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(InventoryBehavior), nameof(slotPrefab));
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

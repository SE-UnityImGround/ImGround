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

    private Inventory inventory = new Inventory(24);

    private ItemBundle selectedItem = null;

    public override void initialize()
    {
        if (slotPrefab == null)
        {
            Debug.LogErrorFormat("{0}�� ���� ������ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(InventoryBehavior), nameof(slotPrefab));
            return;
        }

        generateSlots();
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

    /*========================================================
     *                     �̺�Ʈ ó��
     *========================================================*/

    private void onItemSelected(ItemBundle selection)
    {
        this.selectedItem = selection;
    }

    /*========================================================
     *                     �ܺ� ���� �޼ҵ�
     *========================================================*/

    /// <summary>
    /// ���� ���õ� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public ItemBundle getSelectedItem()
    {
        return selectedItem;
    }

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, �� �� �̻��� �������� �߰��Ǹ� true�� ��ȯ�մϴ�.
    /// <br/>�������� �߰��� �� ���� ������ �Էµ� item ��ü�� �ݿ��˴ϴ�.
    /// </summary>
    /// <param name="item">�κ��丮�� �߰��� ������</param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        return inventory.addItem(new ItemBundle(item, 1, true));
    }

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, �� �� �̻��� �������� �߰��Ǹ� true�� ��ȯ�մϴ�.
    /// <br/>�������� �߰��� �� ���� ������ �Էµ� items�� �� �����ۿ� �ݿ��˴ϴ�.
    /// </summary>
    /// <param name="items">�κ��丮�� �߰��� �����۵�</param>
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

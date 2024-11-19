using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    private const int INVENTORY_SIZE = 24;
    private static ItemBundle[] inventory = new ItemBundle[INVENTORY_SIZE];
    private static int selectedSlotIdx = -1;
    private static int money;

    /// <summary>
    /// �κ��丮�� ���� ���ð��� ����� ��� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    /// <param name="selectedIdx"></param>
    public delegate void onSelectionChanged(int selectedIdx);
    public static onSelectionChanged onSelectionChangedHandler;

    /// <summary>
    /// �κ��丮�� Ư�� ���Կ� ������ ������ �߻��� ��� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    /// <param name="selectedIdx"></param>
    public delegate void onSlotItemChanged(int slotIdx);
    public static onSlotItemChanged onSlotItemChangedHandler;

    /*====================================
     *      Selection Management
     *===================================*/

    /// <summary>
    /// �κ��丮�� Ư�� ������ �����մϴ�.
    /// </summary>
    /// <param name="idx"></param>
    public static void selectSlot(int idx)
    {
        selectedSlotIdx = idx;
        if (selectedSlotIdx < -1 || selectedSlotIdx > INVENTORY_SIZE)
            selectedSlotIdx = -1;
        onSelectionChangedHandler?.Invoke(selectedSlotIdx);
    }

    /// <summary>
    /// ���� ���õ� ���� ��ȣ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public static int getSelectedSlotIndex()
    {
        return selectedSlotIdx;
    }

    /*====================================
     *       Inventory Information
     *===================================*/

    /// <summary>
    /// �κ��丮�� �����ϴ� Ư�� �������� ������ ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public static int getItemAmount(ItemIdEnum item)
    {
        int count = 0;
        foreach (ItemBundle bundle in inventory)
        {
            if (bundle != null && bundle.item.itemId == item && bundle.count > 0)
            {
                count += bundle.count;
            }
        }
        return count;
    }

    /// <summary>
    /// �κ��丮 �� �����ϴ� �����۰� ���� ������ ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<ItemIdEnum, int> getInventoryInfo()
    {
        Dictionary<ItemIdEnum, int> info = new Dictionary<ItemIdEnum, int>();
        foreach (ItemBundle item in inventory)
        {
            if (item != null && item.count > 0)
            {
                if (!info.ContainsKey(item.item.itemId))
                {
                    info.Add(item.item.itemId, 0);
                }
                info[item.item.itemId] += item.count;
            }
        }
        return info;
    }

    /// <summary>
    /// �κ��丮�� ũ�⸦ ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public static int getInventorySize()
    {
        return INVENTORY_SIZE;
    }

    /*====================================
     *         Money Getter/Setter
     *===================================*/

    /// <summary>
    /// �־��� �׼���ŭ ���� ���� �߰�/�����մϴ�.
    /// </summary>
    /// <param name="money"></param>
    public static void changeMoney(int money)
    {
        InventoryManager.money += money;
    }

    /// <summary>
    /// ���� ���� ���� �׼��� ��ȯ�մϴ�.
    /// </summary>
    public static int getMoney()
    {
        return money;
    }

    /*====================================
     *         Item Getter/Setter
     *===================================*/

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, �����ϸ� true�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="item">�κ��丮�� �߰��� ������</param>
    /// <returns></returns>
    public static bool addItem(Item item)
    {
        return addItems(new ItemBundle(item, 1, true));
    }

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, �� �� �̻��� ������ �߰��� �����ϸ� true�� ��ȯ�մϴ�.
    /// <br/>�������� �߰��� �� ���� ������ �Էµ� <paramref name="items"/> ��ü�� �ݿ��˴ϴ�.
    /// </summary>
    /// <param name="items">�κ��丮�� �߰��� ������</param>
    /// <returns></returns>
    public static bool addItems(ItemBundle items)
    {
        bool added = false;
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            if (inventory[i] == null || inventory[i].item.itemId == ItemIdEnum.TEST_NULL_ITEM)
            {
                inventory[i] = new ItemBundle(items.item, 0, true);
            }
            
            if (inventory[i].addItem(items))
            {
                onSlotItemChangedHandler?.Invoke(i);
                added = true;
            }

            if (items.count == 0)
                break;
        }

        return added;
    }

    /// <summary>
    /// Ư�� ������ ������ ID�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="slotIdx"></param>
    /// <returns></returns>
    public static ItemIdEnum getItemId(int slotIdx)
    {
        if ((slotIdx < 0 && slotIdx > INVENTORY_SIZE)
            || inventory[slotIdx] == null)
            return ItemIdEnum.TEST_NULL_ITEM;
        else
            return inventory[slotIdx].item.itemId;
    }

    /// <summary>
    /// �־��� id�� �������� �־��� ������ŭ ������ �õ��մϴ�. ������ ��ŭ�� �������� ��ȯ�˴ϴ�.
    /// <br/> ������ �Է��ϸ� ��� ������ �����ϴ�.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static ItemBundle removeItem(ItemIdEnum id, int amount)
    {
        if (amount < 0) amount = -1; // �����÷ο� ��ȭ
        ItemBundle result = new ItemBundle(id, 0, false);
        for (int i = 0; i < inventory.Length; i++)
        {
            if (amount == 0)
                break;
            if (inventory[i] != null && inventory[i].item.itemId == id && inventory[i].count > 0)
            {
                ItemBundle getItem = inventory[i].getDividedItems(amount);
                if (inventory[i].count == 0)
                    inventory[i] = null;
                onSlotItemChangedHandler?.Invoke(i);
                amount -= getItem.count;
                result.addItem(getItem);
            }
        }
        return result;
    }

    /// <summary>
    /// ��� ������ �������� �����ϴ�.
    /// </summary>
    /// <returns>���� ��� �κ��丮�� ������</returns>
    public static ItemBundle[] popAllItems()
    {
        List<ItemBundle> items = new List<ItemBundle>(INVENTORY_SIZE);
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            ItemBundle getItem = takeItem(i, -1);
            if (getItem != null && getItem.count > 0)
                items.Add(getItem);
        }
        return items.ToArray();
    }

    /// <summary>
    /// ���Կ��� �������� �־��� ������ŭ �����ϴ�.
    /// <br/> ������ �����̸� ��� �����ϴ�.
    /// <br/> �������� ���ٸ� null�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static ItemBundle takeItem(int slotIdx, int count)
    {
        if (inventory[slotIdx] == null)
            return null;

        ItemBundle result;

        if (count < 0 || count >= inventory[slotIdx].count)
        {
            result = inventory[slotIdx];
            inventory[slotIdx] = null;
        }
        else
        {
            result = inventory[slotIdx].getDividedItems(count);
            if (inventory[slotIdx].count == 0)
                inventory[slotIdx] = null;
        }

        onSlotItemChangedHandler?.Invoke(slotIdx);
        return result;
    }
}

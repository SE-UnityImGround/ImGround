using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    private const int INVENTORY_SIZE = 24;
    private static ItemBundle[] inventory = new ItemBundle[INVENTORY_SIZE];
    private static int selectedSlotIdx = -1;
    private static int money = 300000;

    /*====================================
     *         Inventory Events
     *===================================*/

    private static void dealWithEventError(Exception e)
    {
        Debug.LogError("�κ��丮 ���� ó�� �� ���� : ���� �α� ����\n");
        Debug.LogException(e);
    }

    /// <summary>
    /// �κ��丮�� ���� ���ð��� ����� ��� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    public delegate void onSelectionChanged(int selectedIdx);
    public static onSelectionChanged onSelectionChangedHandler;
    private static void invokeOnSelectionChanged(int selectedIdx)
    {
        try
        {
            onSelectionChangedHandler?.Invoke(selectedIdx);
        }
        catch (Exception e)
        {
            dealWithEventError(e);
        }
    }

    /// <summary>
    /// �κ��丮�� Ư�� ���Կ� ������ ������ �߻��� ��� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    public delegate void onSlotItemChanged(int slotIdx);
    public static onSlotItemChanged onSlotItemChangedHandler;
    private static void invokeOnSlotItemChanged(int slotIdx)
    {
        try
        {
            onSlotItemChangedHandler?.Invoke(slotIdx);
        }
        catch (Exception e)
        {
            dealWithEventError(e);
        }
        if (getItemAmount(slotIdx) == 0 && selectedSlotIdx == slotIdx)
            selectSlot(-1);
    }

    /// <summary>
    /// ���� ���� ���� ��ȭ�� �� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    public static Action<int> onMoneyChangedHandler;
    private static void invokeOnMoneyChanged(int changedMoney)
    {
        try
        {
            onMoneyChangedHandler?.Invoke(changedMoney);
        }
        catch (Exception e)
        {
            dealWithEventError(e);
        }
    }

    /*====================================
     *           Save And Load
     *===================================*/

    private const string SAVE_NAME_MONEY = "Inv_money";
    private const string SAVE_NAME_INVENTORY = "Inv_inventory";

    private class InventoryData
    {
        public List<ItemBundle> inv;

        public InventoryData()
        {
            inv = new List<ItemBundle>();
            foreach(ItemBundle item in inventory)
            {
                if (item != null
                    && item.item.itemId != ItemIdEnum.TEST_NULL_ITEM
                    && item.count != 0)
                    inv.Add(new ItemBundle(item));
            }
        }
    }

    public static void initialize()
    {
        SaveManager.setOnSave(onStartSave);

        if (!SaveManager.isLoadedGame)
            return;

        money = PlayerPrefs.GetInt(SAVE_NAME_MONEY, 300000);
        string inventoryData = PlayerPrefs.GetString(SAVE_NAME_INVENTORY, null);
        if (inventoryData != null)
        {
            ItemBundle[] inv = JsonUtility.FromJson<InventoryData>(inventoryData)?.inv?.ToArray();
            if (inv != null)
                for (int i = 0; i < INVENTORY_SIZE && i < inv.Length; i++)
                {
                    inventory[i] = inv[i];
                    onSlotItemChangedHandler?.Invoke(i);
                }
        }
    }

    private static void onStartSave()
    {
        PlayerPrefs.SetInt(SAVE_NAME_MONEY, money);
        PlayerPrefs.SetString(
            SAVE_NAME_INVENTORY, 
            JsonUtility.ToJson(new InventoryData()));
    }

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
        invokeOnSelectionChanged(selectedSlotIdx);
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
        invokeOnMoneyChanged(InventoryManager.money);
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
        ItemBundle addedItem = new ItemBundle(item, 1, true);
        addItems(addedItem);
        return (addedItem.count == 0);
    }

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, ���� ������ ���
    /// <br/>�������� �߰��� �� ���� ������ �Էµ� <paramref name="items"/> ��ü�� �ݿ��˴ϴ�.
    /// </summary>
    /// <param name="items">�κ��丮�� �߰��� ������</param>
    public static void addItems(ItemBundle items)
    {
        // �ý��� ������ ó�� ����
        if (items.item.itemId == ItemIdEnum.TEST_NULL_ITEM)
        {
            items.discardItem(-1);
            return;
        }
        if (items.item.itemId == ItemIdEnum.PACKAGE)
        {
            if (!(items.item is ItemPackage))
            {
                items.discardItem(-1);
                return;
            }

            ItemPackage result = addPackage((ItemPackage)items.item);
            if (result == null)
            {
                items.discardItem(-1);
            }
            else
            {
                items.setItemBundle(result, 1, false);
            }
            return;
        }
        if (items.item.itemId == ItemIdEnum.MONEY)
        {
            changeMoney(items.count);
            items.discardItem(-1);
            return;
        }

        // �Ϲ� ������ ó��
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            // 1. ������ ������ �������� �ִٸ� �ش� ���Կ� ���� ä���
            if (items.count == 0)
                break;

            if (inventory[i] != null
                && inventory[i].item.itemId == items.item.itemId
                && inventory[i].addItem(items))
            {
                invokeOnSlotItemChanged(i);
            }
        }
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            // 2. ���� ������ ó��
            if (items.count == 0)
                break;

            if (inventory[i] == null
                || inventory[i].item.itemId == ItemIdEnum.TEST_NULL_ITEM
                || inventory[i].count == 0)
            {
                inventory[i] = new ItemBundle(items.item, 0, true);
            }

            if (inventory[i].addItem(items))
            {
                invokeOnSlotItemChanged(i);
            }
        }
    }

    /// <summary>
    /// ������ ��Ű���� Ǯ� �κ��丮�� ����ϴ�.
    /// <br/> ��� �������� ������ �õ��ϸ�, ������ �������� �ٽ� ��Ű���� ��ȯ�˴ϴ�.
    /// <br/> ���� ���� �������� ���ٸ� null�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="pack"></param>
    /// <returns></returns>
    private static ItemPackage addPackage(ItemPackage pack)
    {
        List<ItemBundle> remains = new List<ItemBundle>();
        foreach (ItemBundle bundle in pack.items)
        {
            addItems(bundle);
            if (bundle.count > 0)
            {
                remains.Add(new ItemBundle(bundle));
            }
        }

        if (remains.Count == 0)
            return null;
        else 
            return new ItemPackage(remains.ToArray());
    }

    /// <summary>
    /// Ư�� ������ ������ ID�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="slotIdx"></param>
    /// <returns></returns>
    public static ItemIdEnum getItemId(int slotIdx)
    {
        if (slotIdx < 0 || slotIdx > INVENTORY_SIZE || inventory[slotIdx] == null)
            return ItemIdEnum.TEST_NULL_ITEM;
        else
            return inventory[slotIdx].item.itemId;
    }

    /// <summary>
    /// Ư�� ������ ������ ������ ��ȯ�մϴ�.
    /// </summary>
    /// <param name="slotIdx"></param>
    /// <returns></returns>
    public static int getItemAmount(int slotIdx)
    {
        if ((slotIdx < 0 && slotIdx > INVENTORY_SIZE)
            || inventory[slotIdx] == null)
            return 0;
        else
            return inventory[slotIdx].count;
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
                invokeOnSlotItemChanged(i);
                amount -= getItem.count;
                result.addItem(getItem);
            }
        }
        return result;
    }

    /// <summary>
    /// ��� ������ �������� ��Ű�� ���·� �����ϴ�.
    /// </summary>
    /// <returns></returns>
    public static ItemPackage popAllItemsByPackage()
    {
        return new ItemPackage(popAllItems());
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
        if (inventory[slotIdx] == null || inventory[slotIdx].item.itemId == ItemIdEnum.TEST_NULL_ITEM)
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

        invokeOnSlotItemChanged(slotIdx);
        return result;
    }
}

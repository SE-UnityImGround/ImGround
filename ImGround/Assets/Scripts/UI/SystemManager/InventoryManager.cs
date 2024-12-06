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
        Debug.LogError("인벤토리 로직 처리 중 에러 : 다음 로그 참고\n");
        Debug.LogException(e);
    }

    /// <summary>
    /// 인벤토리의 슬롯 선택값이 변경될 경우 발생하는 이벤트입니다.
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
    /// 인벤토리의 특정 슬롯에 아이템 갱신이 발생할 경우 발생하는 이벤트입니다.
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
    /// 현재 가진 돈이 변화할 때 발생하는 이벤트입니다.
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
    /// 인벤토리의 특정 슬롯을 선택합니다.
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
    /// 현재 선택된 슬롯 번호를 반환합니다.
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
    /// 인벤토리에 존재하는 특정 아이템의 갯수를 반환합니다.
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
    /// 인벤토리 내 존재하는 아이템과 수량 정보를 반환합니다.
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
    /// 인벤토리의 크기를 반환합니다.
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
    /// 주어진 액수만큼 가진 돈을 추가/제거합니다.
    /// </summary>
    /// <param name="money"></param>
    public static void changeMoney(int money)
    {
        InventoryManager.money += money;
        invokeOnMoneyChanged(InventoryManager.money);
    }

    /// <summary>
    /// 현재 가진 돈의 액수를 반환합니다.
    /// </summary>
    public static int getMoney()
    {
        return money;
    }

    /*====================================
     *         Item Getter/Setter
     *===================================*/

    /// <summary>
    /// 아이템을 인벤토리 빈 공간에 추가하려고 시도하며, 성공하면 true를 반환합니다.
    /// </summary>
    /// <param name="item">인벤토리에 추가할 아이템</param>
    /// <returns></returns>
    public static bool addItem(Item item)
    {
        ItemBundle addedItem = new ItemBundle(item, 1, true);
        addItems(addedItem);
        return (addedItem.count == 0);
    }

    /// <summary>
    /// 아이템을 인벤토리 빈 공간에 추가하려고 시도하며, 만약 실패할 경우
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 <paramref name="items"/> 객체에 반영됩니다.
    /// </summary>
    /// <param name="items">인벤토리에 추가할 아이템</param>
    public static void addItems(ItemBundle items)
    {
        // 시스템 아이템 처리 구분
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

        // 일반 아이템 처리
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            // 1. 기존에 동일한 아이템이 있다면 해당 슬롯에 먼저 채우기
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
            // 2. 남은 아이템 처리
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
    /// 아이템 패키지를 풀어서 인벤토리에 담습니다.
    /// <br/> 모든 아이템을 담으려 시도하며, 실패한 아이템이 다시 패키지로 반환됩니다.
    /// <br/> 만약 남은 아이템이 없다면 null을 반환합니다.
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
    /// 특정 슬롯의 아이템 ID를 반환합니다.
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
    /// 특정 슬롯의 아이템 수량을 반환합니다.
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
    /// 주어진 id의 아이템을 주어진 갯수만큼 꺼내려 시도합니다. 성공한 만큼의 아이템이 반환됩니다.
    /// <br/> 음수를 입력하면 모든 갯수를 꺼냅니다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static ItemBundle removeItem(ItemIdEnum id, int amount)
    {
        if (amount < 0) amount = -1; // 오버플로우 완화
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
    /// 모든 슬롯의 아이템을 패키지 형태로 꺼냅니다.
    /// </summary>
    /// <returns></returns>
    public static ItemPackage popAllItemsByPackage()
    {
        return new ItemPackage(popAllItems());
    }

    /// <summary>
    /// 모든 슬롯의 아이템을 꺼냅니다.
    /// </summary>
    /// <returns>꺼낸 모든 인벤토리의 아이템</returns>
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
    /// 슬롯에서 아이템을 주어진 갯수만큼 꺼냅니다.
    /// <br/> 갯수가 음수이면 모두 꺼냅니다.
    /// <br/> 아이템이 없다면 null을 반환합니다.
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

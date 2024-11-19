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
    /// 인벤토리의 슬롯 선택값이 변경될 경우 발생하는 이벤트입니다.
    /// </summary>
    /// <param name="selectedIdx"></param>
    public delegate void onSelectionChanged(int selectedIdx);
    public static onSelectionChanged onSelectionChangedHandler;

    /// <summary>
    /// 인벤토리의 특정 슬롯에 아이템 갱신이 발생할 경우 발생하는 이벤트입니다.
    /// </summary>
    /// <param name="selectedIdx"></param>
    public delegate void onSlotItemChanged(int slotIdx);
    public static onSlotItemChanged onSlotItemChangedHandler;

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
        onSelectionChangedHandler?.Invoke(selectedSlotIdx);
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
        return addItems(new ItemBundle(item, 1, true));
    }

    /// <summary>
    /// 아이템을 인벤토리 빈 공간에 추가하려고 시도하며, 한 개 이상의 아이템 추가에 성공하면 true를 반환합니다.
    /// <br/>아이템을 추가한 후 남은 수량이 입력된 <paramref name="items"/> 객체에 반영됩니다.
    /// </summary>
    /// <param name="items">인벤토리에 추가할 아이템</param>
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
    /// 특정 슬롯의 아이템 ID를 반환합니다.
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
                onSlotItemChangedHandler?.Invoke(i);
                amount -= getItem.count;
                result.addItem(getItem);
            }
        }
        return result;
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

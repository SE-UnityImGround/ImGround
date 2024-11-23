using UnityEngine;

public class ItemPrefabID : MonoBehaviour
{
    public ItemIdEnum itemType = ItemIdEnum.TEST_NULL_ITEM;
    public int amount = 1;
    private ItemBundle itemData = null;

    public void setItemData(ItemBundle item)
    {
        itemData = item;
    }

    public ItemBundle getItem()
    {
        if (itemData == null)
            return new ItemBundle(itemType, amount, false);
        else
            return itemData;
    }
}

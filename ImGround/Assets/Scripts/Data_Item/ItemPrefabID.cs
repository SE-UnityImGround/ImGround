using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefabID : MonoBehaviour
{
    [SerializeField]
    [Tooltip("(만약 2D 이미지를 자동 관리하고자 한다면 추가하세요)")]
    private SpriteRenderer Item_2d_image = null;
    public ItemIdEnum itemType = ItemIdEnum.TEST_NULL_ITEM;
    public int amount = 1;
    private ItemBundle itemData = null;

    public void setItemData(ItemBundle item)
    {
        itemData = item;
        itemType = item.item.itemId;
        amount = item.count;
        if (Item_2d_image!= null)
        {
            Item_2d_image.sprite = ItemInfoManager.getItemImage(item.item.itemId);
        }
    }

    public ItemBundle getItem()
    {
        if (itemData == null)
            return new ItemBundle(itemType, amount, false);
        else
            return itemData;
    }
}

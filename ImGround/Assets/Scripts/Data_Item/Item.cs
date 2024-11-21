using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 단일 아이템을 표현하는 기본 자료구조입니다.
/// </summary>
public class Item
{
    public ItemIdEnum itemId { get; private set; }
    public string name { get { return ItemInfoManager.getItemInfo(itemId).itemName; } }
    public Sprite image { get { return ItemInfoManager.getItemImage(itemId); } }
    
    /// <summary>
    /// 아이템을 생성합니다.
    /// </summary>
    /// <param name="itemId"></param>
    public Item(ItemIdEnum itemId)
    {
        this.itemId = itemId;
    }
}

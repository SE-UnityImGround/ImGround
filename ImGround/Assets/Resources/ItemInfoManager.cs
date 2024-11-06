using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemInfoManager
{
    private const int DEFAULT_COUNT = 4;

    private static ItemInfo[] informations =
    {
        new ItemInfo(ItemIdEnum.TEST_NULL_ITEM, ImageIdEnum.ITEM_NULL, "TestNullItem", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.MILK_PACK, ImageIdEnum.ITEM_MILK_PACK, "우유 팩", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.MILK_BUCKET, ImageIdEnum.ITEM_MILK_BUCKET, "우유 양동이", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.HORSE_LEATHER, ImageIdEnum.ITEM_HORSE_LEATHER, "말 가죽", DEFAULT_COUNT),

        new ItemInfo(ItemIdEnum.GOLD_ORE, ImageIdEnum.ITEM_GOLD_ORE, "금 원석", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.GOLD_INGOT, ImageIdEnum.ITEM_GOLD_INGOT, "금 주괴", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.GOLD_NECKLACE, ImageIdEnum.ITEM_GOLD_NECKLACE, "금 목걸이", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.SILVER_ORE, ImageIdEnum.ITEM_SILVER_ORE, "은 원석", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.SILVER_INGOT, ImageIdEnum.ITEM_SILVER_INGOT, "은 주괴", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.SILVER_NECKLACE, ImageIdEnum.ITEM_SILVER_NECKLACE, "은 목걸이", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.IRON_ORE, ImageIdEnum.ITEM_IRON_ORE, "철 원석", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.IRON_INGOT, ImageIdEnum.ITEM_IRON_INGOT, "철 주괴", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.IRON_NECKLACE, ImageIdEnum.ITEM_IRON_NECKLACE, "철 목걸이", DEFAULT_COUNT),

        new ItemInfo(ItemIdEnum.CARROT_SEED, ImageIdEnum.ITEM_CARROT_SEED, "당근 씨앗", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.LEMMON_SEED, ImageIdEnum.ITEM_LEMMON_SEED, "레몬 씨앗", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.RICE_SEED, ImageIdEnum.ITEM_RICE_SEED, "볍씨", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.TOMATO_SEED, ImageIdEnum.ITEM_TOMATO_SEED, "토마토 씨앗", DEFAULT_COUNT),
        new ItemInfo(ItemIdEnum.WATERMELON_SEED, ImageIdEnum.ITEM_WATERMELON_SEED, "수박 씨앗", DEFAULT_COUNT)
    };

    private static ItemInfo findItemInfo(ItemIdEnum itemId)
    {
        // 현재 O(N) 알고리즘 -> 리팩터링 요소
        for (int i = 0; i < informations.Length; i++)
            if (informations[i].itemId == itemId)
                return informations[i];
        throw new Exception(itemId.ToString() + "에 대한 아이템 정보가 등록되지 않았습니다!");
    }

    /// <summary>
    /// 아이템의 이미지를 반환합니다.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public static Sprite getItemImage(ItemIdEnum itemId)
    {
        return ImageManager.getImage(findItemInfo(itemId).itemImg);
    }

    /// <summary>
    /// 아이템의 id별 고유 이름을 반환합니다.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public static string getItemName(ItemIdEnum itemId)
    {
        return findItemInfo(itemId).itemName;
    }

    /// <summary>
    /// 아이템의 id별 고유 중첩가능 수를 반환합니다.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public static int getMaxCount(ItemIdEnum itemId)
    {
        return findItemInfo(itemId).maxCount;
    }
}

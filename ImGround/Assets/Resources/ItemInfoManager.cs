using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemInfoManager
{
    /*==================================================
     *                 싱글톤 관리자
     *==================================================*/

    /// <summary>
    /// 절대 이 멤버로 접근하지 말 것. <see cref="getInstance"/>를 사용하세요.
    /// </summary>
    private static ItemInfoManager instance = null;

    private ItemInfoManager()
    {

    }

    private static ItemInfoManager getInstance()
    {
        if (instance == null)
        {
            instance = new ItemInfoManager();
            checkItemInfo();
        }
        return instance;
    }

    /// <summary>
    /// 모든 아이템 정보가 포함되어 있는지를 검사합니다.
    /// </summary>
    private static void checkItemInfo()
    {
        string omittedImages = "";
        foreach (ItemIdEnum id in Enum.GetValues(typeof(ItemIdEnum)))
        {
            try
            {
                findItemInfo(id);
            }
            catch
            {
                omittedImages += id.ToString() + "\n";
            }
        }
        if (omittedImages.Length > 0)
            Debug.LogError("다음의 아이템 정보가 아직 등록되지 않았습니다! :\n" + omittedImages);
    }

    /*==================================================
     *                 Item 데이터 
     *==================================================*/

    private const int DEFAULT_COUNT = 4;

    private ItemInfo[] informations =
    {
        new ItemInfo(ItemIdEnum.TEST_NULL_ITEM, ImageIdEnum.ITEM_NULL, "TestNullItem", DEFAULT_COUNT, 0, false, 0.0f),
        new ItemInfo(ItemIdEnum.PACKAGE, ImageIdEnum.ITEM_PACKAGE, "짐 꾸러미", DEFAULT_COUNT, 0, false, 0.0f),
        new ItemInfo(ItemIdEnum.MONEY, ImageIdEnum.ICON_COIN, "돈", DEFAULT_COUNT, 0, false, 0.0f),

        new ItemInfo(ItemIdEnum.MILK_PACK, ImageIdEnum.ITEM_MILK_PACK, "우유 팩", DEFAULT_COUNT, 1500, true, 5.0f),
        new ItemInfo(ItemIdEnum.MILK_BUCKET, ImageIdEnum.ITEM_MILK_BUCKET, "우유 양동이", DEFAULT_COUNT, 1000, true, 5.0f),
        new ItemInfo(ItemIdEnum.APPLE, ImageIdEnum.ITEM_APPLE, "사과", DEFAULT_COUNT, 1000, true, 5.0f),
        new ItemInfo(ItemIdEnum.APPLE_JUICE, ImageIdEnum.ITEM_APPLE_JUICE, "사과 쥬스", DEFAULT_COUNT, 5000, true, 5.0f),
        new ItemInfo(ItemIdEnum.BANANA, ImageIdEnum.ITEM_BANANA, "바나나", DEFAULT_COUNT, 2500, true, 10.0f),
        new ItemInfo(ItemIdEnum.BANANA_MILK, ImageIdEnum.ITEM_BANANA_MILK, "바나나 우유", DEFAULT_COUNT, 2000, true, 10.0f),
        new ItemInfo(ItemIdEnum.TOMATO, ImageIdEnum.ITEM_TOMATO, "토마토", DEFAULT_COUNT, 1000, true, 5.0f),
        new ItemInfo(ItemIdEnum.TOMATO_JUICE, ImageIdEnum.ITEM_TOMATO_JUICE, "토마토 쥬스", DEFAULT_COUNT, 5000, true, 5.0f),
        new ItemInfo(ItemIdEnum.WATERMELON, ImageIdEnum.ITEM_WATERMELON, "수박", DEFAULT_COUNT, 10000, true, 10.0f),
        new ItemInfo(ItemIdEnum.HALF_WATERMELON, ImageIdEnum.ITEM_HALF_WATERMELON, "수박 반 통", DEFAULT_COUNT, 5000, true, 5.0f),
        new ItemInfo(ItemIdEnum.WATERMELON_JUICE, ImageIdEnum.ITEM_WATERMELON_JUICE, "수박 쥬스", DEFAULT_COUNT, 5000, true, 10.0f),
        new ItemInfo(ItemIdEnum.CARROT, ImageIdEnum.ITEM_CARROT, "당근", DEFAULT_COUNT, 1500, true, 15.0f),
        new ItemInfo(ItemIdEnum.CARROT_JUICE, ImageIdEnum.ITEM_CARROT_JUICE, "당근 쥬스", DEFAULT_COUNT, 5000, true, 20.0f),
        new ItemInfo(ItemIdEnum.LEMON, ImageIdEnum.ITEM_LEMON, "레몬", DEFAULT_COUNT, 1500, true, 10.0f),
        new ItemInfo(ItemIdEnum.LEMON_JUICE, ImageIdEnum.ITEM_LEMON_JUICE, "레몬 에이드", DEFAULT_COUNT, 5000, true, 15.0f),
        new ItemInfo(ItemIdEnum.CHEESE, ImageIdEnum.ITEM_CHEESE, "치즈", DEFAULT_COUNT, 1000, true, 12.0f),
        new ItemInfo(ItemIdEnum.BEEF_SUSHI, ImageIdEnum.ITEM_BEEF_SUSHI, "소고기 초밥", DEFAULT_COUNT, 15000, true, 20.0f),
        new ItemInfo(ItemIdEnum.EGG_SUSHI, ImageIdEnum.ITEM_EGG_SUSHI, "계란 초밥", DEFAULT_COUNT, 10000, true, 20.0f),
        new ItemInfo(ItemIdEnum.SALMON_SUSHI, ImageIdEnum.ITEM_SALMON_SUSHI, "연어 초밥", DEFAULT_COUNT, 25000, true, 20.0f),
        new ItemInfo(ItemIdEnum.CHICKEN_SALAD, ImageIdEnum.ITEM_CHICKEN_SALAD, "닭가슴살 샐러드", DEFAULT_COUNT, 18000, true, 30.0f),
        new ItemInfo(ItemIdEnum.STEAK_SALAD, ImageIdEnum.ITEM_STEAK_SALAD, "스테이크 샐러드", DEFAULT_COUNT, 25000, true, 40.0f),
        new ItemInfo(ItemIdEnum.FROUT_SALAD, ImageIdEnum.ITEM_FROUT_SALAD, "과일 샐러드", DEFAULT_COUNT, 10000, true, 25.0f),
        new ItemInfo(ItemIdEnum.UNCOOKED_CHICKEN, ImageIdEnum.ITEM_UNCOOKED_CHICKEN, "생 닭고기", DEFAULT_COUNT, 5400, true, 2.0f),
        new ItemInfo(ItemIdEnum.UNCOOKED_LAMB, ImageIdEnum.ITEM_UNCOOKED_LAMB, "생 양고기", DEFAULT_COUNT, 13500, true, 2.0f),
        new ItemInfo(ItemIdEnum.UNCOOKED_PORK, ImageIdEnum.ITEM_UNCOOKED_PORK, "생 돼지고기", DEFAULT_COUNT, 7500, true, 2.0f),
        new ItemInfo(ItemIdEnum.UNCOOKED_BEEF, ImageIdEnum.ITEM_UNCOOKED_BEEF, "생 소고기", DEFAULT_COUNT, 10800, true, 2.0f),
        new ItemInfo(ItemIdEnum.COOKED_CHICKEN, ImageIdEnum.ITEM_COOKED_CHICKEN, "구운 닭고기", DEFAULT_COUNT, 8000, true, 20.0f),
        new ItemInfo(ItemIdEnum.COOKED_LAMB, ImageIdEnum.ITEM_COOKED_LAMB, "구운 양고기", DEFAULT_COUNT, 15000, true, 20.0f),
        new ItemInfo(ItemIdEnum.COOKED_PORK, ImageIdEnum.ITEM_COOKED_PORK, "구운 돼지고기", DEFAULT_COUNT, 10000, true, 20.0f),
        new ItemInfo(ItemIdEnum.COOKED_BEEF, ImageIdEnum.ITEM_COOKED_BEEF, "구운 소고기", DEFAULT_COUNT, 14000, true, 20.0f),
        new ItemInfo(ItemIdEnum.STEAK, ImageIdEnum.ITEM_STEAK, "스테이크", DEFAULT_COUNT, 20000, true, 30.0f),
        new ItemInfo(ItemIdEnum.EGG_TOAST, ImageIdEnum.ITEM_EGG_TOAST, "계란빵 토스트", DEFAULT_COUNT, 5000, true, 10.0f),
        new ItemInfo(ItemIdEnum.FISH_AND_CHIPS, ImageIdEnum.ITEM_FISH_AND_CHIPS, "피시 앤 칩스", DEFAULT_COUNT, 15000, true, 20.0f),
        new ItemInfo(ItemIdEnum.HAMBURGER, ImageIdEnum.ITEM_HAMBURGER, "햄버거", DEFAULT_COUNT, 13000, true, 15.0f),
        new ItemInfo(ItemIdEnum.MASHED_POTATO, ImageIdEnum.ITEM_MASHED_POTATO, "으깬 감자", DEFAULT_COUNT, 3000, true, 15.0f),
        new ItemInfo(ItemIdEnum.PIZZA, ImageIdEnum.ITEM_PIZZA, "피자", DEFAULT_COUNT, 23000, true, 20.0f),
        new ItemInfo(ItemIdEnum.RICE, ImageIdEnum.ITEM_RICE, "밥", DEFAULT_COUNT, 2000, true, 25.0f),
        new ItemInfo(ItemIdEnum.BREAD, ImageIdEnum.ITEM_BREAD, "빵", DEFAULT_COUNT, 3000, true, 20.0f),

        new ItemInfo(ItemIdEnum.HORSE_LEATHER, ImageIdEnum.ITEM_HORSE_LEATHER, "말 가죽", DEFAULT_COUNT, 30000, false, 0.0f),
        new ItemInfo(ItemIdEnum.EGG, ImageIdEnum.ITEM_EGG, "달걀", DEFAULT_COUNT, 7000, true, 5.0f),
        new ItemInfo(ItemIdEnum.FISH, ImageIdEnum.ITEM_FISH, "생선", DEFAULT_COUNT, 10000, true, 5.0f),
        new ItemInfo(ItemIdEnum.SALMON, ImageIdEnum.ITEM_SALMON, "연어", DEFAULT_COUNT, 20000, true, 10.0f),

        new ItemInfo(ItemIdEnum.GOLD_ORE, ImageIdEnum.ITEM_GOLD_ORE, "금 원석", DEFAULT_COUNT, 14000, false, 0.0f),
        new ItemInfo(ItemIdEnum.GOLD_INGOT, ImageIdEnum.ITEM_GOLD_INGOT, "금 주괴", DEFAULT_COUNT, 19000, false, 0.0f),
        new ItemInfo(ItemIdEnum.GOLD_NECKLACE, ImageIdEnum.ITEM_GOLD_NECKLACE, "금 목걸이", DEFAULT_COUNT, 25000, false, 0.0f),
        new ItemInfo(ItemIdEnum.SILVER_ORE, ImageIdEnum.ITEM_SILVER_ORE, "은 원석", DEFAULT_COUNT, 12000, false, 0.0f),
        new ItemInfo(ItemIdEnum.SILVER_INGOT, ImageIdEnum.ITEM_SILVER_INGOT, "은 주괴", DEFAULT_COUNT, 16000, false, 0.0f),
        new ItemInfo(ItemIdEnum.SILVER_NECKLACE, ImageIdEnum.ITEM_SILVER_NECKLACE, "은 목걸이", DEFAULT_COUNT, 20000, false, 0.0f),
        new ItemInfo(ItemIdEnum.IRON_ORE, ImageIdEnum.ITEM_IRON_ORE, "철 원석", DEFAULT_COUNT, 10000, false, 0.0f),
        new ItemInfo(ItemIdEnum.IRON_INGOT, ImageIdEnum.ITEM_IRON_INGOT, "철 주괴", DEFAULT_COUNT, 13000, false, 0.0f),
        new ItemInfo(ItemIdEnum.IRON_NECKLACE, ImageIdEnum.ITEM_IRON_NECKLACE, "철 목걸이", DEFAULT_COUNT, 15000, false, 0.0f),

        new ItemInfo(ItemIdEnum.CARROT_SEED, ImageIdEnum.ITEM_CARROT_SEED, "당근 씨앗", DEFAULT_COUNT, 100, true, 2.0f),
        new ItemInfo(ItemIdEnum.LEMMON_SEED, ImageIdEnum.ITEM_LEMMON_SEED, "레몬 씨앗", DEFAULT_COUNT, 100, true, 2.0f),
        new ItemInfo(ItemIdEnum.RICE_SEED, ImageIdEnum.ITEM_RICE_SEED, "볍씨", DEFAULT_COUNT, 100, true, 2.0f),
        new ItemInfo(ItemIdEnum.TOMATO_SEED, ImageIdEnum.ITEM_TOMATO_SEED, "토마토 씨앗", DEFAULT_COUNT, 100, true, 2.0f),
        new ItemInfo(ItemIdEnum.WATERMELON_SEED, ImageIdEnum.ITEM_WATERMELON_SEED, "수박 씨앗", DEFAULT_COUNT, 100, true, 2.0f),
        new ItemInfo(ItemIdEnum.POTATO, ImageIdEnum.ITEM_POTATO, "감자", DEFAULT_COUNT, 1000, true, 5.0f),
        new ItemInfo(ItemIdEnum.CUCUMBER, ImageIdEnum.ITEM_CUCUMBER, "오이", DEFAULT_COUNT, 2000, true, 10.0f),
        new ItemInfo(ItemIdEnum.FLOUR, ImageIdEnum.ITEM_FLOUR, "밀가루", DEFAULT_COUNT, 500, true, 2.0f),
        new ItemInfo(ItemIdEnum.RICE_PLANT, ImageIdEnum.ITEM_RICE_PLANT, "벼", DEFAULT_COUNT, 100, true, 2.0f),
        new ItemInfo(ItemIdEnum.REFINED_RICE, ImageIdEnum.ITEM_REFINED_RICE, "정제된 쌀", DEFAULT_COUNT, 1000, true, 5.0f),

        new ItemInfo(ItemIdEnum.HOE, ImageIdEnum.ITEM_HOE, "괭이", DEFAULT_COUNT, 10000, false, 0.0f),
        new ItemInfo(ItemIdEnum.RAKE, ImageIdEnum.ITEM_RAKE, "갈퀴", DEFAULT_COUNT, 16000, false, 0.0f),
        new ItemInfo(ItemIdEnum.PICKAXE, ImageIdEnum.ITEM_PICKAXE, "곡괭이", DEFAULT_COUNT, 18000, false, 0.0f),
        new ItemInfo(ItemIdEnum.SHOVEL, ImageIdEnum.ITEM_SHOVEL, "삽", DEFAULT_COUNT, 14000, false, 0.0f),
        new ItemInfo(ItemIdEnum.KNIFE, ImageIdEnum.ITEM_KNIFE, "칼", DEFAULT_COUNT, 20000, false, 0.0f),
        new ItemInfo(ItemIdEnum.SICKLE, ImageIdEnum.ITEM_SICKLE, "낫", DEFAULT_COUNT, 12000, false, 0.0f),
        new ItemInfo(ItemIdEnum.STRIPED_MARLIN, ImageIdEnum.ITEM_STRIPED_MARLIN, "청새치", DEFAULT_COUNT, 50000, false, 0.0f),

        new ItemInfo(ItemIdEnum.BED, ImageIdEnum.ITEM_BED, "침대", DEFAULT_COUNT, 50000, false, 0.0f)
    };

    /*==================================================
     *                 Util Method
     *==================================================*/

    private static ItemInfo findItemInfo(ItemIdEnum itemId)
    {
        // 현재 O(N) 알고리즘 -> 리팩터링 요소
        foreach (ItemInfo info in getInstance().informations)
            if (info.itemId == itemId)
                return info;
        throw new Exception("오류 : " + itemId.ToString() + "에 대한 아이템 정보가 등록되지 않았습니다!");
    }

    /*==================================================
     *                 Item Info Getter 
     *==================================================*/

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
    /// 아이템 정보를 반환합니다.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public static ItemInfo getItemInfo(ItemIdEnum itemId)
    {
        return findItemInfo(itemId);
    }
}

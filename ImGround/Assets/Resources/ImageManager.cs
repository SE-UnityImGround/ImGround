using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager
{
    /*==================================================
     *                 싱글톤 관리자
     *==================================================*/

    /// <summary>
    /// 절대 이 멤버로 접근하지 말 것. <see cref="getInstance"/>로 접근하세요.
    /// </summary>
    private static ImageManager instance = null;

    private ImageManager()
    {

    }

    /// <summary>
    /// 싱글톤으로 사용하기 위해 인스턴스 초기화 및 가져오는 메소드입니다.
    /// </summary>
    /// <returns></returns>
    private static ImageManager getInstance()
    {
        if (instance == null)
        {
            instance = new ImageManager();
            loadImageFiles();
            checkImageLoaded();
        }
        return instance;
    }

    /// <summary>
    /// 현재 클래스에 등록되어 있는 이미지 정보에 대해 Resource 폴더로부터 이미지 파일 에셋을 로딩합니다.
    /// </summary>
    /// <param name="instance"></param>
    private static void loadImageFiles()
    {
        string faildList = "";
        foreach (ImageInfo info in getInstance().imageData)
        {
            if (!info.load())
            {
                faildList += info.resourcePath + "\n";
            }
        }
        if (faildList.Length > 0)
            Debug.LogError("다음의 이미지를 로딩하지 못했습니다! :\n" + faildList);
    }

    /// <summary>
    /// 모든 이미지 파일이 등록되었는지 확인합니다.
    /// </summary>
    private static void checkImageLoaded()
    {
        string omittedImages = "";
        foreach (ImageIdEnum id in Enum.GetValues(typeof(ImageIdEnum)))
        {
            try
            {
                findImageInfo(id);
            }
            catch
            {
                omittedImages += id.ToString() + "\n";
            }
        }
        if (omittedImages.Length > 0)
            Debug.LogError("다음의 이미지가 아직 등록되지 않았습니다! :\n" + omittedImages);
    }

    /*==================================================
     *                 Util Method
     *==================================================*/

    private static ImageInfo findImageInfo(ImageIdEnum id)
    {
        // 현재 O(N) 알고리즘. 성능 개선 필요시 고려해야 할 부분임!
        foreach (ImageInfo info in getInstance().imageData)
        {
            if (info.id == id)
            {
                return info;
            }
        }
        throw new Exception("오류 : " + id.ToString() + "에 대한 이미지가 등록되지 않았습니다!");
    }

    /*==================================================
     *                 Image 데이터 
     *==================================================*/

    /// <summary>
    /// 이미지 데이터를 Image ID에 따라 저장합니다. 로드되기 전에는 이미지는 모두 null입니다.
    /// </summary>
    private ImageInfo[] imageData = {
        
        /*=============================
         *            System
         *=============================*/

        new ImageInfo(ImageIdEnum.NULL, null),
        
        /*=============================
         *            ITEM
         *=============================*/
        
        new ImageInfo(ImageIdEnum.ITEM_NULL, null),

        new ImageInfo(ImageIdEnum.ITEM_MILK_PACK, "Images/Item2D/Food/곽우유"),
        new ImageInfo(ImageIdEnum.ITEM_MILK_BUCKET, "Images/Item2D/Food/양동이우유"),

        new ImageInfo(ImageIdEnum.ITEM_APPLE, "Images/Item2D/Food/apple"),
        new ImageInfo(ImageIdEnum.ITEM_BANANA, "Images/Item2D/Food/banana"),
        new ImageInfo(ImageIdEnum.ITEM_BANANA_MILK, "Images/Item2D/Food/banana milk"),
        new ImageInfo(ImageIdEnum.ITEM_WATERMELON, "Images/Item2D/Food/watermelon"),
        new ImageInfo(ImageIdEnum.ITEM_HALF_WATERMELON, "Images/Item2D/Food/half watermelon"),
        new ImageInfo(ImageIdEnum.ITEM_WATERMELON_JUICE, "Images/Item2D/Food/watermelon juice"),
        new ImageInfo(ImageIdEnum.ITEM_CARROT_JUICE, "Images/Item2D/Food/carrot juice"),
        new ImageInfo(ImageIdEnum.ITEM_LEMON, "Images/Item2D/Food/lemon"),
        new ImageInfo(ImageIdEnum.ITEM_LEMON_JUICE, "Images/Item2D/Food/lemon juice"),
        new ImageInfo(ImageIdEnum.ITEM_CHEESE, "Images/Item2D/Food/치즈"),
        new ImageInfo(ImageIdEnum.ITEM_BEEF_SUSHI, "Images/Item2D/Food/Beef sushi"),
        new ImageInfo(ImageIdEnum.ITEM_EGG_SUSHI, "Images/Item2D/Food/egg sushi"),
        new ImageInfo(ImageIdEnum.ITEM_SALMON_SUSHI, "Images/Item2D/Food/salmon sushi"),
        new ImageInfo(ImageIdEnum.ITEM_CHICKEN_SALAD, "Images/Item2D/Food/chicken salad"),
        new ImageInfo(ImageIdEnum.ITEM_STEAK_SALAD, "Images/Item2D/Food/steak salad"),
        new ImageInfo(ImageIdEnum.ITEM_FROUT_SALAD, "Images/Item2D/Food/과일샐러드"),
        new ImageInfo(ImageIdEnum.ITEM_UNCOOKED_CHICKEN, "Images/Item2D/Food/uncooked chicken"),
        new ImageInfo(ImageIdEnum.ITEM_UNCOOKED_LAMB, "Images/Item2D/Food/uncooked lamb"),
        new ImageInfo(ImageIdEnum.ITEM_UNCOOKED_PORK, "Images/Item2D/Food/uncooked pork"),
        new ImageInfo(ImageIdEnum.ITEM_UNCOOKED_BEEF, "Images/Item2D/Food/소고기"),
        new ImageInfo(ImageIdEnum.ITEM_COOKED_CHICKEN, "Images/Item2D/Food/cooked chicken"),
        new ImageInfo(ImageIdEnum.ITEM_COOKED_LAMB, "Images/Item2D/Food/cooked lamb"),
        new ImageInfo(ImageIdEnum.ITEM_COOKED_PORK, "Images/Item2D/Food/cooked pork"),
        new ImageInfo(ImageIdEnum.ITEM_COOKED_BEEF, "Images/Item2D/Food/익힌 소고기"),
        new ImageInfo(ImageIdEnum.ITEM_STEAK, "Images/Item2D/Food/steak"),
        new ImageInfo(ImageIdEnum.ITEM_EGG_TOAST, "Images/Item2D/Food/egg toast"),
        new ImageInfo(ImageIdEnum.ITEM_FISH_AND_CHIPS, "Images/Item2D/Food/fish and chips"),
        new ImageInfo(ImageIdEnum.ITEM_HAMBURGER, "Images/Item2D/Food/hamburger"),
        new ImageInfo(ImageIdEnum.ITEM_MASHED_POTATO, "Images/Item2D/Food/mashed potato"),
        new ImageInfo(ImageIdEnum.ITEM_PIZZA, "Images/Item2D/Food/pizza"),
        new ImageInfo(ImageIdEnum.ITEM_RICE, "Images/Item2D/Food/rice"),
        new ImageInfo(ImageIdEnum.ITEM_BREAD, "Images/Item2D/Food/빵"),

        new ImageInfo(ImageIdEnum.ITEM_HORSE_LEATHER, "Images/Item2D/horse leather"),
        new ImageInfo(ImageIdEnum.ITEM_EGG, "Images/Item2D/Food/egg"),
        new ImageInfo(ImageIdEnum.ITEM_FISH, "Images/Item2D/Food/fish"),
        new ImageInfo(ImageIdEnum.ITEM_SALMON, "Images/Item2D/Food/salmon"),

        new ImageInfo(ImageIdEnum.ITEM_GOLD_ORE, "Images/Item2D/mineral/before gold"),
        new ImageInfo(ImageIdEnum.ITEM_GOLD_INGOT, "Images/Item2D/mineral/gold"),
        new ImageInfo(ImageIdEnum.ITEM_GOLD_NECKLACE, "Images/Item2D/mineral/gold necklace"),
        new ImageInfo(ImageIdEnum.ITEM_SILVER_ORE, "Images/Item2D/mineral/before silver"),
        new ImageInfo(ImageIdEnum.ITEM_SILVER_INGOT, "Images/Item2D/mineral/silver"),
        new ImageInfo(ImageIdEnum.ITEM_SILVER_NECKLACE, "Images/Item2D/mineral/silver necklace"),
        new ImageInfo(ImageIdEnum.ITEM_IRON_ORE, "Images/Item2D/mineral/before iron"),
        new ImageInfo(ImageIdEnum.ITEM_IRON_INGOT, "Images/Item2D/mineral/iron"),
        new ImageInfo(ImageIdEnum.ITEM_IRON_NECKLACE, "Images/Item2D/mineral/iron neck lave"),

        new ImageInfo(ImageIdEnum.ITEM_CARROT_SEED, "Images/Item2D/seed/carrot seed"),
        new ImageInfo(ImageIdEnum.ITEM_LEMMON_SEED, "Images/Item2D/seed/lemon seed"),
        new ImageInfo(ImageIdEnum.ITEM_RICE_SEED, "Images/Item2D/seed/rice seed"),
        new ImageInfo(ImageIdEnum.ITEM_TOMATO_SEED, "Images/Item2D/seed/tomato seed"),
        new ImageInfo(ImageIdEnum.ITEM_WATERMELON_SEED, "Images/Item2D/seed/watermelon seed"),
        new ImageInfo(ImageIdEnum.ITEM_POTATO, "Images/Item2D/Food/감자"),
        new ImageInfo(ImageIdEnum.ITEM_CUCUMBER, "Images/Item2D/Food/오이"),
        new ImageInfo(ImageIdEnum.ITEM_FLOUR, "Images/Item2D/Food/flour"),

        /*=============================
         *            ICON
         *=============================*/

        new ImageInfo(ImageIdEnum.ICON_COIN, "Images/Coin"),
        new ImageInfo(ImageIdEnum.ICON_MEAT, "Images/Meat"),
        new ImageInfo(ImageIdEnum.ICON_GEM, "Images/Gem"),
        new ImageInfo(ImageIdEnum.ICON_INGOT, "Images/Ingots"),
        new ImageInfo(ImageIdEnum.ICON_HAMMER, "Images/Hammer"),
        new ImageInfo(ImageIdEnum.ICON_WOOD, "Images/Wood"),
        new ImageInfo(ImageIdEnum.ICON_ARCHER, "Images/Icons/UI_Icon_Archer"),
        new ImageInfo(ImageIdEnum.ICON_ATTACK, "Images/Icons/UI_Icon_Attack"),

        /*=============================
         *              UI
         *=============================*/

        new ImageInfo(ImageIdEnum.UI_LIFE_0, "Images/UI/Life_0"),
        new ImageInfo(ImageIdEnum.UI_LIFE_30, "Images/UI/Life_30"),
        new ImageInfo(ImageIdEnum.UI_LIFE_50, "Images/UI/Life_50"),
        new ImageInfo(ImageIdEnum.UI_LIFE_80, "Images/UI/Life_80"),
        new ImageInfo(ImageIdEnum.UI_LIFE_100, "Images/UI/Life_100")
    };

    /*==================================================
     *                 Image Getter 
     *==================================================*/

    /// <summary>
    /// 주어진 이미지 ID로 이미지를 반환합니다.
    /// </summary>
    /// <param name="imageId"></param>
    /// <returns></returns>
    public static Sprite getImage(ImageIdEnum imageId)
    {
        return findImageInfo(imageId).img;
    }
}

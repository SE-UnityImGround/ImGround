using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager
{
    /*==================================================
     *                 싱글톤 관리자
     *==================================================*/

    private static ImageManager instance = null;

    /// <summary>
    /// 싱글톤으로 사용하기 위해 인스턴스 초기화 및 가져오는 메소드입니다.
    /// </summary>
    /// <returns></returns>
    private static ImageManager getImagesSO()
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
        foreach (ImageInfo info in getImagesSO().imageData)
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
            catch (Exception e)
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
        foreach (ImageInfo info in getImagesSO().imageData)
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
        new ImageInfo(ImageIdEnum.ITEM_MILK_PACK, "Images/Item2D/곽우유"),
        new ImageInfo(ImageIdEnum.ITEM_MILK_BUCKET, "Images/Item2D/양동이우유"),
        new ImageInfo(ImageIdEnum.ITEM_HORSE_LEATHER, "Images/Item2D/horse leather"),
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
        new ImageInfo(ImageIdEnum.ICON_ATTACK, "Images/Icons/UI_Icon_Attack")
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

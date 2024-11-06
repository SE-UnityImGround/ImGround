using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 이 클래스는 ScriptableObject로, 에셋으로 생성하여 사용해야 합니다!
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Images")]
public class ImageManager : ScriptableObject
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
            setImageFiles(instance);
            checkImageLoaded();
        }
        return instance;
    }

    /// <summary>
    /// 모든 이미지 파일이 등록되었는지 확인합니다.
    /// </summary>
    private static void checkImageLoaded()
    {
        foreach (ImageIdEnum d in Enum.GetValues(typeof(ImageIdEnum)))
        {
            try
            {
                if (getImage(d) == null
                    && d != ImageIdEnum.NULL
                    && d != ImageIdEnum.ITEM_NULL)
                {
                    Debug.Log("Image Null : " + d.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.LogError("● 이미지가 등록되지 않았습니다!!");
                throw e;
            }
        }
    }

    /*==================================================
     *                 Image 데이터 
     *==================================================*/

    private readonly Sprite NULL = null;

    private readonly Sprite ITEM_NULL = null;
    private Sprite ITEM_MILK_PACK;
    private Sprite ITEM_MILK_BUCKET;
    private Sprite ITEM_HORSE_LEATHER;
    private Sprite ITEM_GOLD_ORE;
    private Sprite ITEM_GOLD_INGOT;
    private Sprite ITEM_GOLD_NECKLACE;
    private Sprite ITEM_SILVER_ORE;
    private Sprite ITEM_SILVER_INGOT;
    private Sprite ITEM_SILVER_NECKLACE;
    private Sprite ITEM_IRON_ORE;
    private Sprite ITEM_IRON_INGOT;
    private Sprite ITEM_IRON_NECKLACE;
    private Sprite ITEM_CARROT_SEED;
    private Sprite ITEM_LEMMON_SEED;
    private Sprite ITEM_RICE_SEED;
    private Sprite ITEM_TOMATO_SEED;
    private Sprite ITEM_WATERMELON_SEED;

    private Sprite ICON_COIN;
    private Sprite ICON_MEAT;
    private Sprite ICON_GEM;
    private Sprite ICON_INGOT;
    private Sprite ICON_HAMMER;
    private Sprite ICON_WOOD;
    private Sprite ICON_ARCHER;
    private Sprite ICON_ATTACK;

    private static void setImageFiles(ImageManager instance)
    {
        instance.ITEM_MILK_PACK = Resources.Load<Sprite>("Images/Item2D/곽우유");
        instance.ITEM_MILK_BUCKET = Resources.Load<Sprite>("Images/Item2D/양동이우유");
        instance.ITEM_HORSE_LEATHER = Resources.Load<Sprite>("Images/Item2D/horse leather");
        instance.ITEM_GOLD_ORE = Resources.Load<Sprite>("Images/Item2D/mineral/before gold");
        instance.ITEM_GOLD_INGOT = Resources.Load<Sprite>("Images/Item2D/mineral/gold");
        instance.ITEM_GOLD_NECKLACE = Resources.Load<Sprite>("Images/Item2D/mineral/gold necklace");
        instance.ITEM_SILVER_ORE = Resources.Load<Sprite>("Images/Item2D/mineral/before silver");
        instance.ITEM_SILVER_INGOT = Resources.Load<Sprite>("Images/Item2D/mineral/silver");
        instance.ITEM_SILVER_NECKLACE = Resources.Load<Sprite>("Images/Item2D/mineral/silver necklace");
        instance.ITEM_IRON_ORE = Resources.Load<Sprite>("Images/Item2D/mineral/before iron");
        instance.ITEM_IRON_INGOT = Resources.Load<Sprite>("Images/Item2D/mineral/iron");
        instance.ITEM_IRON_NECKLACE = Resources.Load<Sprite>("Images/Item2D/mineral/iron neck lave");
        instance.ITEM_CARROT_SEED = Resources.Load<Sprite>("Images/Item2D/seed/carrot seed");
        instance.ITEM_LEMMON_SEED = Resources.Load<Sprite>("Images/Item2D/seed/lemon seed");
        instance.ITEM_RICE_SEED = Resources.Load<Sprite>("Images/Item2D/seed/rice seed");
        instance.ITEM_TOMATO_SEED = Resources.Load<Sprite>("Images/Item2D/seed/tomato seed");
        instance.ITEM_WATERMELON_SEED = Resources.Load<Sprite>("Images/Item2D/seed/watermelon seed");

        instance.ICON_COIN = Resources.Load<Sprite>("Images/Coin");
        instance.ICON_MEAT = Resources.Load<Sprite>("Images/Meat");
        instance.ICON_GEM = Resources.Load<Sprite>("Images/Gem");
        instance.ICON_INGOT = Resources.Load<Sprite>("Images/Ingots");
        instance.ICON_HAMMER = Resources.Load<Sprite>("Images/Hammer");
        instance.ICON_WOOD = Resources.Load<Sprite>("Images/Wood");
        instance.ICON_ARCHER = Resources.Load<Sprite>("Images/Icons/UI_Icon_Archer");
        instance.ICON_ATTACK = Resources.Load<Sprite>("Images/Icons/UI_Icon_Attack");
    }

    /// <summary>
    /// 주어진 이미지 ID로 이미지를 반환합니다.
    /// </summary>
    /// <param name="imageId"></param>
    /// <returns></returns>
    public static Sprite getImage(ImageIdEnum imageId)
    {
        switch (imageId)
        {
            /*=============================
             *            System
             *=============================*/

            case ImageIdEnum.NULL:
                return getImagesSO().NULL;

            /*=============================
             *            ITEM
             *=============================*/

            case ImageIdEnum.ITEM_NULL:
                return getImagesSO().ITEM_NULL;

            case ImageIdEnum.ITEM_MILK_PACK:
                return getImagesSO().ITEM_MILK_PACK;
            case ImageIdEnum.ITEM_MILK_BUCKET:
                return getImagesSO().ITEM_MILK_BUCKET;
            case ImageIdEnum.ITEM_HORSE_LEATHER:
                return getImagesSO().ITEM_HORSE_LEATHER;

            case ImageIdEnum.ITEM_GOLD_ORE:
                return getImagesSO().ITEM_GOLD_ORE;
            case ImageIdEnum.ITEM_GOLD_INGOT:
                return getImagesSO().ITEM_GOLD_INGOT;
            case ImageIdEnum.ITEM_GOLD_NECKLACE:
                return getImagesSO().ITEM_GOLD_NECKLACE;
            case ImageIdEnum.ITEM_SILVER_ORE:
                return getImagesSO().ITEM_SILVER_ORE;
            case ImageIdEnum.ITEM_SILVER_INGOT:
                return getImagesSO().ITEM_SILVER_INGOT;
            case ImageIdEnum.ITEM_SILVER_NECKLACE:
                return getImagesSO().ITEM_SILVER_NECKLACE;
            case ImageIdEnum.ITEM_IRON_ORE:
                return getImagesSO().ITEM_IRON_ORE;
            case ImageIdEnum.ITEM_IRON_INGOT:
                return getImagesSO().ITEM_IRON_INGOT;
            case ImageIdEnum.ITEM_IRON_NECKLACE:
                return getImagesSO().ITEM_IRON_NECKLACE;

            case ImageIdEnum.ITEM_CARROT_SEED:
                return getImagesSO().ITEM_CARROT_SEED;
            case ImageIdEnum.ITEM_LEMMON_SEED:
                return getImagesSO().ITEM_LEMMON_SEED;
            case ImageIdEnum.ITEM_RICE_SEED:
                return getImagesSO().ITEM_RICE_SEED;
            case ImageIdEnum.ITEM_TOMATO_SEED:
                return getImagesSO().ITEM_TOMATO_SEED;
            case ImageIdEnum.ITEM_WATERMELON_SEED:
                return getImagesSO().ITEM_WATERMELON_SEED;


            /*=============================
             *            ICON
             *=============================*/

            case ImageIdEnum.ICON_COIN:
                return getImagesSO().ICON_COIN;
            case ImageIdEnum.ICON_MEAT:
                return getImagesSO().ICON_MEAT;
            case ImageIdEnum.ICON_GEM:
                return getImagesSO().ICON_GEM;
            case ImageIdEnum.ICON_INGOT:
                return getImagesSO().ICON_INGOT;
            case ImageIdEnum.ICON_HAMMER:
                return getImagesSO().ICON_HAMMER;
            case ImageIdEnum.ICON_WOOD:
                return getImagesSO().ICON_WOOD;
            case ImageIdEnum.ICON_ARCHER:
                return getImagesSO().ICON_ARCHER;
            case ImageIdEnum.ICON_ATTACK:
                return getImagesSO().ICON_ATTACK;

            default:
                throw new Exception(imageId.ToString() + "에 대한 이미지가 등록되지 않았습니다!");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager
{
    /*==================================================
     *                 �̱��� ������
     *==================================================*/

    private static ImageManager instance = null;

    /// <summary>
    /// �̱������� ����ϱ� ���� �ν��Ͻ� �ʱ�ȭ �� �������� �޼ҵ��Դϴ�.
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
    /// ���� Ŭ������ ��ϵǾ� �ִ� �̹��� ������ ���� Resource �����κ��� �̹��� ���� ������ �ε��մϴ�.
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
            Debug.LogError("������ �̹����� �ε����� ���߽��ϴ�! :\n" + faildList);
    }

    /// <summary>
    /// ��� �̹��� ������ ��ϵǾ����� Ȯ���մϴ�.
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
            Debug.LogError("������ �̹����� ���� ��ϵ��� �ʾҽ��ϴ�! :\n" + omittedImages);
    }

    /*==================================================
     *                 Util Method
     *==================================================*/

    private static ImageInfo findImageInfo(ImageIdEnum id)
    {
        // ���� O(N) �˰���. ���� ���� �ʿ�� ����ؾ� �� �κ���!
        foreach (ImageInfo info in getImagesSO().imageData)
        {
            if (info.id == id)
            {
                return info;
            }
        }
        throw new Exception("���� : " + id.ToString() + "�� ���� �̹����� ��ϵ��� �ʾҽ��ϴ�!");
    }

    /*==================================================
     *                 Image ������ 
     *==================================================*/

    /// <summary>
    /// �̹��� �����͸� Image ID�� ���� �����մϴ�. �ε�Ǳ� ������ �̹����� ��� null�Դϴ�.
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
        new ImageInfo(ImageIdEnum.ITEM_MILK_PACK, "Images/Item2D/������"),
        new ImageInfo(ImageIdEnum.ITEM_MILK_BUCKET, "Images/Item2D/�絿�̿���"),
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
    /// �־��� �̹��� ID�� �̹����� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="imageId"></param>
    /// <returns></returns>
    public static Sprite getImage(ImageIdEnum imageId)
    {
        return findImageInfo(imageId).img;
    }
}

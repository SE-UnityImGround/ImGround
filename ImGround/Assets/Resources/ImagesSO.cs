using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 이 클래스는 ScriptableObject로, 에셋으로 생성하여 사용해야 합니다!
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Images")]
public class ImagesSO : ScriptableObject
{
    /*==================================================
     *                 Static 접근자 
     *==================================================*/

    private static ImagesSO instance = null;

    private static ImagesSO getImagesSO()
    {
        if (instance == null)
        {
            instance = Resources.Load<ImagesSO>("ImagesSO");
            if (instance == null)
            {
                Debug.LogError("Resources 폴더에 ImagesSO ScriptableObject에 대한 에셋이 등록되지 않았습니다!");
            }
        }
        return instance;
    }

    public static Sprite getImage(ImageIdEnum imageId)
    {
        switch (imageId)
        {
            case ImageIdEnum.ITEM_NULL:
                return getImagesSO().ITEM_NULL;

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
                return getImagesSO().NULL;
        }
    }

    public static Sprite getImage(ItemIdEnum itemId)
    {
        switch (itemId)
        {
            case ItemIdEnum.TEST_NULL_ITEM:
                return getImage(ImageIdEnum.ITEM_NULL);

            default:
                return getImage(ImageIdEnum.NULL);
        }
    }

    /*==================================================
     *                 Image 데이터들 
     *==================================================*/

    public readonly Sprite NULL = null;

    public readonly Sprite ITEM_NULL = null;
    
    public Sprite ICON_COIN;
    public Sprite ICON_MEAT;
    public Sprite ICON_GEM;
    public Sprite ICON_INGOT;
    public Sprite ICON_HAMMER;
    public Sprite ICON_WOOD;
    public Sprite ICON_ARCHER;
    public Sprite ICON_ATTACK;
}

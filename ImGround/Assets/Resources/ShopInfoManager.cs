using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShopInfoManager
{
    /*=====================================
     *          가게별 물품 정보
     *=====================================*/

    private static Dictionary<ShopIdEnum, ShopInfo> shopInfos =
        new Dictionary<ShopIdEnum, ShopInfo>()
        {
            {
                ShopIdEnum.FISH,
                new ShopInfo(
                    "생선 가게",
                    true,
                    new ItemBundle(ItemIdEnum.FISH, 1, false),
                    new ItemBundle(ItemIdEnum.SALMON, 1, false))
            },
            {
                ShopIdEnum.BREAD,
                new ShopInfo(
                    "빵 가게",
                    true,
                    new ItemBundle(ItemIdEnum.BREAD, 1, false),
                    new ItemBundle(ItemIdEnum.CHEESE, 1, false),
                    new ItemBundle(ItemIdEnum.RICE, 1, false),
                    new ItemBundle(ItemIdEnum.FLOUR, 1, false))
            },
            {
                ShopIdEnum.FRUIT,
                new ShopInfo(
                    "과일 가게",
                    true,
                    new ItemBundle(ItemIdEnum.APPLE, 1, false),
                    new ItemBundle(ItemIdEnum.LEMON, 1, false),
                    new ItemBundle(ItemIdEnum.TOMATO, 1, false),
                    new ItemBundle(ItemIdEnum.BANANA, 1, false),
                    new ItemBundle(ItemIdEnum.WATERMELON, 1, false),
                    new ItemBundle(ItemIdEnum.CARROT, 1, false),
                    new ItemBundle(ItemIdEnum.POTATO, 1, false),
                    new ItemBundle(ItemIdEnum.CUCUMBER, 1, false),
                    new ItemBundle(ItemIdEnum.RICE_SEED, 1, false),
                    new ItemBundle(ItemIdEnum.LEMMON_SEED, 1, false),
                    new ItemBundle(ItemIdEnum.TOMATO_SEED, 1, false),
                    new ItemBundle(ItemIdEnum.WATERMELON_SEED, 1, false),
                    new ItemBundle(ItemIdEnum.CARROT_SEED, 1, false))
            },
            {
                ShopIdEnum.TOOLS,
                new ShopInfo(
                    "도구 상점",
                    true,
                    new ItemBundle(ItemIdEnum.HOE, 1, false),
                    new ItemBundle(ItemIdEnum.RAKE, 1, false),
                    new ItemBundle(ItemIdEnum.PICKAXE, 1, false),
                    new ItemBundle(ItemIdEnum.SHOVEL, 1, false),
                    new ItemBundle(ItemIdEnum.KNIFE, 1, false),
                    new ItemBundle(ItemIdEnum.SICKLE, 1, false))
            },
            {
                ShopIdEnum.MEAT,
                new ShopInfo(
                    "육류 상점",
                    true,
                    new ItemBundle(ItemIdEnum.UNCOOKED_BEEF, 1, false),
                    new ItemBundle(ItemIdEnum.UNCOOKED_CHICKEN, 1, false),
                    new ItemBundle(ItemIdEnum.UNCOOKED_LAMB, 1, false),
                    new ItemBundle(ItemIdEnum.UNCOOKED_PORK, 1, false))
            },
            {
                ShopIdEnum.SELLER,
                new ShopInfo(
                    "판매",
                    false)
            }
        };

    /*=====================================
     *          NPC별 가게 할당
     *=====================================*/

    private static Dictionary<NPCType, ShopIdEnum> npcShopMap = new Dictionary<NPCType, ShopIdEnum>()
    {
        { NPCType.SHOP_BREAD, ShopIdEnum.BREAD },
        { NPCType.SHOP_FISH, ShopIdEnum.FISH },
        { NPCType.SHOP_FRUIT, ShopIdEnum.FRUIT },
        { NPCType.SHOP_INDUSTRY, ShopIdEnum.INDUSTRY },
        { NPCType.SHOP_MEAT, ShopIdEnum.MEAT },
        { NPCType.SHOP_SELLER, ShopIdEnum.SELLER },
        { NPCType.SHOP_TOOL, ShopIdEnum.TOOLS }
    };

    /*=====================================
     *          외부 지원 메소드
     *=====================================*/

    public static ShopInfo getShopInfo(ShopIdEnum shopId)
    {
        if (shopInfos.ContainsKey(shopId))
            return shopInfos[shopId];
        throw new Exception("상점 " + shopId.ToString() + "의 정보가 등록되지 않음.");
    }
    public static ShopInfo getShopInfo(NPCType npcType)
    {
        if (npcShopMap.ContainsKey(npcType))
            return getShopInfo(npcShopMap[npcType]);
        throw new Exception("NPC " + npcType.ToString() + "에 대한 상점이 존재하지 않음.");
    }
}

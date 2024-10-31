using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 아이템의 각종 고유정보를 저장하는 클래스입니다.
/// 아이템은 ItemIdEnum으로 구분되며, 고유정보에는 이름, 최대 중첩가능 수 등이 포함됩니다.
/// </summary>
public class ItemsInfo
{
    /// <summary>
    /// 아이템의 id별 고유 이름을 반환합니다.
    /// <br/>잘못된 id에 대해 null을 반환합니다.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public static string getItemName(ItemIdEnum itemId)
    {
        switch (itemId)
        {
            case ItemIdEnum.TEST_NULL_ITEM:
                return "TestNumItem";
            default:
                return null;
        }
    }

    /// <summary>
    /// 아이템의 id별 고유 중첩가능 수를 반환합니다.
    /// <br/>잘못된 id에 대해 -1을 반환합니다.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public static int getMaxCount(ItemIdEnum itemId)
    {
        switch (itemId)
        {
            case ItemIdEnum.TEST_NULL_ITEM:
                return 4;
            default:
                return -1;
        }
    }
}

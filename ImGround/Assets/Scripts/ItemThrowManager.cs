using UnityEngine;

public class ItemThrowManager
{
    /// <summary>
    /// 아이템 내보내기를 요청할 때 발생하는 이벤트입니다.
    /// </summary>
    /// <param name="itemBundle"></param>
    public delegate void onStartItemThrow(GameObject itemObject);
    private static onStartItemThrow onStartItemThrowHandler;

    /// <summary>
    /// 아이템 내보내기 이벤트에 연결합니다.
    /// </summary>
    public static void listenItemThrowEvent(onStartItemThrow handler)
    {
        onStartItemThrowHandler += handler;
    }

    /// <summary>
    /// 아이템 내보내기를 요청합니다.
    /// </summary>
    /// <param name="itemBundle"></param>
    public static void throwItem(ItemBundle itemBundle)
    {
        onStartItemThrowHandler?.Invoke(ItemPrefabSO.getItemPrefab(itemBundle).gameObject);
    }
}

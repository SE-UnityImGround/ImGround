using UnityEngine;

/// <summary>
/// 이 클래스는 ScriptableObject로, 에셋으로 생성하여 사용해야 합니다!
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/ItemPrefabSO")]
public class ItemPrefabSO : ScriptableObject
{
    /*==================================================
     *                 프리팹 데이터 
     *==================================================*/

    public ItemPrefabID defaultNullItem;
    public ItemPrefabID default2DItem;
    public ItemPrefabID defaultPackageItem;
    public ItemPrefabID[] itemPrefabs;

    /*==================================================
     *                 Static 접근자 
     *==================================================*/

    private ItemPrefabSO()
    {

    }

    private static ItemPrefabSO instance = null;

    private static ItemPrefabSO getInstance()
    {
        if (instance == null)
        {
            instance = Resources.Load<ItemPrefabSO>("ItemPrefabSO/ItemPrefabSO");
            if (instance == null)
            {
                Debug.LogError("Resources 폴더에 " + nameof(ItemPrefabSO) + " ScriptableObject에 대한 에셋이 등록되지 않았습니다!");
            }
        }
        return instance;
    }

    /*==================================================
     *                   Util 메소드 
     *==================================================*/

    private ItemPrefabID findPrefabById(ItemIdEnum itemId)
    {
        foreach (ItemPrefabID item in itemPrefabs)
        {
            if (item.itemType == itemId)
            {
                return Instantiate(item.gameObject).GetComponent<ItemPrefabID>();
            }
        }

        // 없을시 기본 지정 프리팹으로
        if (itemId == ItemIdEnum.TEST_NULL_ITEM)
        {
            return Instantiate(defaultNullItem.gameObject).GetComponent<ItemPrefabID>();
        }
        if (itemId == ItemIdEnum.PACKAGE)
        {
            return Instantiate(defaultPackageItem.gameObject).GetComponent<ItemPrefabID>();
        }
        return Instantiate(default2DItem.gameObject).GetComponent<ItemPrefabID>();
    }

    /*==================================================
     *                외부 지원 메소드
     *==================================================*/

    public static ItemPrefabID getItemPrefab(ItemBundle item)
    {
        ItemPrefabID itemPrefab = getInstance().findPrefabById(item.item.itemId);
        itemPrefab.setItemData(item);
        return itemPrefab;
    }
}

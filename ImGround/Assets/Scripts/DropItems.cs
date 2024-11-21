using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/BasicItem")]
public class DropItems : ScriptableObject
{
    public string itemName;           // 아이템 이름
    public GameObject prefab;         // 아이템 프리팹
}
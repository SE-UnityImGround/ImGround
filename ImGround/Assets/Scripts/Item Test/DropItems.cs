using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/BasicItem")]
public class DropItems : ScriptableObject
{
    public string itemName;           // ������ �̸�
    public GameObject prefab;         // ������ ������
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private InventoryBehavior inventoryUI;

    private static InventoryManager instance = null;
    public static InventoryManager getInstance()
    {
        if (instance == null)
        {
            throw new System.Exception(nameof(InventoryManager) + "�� ���� ���� ������ �ʾҽ��ϴ�!");
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception(nameof(InventoryManager) + "�� �ϳ��� �����ؾ��մϴ�!");
        }
        instance = this;

        if (inventoryUI == null)
        {
            throw new System.Exception("�κ��丮 UI�� ��ϵ��� �ʾҽ��ϴ�!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �κ��丮�� Ư�� �������� � �ִ����� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public int getItemAmount(ItemIdEnum itemType)
    {
        return inventoryUI.getItemAmount(itemType);
    }

    /// <summary>
    /// ���� �κ��丮���� ���õ� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public ItemBundle getSelectedItem()
    {
        return inventoryUI.getSelectedItem();
    }

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, �� �� �̻��� �������� �߰��Ǹ� true�� ��ȯ�մϴ�.
    /// <br/>���� : �������� �߰��� �� ���� ������ �Էµ� item ��ü�� �����ֽ��ϴ�.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        return inventoryUI.addItem(item);
    }
}

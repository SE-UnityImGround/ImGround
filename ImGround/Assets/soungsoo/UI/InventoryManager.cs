using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private InventoryBehavior inventoryUI;
    [SerializeField]
    private QuestListBehavior questUI;

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryUI == null)
        {
            throw new System.Exception("�κ��丮 UI�� ��ϵ��� �ʾҽ��ϴ�!");
        }
        if (questUI == null)
        {
            throw new System.Exception("����Ʈ UI�� ��ϵ��� �ʾҽ��ϴ�!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ���� �κ��丮���� ���õ� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public Item getSelectedItem()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBehavior : MonoBehaviour
{
    public GameObject slotPrefab = null;
    public GameObject SlotList = null;

    private Inventory inventory = new Inventory(20);

    // Start is called before the first frame update
    void Start()
    {
        if (slotPrefab == null)
        {
            Debug.LogErrorFormat("{0}�� ���� ������ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(InventoryBehavior), nameof(slotPrefab));
            return;
        }

        for (int i = 1; i <= inventory.size; i++)
        {
            SlotBehavior newSlotScript = Instantiate(slotPrefab, SlotList.transform).GetComponent<SlotBehavior>();
            newSlotScript.initialize(this, inventory.slots[i - 1], i);
        }

        setActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �κ��丮�� ǥ��/������ �����մϴ�.
    /// </summary>
    /// <param name="isActive"></param>
    public void setActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// ���� �κ��丮�� ǥ�õǴ����� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>�κ��丮�� ǥ�����̸� true, �׷��� ������ false�� ��ȯ�մϴ�.</returns>
    public bool getActive()
    {
        return gameObject.activeSelf;
    }

    /// <summary>
    /// ���� Ŭ�� �߻��� �׽�Ʈ�ϴ� �����Դϴ�.
    /// </summary>
    /// <param name="idx"></param>
    public void testSlotClicked(int slotIdx)
    {
        Debug.Log(slotIdx);
        Debug.Log(inventory.slots[slotIdx-1].item.name);
    }

    /// <summary>
    /// ������ �Է��� �׽�Ʈ�ϴ� �����Դϴ�.
    /// </summary>
    public void testAddItem()
    {
        Debug.Log(inventory.addItem(new Item(ItemIdEnum.TEST_NULL_ITEM, 2)));
    }
}

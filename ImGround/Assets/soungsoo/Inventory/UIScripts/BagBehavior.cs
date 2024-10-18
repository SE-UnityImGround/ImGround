using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagBehavior : MonoBehaviour
{
    public GameObject slotPrefab = null;

    private Inventory inventory = new Inventory(20);

    // Start is called before the first frame update
    void Start()
    {
        if (slotPrefab == null)
        {
            Debug.LogErrorFormat("{0}�� ���� ������ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(BagBehavior), nameof(slotPrefab));
            return;
        }

        for (int i = 1; i <= inventory.size; i++)
        {
            SlotBehavior newSlotScript = Instantiate(slotPrefab, transform).GetComponent<SlotBehavior>();
            newSlotScript.initialize(this, inventory.slots[i - 1], i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

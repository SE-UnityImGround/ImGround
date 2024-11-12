using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������ �κ��丮 UI�� ��Ʈ���� ����ϴ� ��Ʈ�� Ŭ�����Դϴ�.
/// </summary>
public class InventoryBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab = null;

    [SerializeField]
    private GameObject SlotList = null;

    private Inventory inventory = new Inventory(20);

    private ItemBundle selectedItem = null;

    // Start is called before the first frame update
    void Start()
    {
        if (slotPrefab == null)
        {
            Debug.LogErrorFormat("{0}�� ���� ������ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(InventoryBehavior), nameof(slotPrefab));
            return;
        }

        generateSlots();
        setActive(false);
    }

    private void generateSlots()
    {
        for (int slotNum = 1; slotNum <= inventory.size; slotNum++)
        {
            SlotBehavior newSlotScript = Instantiate(slotPrefab, SlotList.transform).GetComponent<SlotBehavior>();
            newSlotScript.initialize(inventory.slots[slotNum - 1]);
            newSlotScript.itemSelectedEventHandler += onItemSelected;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*========================================================
     *                     �̺�Ʈ ó��
     *========================================================*/

    private void onItemSelected(ItemBundle selection)
    {
        this.selectedItem = selection;
    }

    /*========================================================
     *                     �ܺ� ���� �޼ҵ�
     *========================================================*/

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
    /// ���� ���õ� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public ItemBundle getSelectedItem()
    {
        return selectedItem;
    }

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, �� �� �̻��� �������� �߰��Ǹ� true�� ��ȯ�մϴ�.
    /// <br/>�������� �߰��� �� ���� ������ �Էµ� item ��ü�� �ݿ��˴ϴ�.
    /// </summary>
    /// <param name="item">�κ��丮�� �߰��� ������</param>
    /// <returns></returns>
    public bool addItem(Item item)
    {
        return inventory.addItem(new ItemBundle(item, 1, true));
    }

    /// <summary>
    /// �������� �κ��丮 �� ������ �߰��Ϸ��� �õ��ϸ�, �� �� �̻��� �������� �߰��Ǹ� true�� ��ȯ�մϴ�.
    /// <br/>�������� �߰��� �� ���� ������ �Էµ� items�� �� �����ۿ� �ݿ��˴ϴ�.
    /// </summary>
    /// <param name="items">�κ��丮�� �߰��� �����۵�</param>
    /// <returns></returns>
    public bool addItems(Item[] items)
    {
        bool result = false;
        foreach (Item item in items)
        {
            result = result || inventory.addItem(new ItemBundle(item, 1, true));
        }
        return result;
    }
}

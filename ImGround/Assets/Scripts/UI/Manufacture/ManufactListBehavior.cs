using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufactListBehavior : UIBehavior
{
    [SerializeField]
    private GameObject ManufactListView;
    [SerializeField]
    private GameObject ManufactPrefab;

    private List<ManufactBehavior> manufactItems = new List<ManufactBehavior>();
    [SerializeField]
    private TabItemBehavior[] tabButtons;
    
    private Action onClose;

    public override void initialize()
    {
        if (ManufactListView == null)
        {
            Debug.LogErrorFormat("{0}�� ����Ʈ ������Ʈ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(ManufactListBehavior), nameof(ManufactListView));
            return;
        }
        if (ManufactPrefab == null)
        {
            Debug.LogErrorFormat("{0}�� ���� ������ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(ManufactListBehavior), nameof(ManufactPrefab));
            return;
        }

        InventoryManager.onSlotItemChangedHandler += onInventoryUpdated;
    }

    /// <summary>
    /// ���� UI�� �����մϴ�. (ǥ������ ����) ����ȭ�� ����� ����Ǿ�� �� �޼ҵ带 �Բ� �Է¹޽��ϴ�.
    /// </summary>
    public void setManufact(Action onCloseCallBack)
    {
        this.onClose = onCloseCallBack;
        setViewByCategory(ManufactCategory.SIMPLE_FOOD);
    }

    /// <summary>
    /// ���� â�� ī�װ��� �����մϴ�.
    /// </summary>
    /// <param name="category"></param>
    private void setViewByCategory(ManufactCategory category)
    {
        foreach (TabItemBehavior tab in tabButtons)
        {
            tab.updateSelection(category);
        }

        foreach (ManufactBehavior man in manufactItems)
        {
            Destroy(man.gameObject);
        }
        manufactItems.Clear();
        Dictionary<ItemIdEnum, int> invenInfo = InventoryManager.getInventoryInfo();
        foreach (ManufactInfo info in ManufactInfoManager.getManufactInfo(category))
        {
            addManufactItem(info, invenInfo);
        }
    }

    /// <summary>
    /// �κ��丮 ������Ʈ���� ���� UI�� ������ ��Ȳ�� ������Ʈ�ϴ� �̺�Ʈ ó�����Դϴ�.
    /// </summary>
    /// <param name="slotIdx"></param>
    private void onInventoryUpdated(int slotIdx)
    {
        Dictionary<ItemIdEnum, int> invenInfo = InventoryManager.getInventoryInfo();
        foreach (ManufactBehavior man in manufactItems)
        {
            man.updateInfo(invenInfo);
        }
    }

    /// <summary>
    /// ���� �׸��� �߰��մϴ�. <paramref name="manufactInfo"/>�� ���� �׸��� ���������Դϴ�. 
    /// <br/> <paramref name="inventoryInfo"/>�� �κ��丮 �����Դϴ�. null�� �κ��丮�� ������� ǥ���մϴ�.
    /// </summary>
    /// <param name="manufactInfo"></param>
    /// <param name="inventoryInfo"></param>
    public void addManufactItem(ManufactInfo manufactInfo, Dictionary<ItemIdEnum, int> inventoryInfo)
    {
        ManufactBehavior man = Instantiate(ManufactPrefab, ManufactListView.transform).GetComponent<ManufactBehavior>();
        if (man == null)
        {
            Debug.LogError("������ " + nameof(ManufactPrefab) + "���� " + nameof(ManufactBehavior) + "�� ã�� �� �����ϴ�!");
        }
        man.initialize(manufactInfo);
        man.updateInfo(inventoryInfo);
        man.doMakeClickHandler += startMake;
        manufactItems.Add(man);
    }

    /// <summary>
    /// �� ��ư�� Ŭ���Ǹ� ó���� �̺�Ʈ�Դϴ�.
    /// </summary>
    /// <param name="category"></param>
    public void onTabButtonClick(ManufactCategory category)
    {
        setViewByCategory(category);
    }

    /// <summary>
    /// UI�� ���� ����� ����� ���۵� ��� ó���ϴ� �̺�Ʈ ó�����Դϴ�.
    /// </summary>
    /// <param name="manufactInfo"></param>
    private void startMake(ManufactInfo manufactInfo)
    {
        Dictionary<ItemIdEnum, int> invenInfo = InventoryManager.getInventoryInfo();

        bool canMake = true;
        foreach (ItemBundle inputItem in manufactInfo.inputItems)
        {
            if (!invenInfo.ContainsKey(inputItem.item.itemId)
                || invenInfo[inputItem.item.itemId] < inputItem.count)
            {
                canMake = false;
                break;
            }
        }

        if (canMake)
        {
            foreach (ItemBundle inputItem in manufactInfo.inputItems)
            {
                InventoryManager.removeItem(inputItem.item.itemId, inputItem.count);
            }
            InventoryManager.addItems(new ItemBundle(manufactInfo.outputItem));
            Debug.Log("���� �õ� : ����!");
        }
        else
        {
            Debug.Log("���� �õ� : ����! ������ �����ؿ�");
        }
    }

    /// <summary>
    /// â �ݱ� �������� �߰� ó���� ���� �������̵� �� �Լ��Դϴ�.
    /// </summary>
    /// <param name="isActive"></param>
    public override void setActive(bool isActive)
    {
        if (!isActive)
        {
            Action closeFunc = onClose;
            onClose = null;
            closeFunc?.Invoke();
        }
        base.setActive(isActive);
    }
}

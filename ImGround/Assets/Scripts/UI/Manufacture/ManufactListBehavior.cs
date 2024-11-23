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

    private Action onClose;

    public override void initialize()
    {
        if (ManufactListView == null)
        {
            Debug.LogErrorFormat("{0}에 리스트 오브젝트 {1}가 입력되지 않았습니다!", nameof(ManufactListBehavior), nameof(ManufactListView));
            return;
        }
        if (ManufactPrefab == null)
        {
            Debug.LogErrorFormat("{0}에 제작 프리팹 {1}가 입력되지 않았습니다!", nameof(ManufactListBehavior), nameof(ManufactPrefab));
            return;
        }

        InventoryManager.onSlotItemChangedHandler += onInventoryUpdated;

        Dictionary<ItemIdEnum, int> invenInfo = InventoryManager.getInventoryInfo();
        foreach (ManufactInfo info in ManufactInfoManager.manufactInfos)
        {
            addManufactItem(info, invenInfo);
        }
    }

    /// <summary>
    /// 제작 UI를 설정합니다. (표시하진 않음) 제작화면 종료시 실행되어야 할 메소드를 함께 입력받습니다.
    /// </summary>
    public void setManufact(Action onCloseCallBack)
    {
        this.onClose = onCloseCallBack;
    }

    /// <summary>
    /// 인벤토리 업데이트마다 제작 UI의 아이템 현황을 업데이트하는 이벤트 처리기입니다.
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
    /// 제작 항목을 추가합니다. <paramref name="manufactInfo"/>는 제작 항목의 조합정보입니다. 
    /// <br/> <paramref name="inventoryInfo"/>는 인벤토리 정보입니다. null로 인벤토리가 비었음을 표시합니다.
    /// </summary>
    /// <param name="manufactInfo"></param>
    /// <param name="inventoryInfo"></param>
    public void addManufactItem(ManufactInfo manufactInfo, Dictionary<ItemIdEnum, int> inventoryInfo)
    {
        ManufactBehavior man = Instantiate(ManufactPrefab, ManufactListView.transform).GetComponent<ManufactBehavior>();
        if (man == null)
        {
            Debug.LogError("프리팹 " + nameof(ManufactPrefab) + "에서 " + nameof(ManufactBehavior) + "을 찾을 수 없습니다!");
        }
        man.initialize(manufactInfo);
        man.updateInfo(inventoryInfo);
        man.doMakeClickHandler += startMake;
        manufactItems.Add(man);
    }

    /// <summary>
    /// UI에 의해 만들기 기능이 시작될 경우 처리하는 이벤트 처리기입니다.
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
            ItemBundle newItem = new ItemBundle(manufactInfo.outputItem);
            InventoryManager.addItems(newItem);
            if (newItem.count > 0)
                ItemThrowManager.throwItem(newItem);
            Debug.Log("제작 시도 : 성공!");
        }
        else
        {
            Debug.Log("제작 시도 : 실패! 아이템 부족해요");
        }
    }

    /// <summary>
    /// 창 닫기 과정에서 추가 처리를 위해 오버라이딩 된 함수입니다.
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

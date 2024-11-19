using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufactListBehavior : UIBehavior
{
    [SerializeField]
    private GameObject ManufactListView;
    [SerializeField]
    private GameObject ManufactPrefab;

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

        test();
    }

    void test()
    {
        foreach (ManufactInfo info in ManufactInfoManager.manufactInfos)
        {
            addManufactItem(info);
        }
    }

    public void addManufactItem(ManufactInfo manufactInfo)
    {
        ManufactBehavior man = Instantiate(ManufactPrefab, ManufactListView.transform).GetComponent<ManufactBehavior>();
        if (man == null)
        {
            Debug.LogError("프리팹 " + nameof(ManufactPrefab) + "에서 " + nameof(ManufactBehavior) + "을 찾을 수 없습니다!");
        }
        man.initialize(manufactInfo);
        man.doMakeClickHandler += startMake;
    }

    private void startMake(ManufactInfo manufactInfo)
    {
        string input = "";
        foreach (ItemBundle item in manufactInfo.inputItems)
            input += item.item.itemId.ToString() + " " + item.count + "\n";
        Debug.Log("input : \n" + input + "\n output : \n" + manufactInfo.outputItem.item.itemId.ToString() + " " + manufactInfo.outputItem.count);
    }
}

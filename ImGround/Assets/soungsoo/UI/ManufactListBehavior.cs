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
            Debug.LogErrorFormat("{0}�� ����Ʈ ������Ʈ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(ManufactListBehavior), nameof(ManufactListView));
            return;
        }
        if (ManufactPrefab == null)
        {
            Debug.LogErrorFormat("{0}�� ���� ������ {1}�� �Էµ��� �ʾҽ��ϴ�!", nameof(ManufactListBehavior), nameof(ManufactPrefab));
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
            Debug.LogError("������ " + nameof(ManufactPrefab) + "���� " + nameof(ManufactBehavior) + "�� ã�� �� �����ϴ�!");
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

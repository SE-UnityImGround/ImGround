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
        for (int i = 0; i < 10; i++)
        {
            addManufactItem(
                new ItemBundle(ItemIdEnum.MILK_PACK, 8, false),
                new ItemBundle(ItemIdEnum.CHEESE, 2, false));

            addManufactItem(
                new ItemBundle(ItemIdEnum.GOLD_INGOT, 8, false),
                new ItemBundle(ItemIdEnum.GOLD_NECKLACE, 6, false));

            addManufactItem(
                new ItemBundle(ItemIdEnum.HORSE_LEATHER, 8, true),
                new ItemBundle(ItemIdEnum.BANANA_MILK, 6, true));
        }
    }

    public void addManufactItem(ItemBundle inItem, ItemBundle outItem)
    {
        ManufactBehavior man = Instantiate(ManufactPrefab, ManufactListView.transform).GetComponent<ManufactBehavior>();
        if (man == null)
        {
            Debug.LogError("������ " + nameof(ManufactPrefab) + "���� " + nameof(ManufactBehavior) + "�� ã�� �� �����ϴ�!");
        }
        man.initialize(inItem, outItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

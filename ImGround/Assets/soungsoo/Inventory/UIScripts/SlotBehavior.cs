using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBehavior : MonoBehaviour
{
    /// <summary>
    /// �׽�Ʈ �Ű����� : ���� ��üȭ���� �ʾҾ��!
    /// </summary>
    private GameObject itemImg;

    private BagBehavior backpack;
    private int slotIdx;

    /// <summary>
    /// ���� ��ü ��Ͻ� �ʱ�ȭ!
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="slotIdx"></param>
    public void initialize(BagBehavior parent, Slot mySlot, int slotIdx)
    {
        this.backpack = parent;
        this.slotIdx = slotIdx;
        mySlot.itemUpdatedEventHandler += itemUpdated;

        // test �׽�Ʈ��
        itemImg = transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// ���ӿ�����Ʈ ������ �ʱ�ȭ(Unity ���� �ʱ�ȭ)
    /// (Start is called before the first frame update)
    /// </summary>
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �� ������ Ŭ�� �̺�Ʈ�� �޴� ó�����Դϴ�.
    /// </summary>
    private void onClick()
    {
        backpack.testSlotClicked(slotIdx);
    }

    /// <summary>
    /// �� ������ ������ ���� �̺�Ʈ�� �޴� ó�����Դϴ�.
    /// </summary>
    /// <param name="updatedItem"></param>
    private void itemUpdated(Item updatedItem)
    {
        testItemUpdated(updatedItem);
    }

    /// <summary>
    /// ������ ������Ʈ ó���� ���� �׽�Ʈ �Լ��Դϴ�.
    /// </summary>
    /// <param name="i"></param>
    private void testItemUpdated(Item i)
    {
        if (i != null) {
            itemImg.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBehavior : MonoBehaviour
{
    private InventoryBehavior backpack;
    private int slotIdx;
    private Image itemImg;

    /// <summary>
    /// ���� ��ü ��Ͻ� �ʱ�ȭ!
    /// </summary>
    /// <param name="inventoryUI"></param>
    /// <param name="slotIdx"></param>
    public void initialize(InventoryBehavior inventoryUI, Slot mySlot, int slotIdx)
    {
        this.backpack = inventoryUI;
        this.slotIdx = slotIdx;
        mySlot.itemUpdatedEventHandler += itemUpdated;
        itemImg = transform.GetChild(0).gameObject.GetComponent<Image>();
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
        setImage(updatedItem);
    }

    /*=======================================================
     *                    ���� ó�� �޼ҵ�
     *=======================================================*/

    private void setImage(Item i)
    {
        if (i == null)
        {
            itemImg.sprite = null;
            itemImg.color = new Color(255, 255, 255, 0);
        }
        else
        {
            itemImg.color = new Color(255, 255, 255, 255);
            if (itemImg.sprite != ImagesSO.getImage(i.itemId))
            {
                itemImg.sprite = ImagesSO.getImage(i.itemId);
            }
        }
    }
}

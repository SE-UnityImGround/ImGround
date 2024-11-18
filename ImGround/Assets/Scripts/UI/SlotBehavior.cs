using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������ ���� UI�� ��Ʈ���� ����ϴ� ��Ʈ�� Ŭ�����Դϴ�.
/// </summary>
public class SlotBehavior : MonoBehaviour
{
    private Slot mySlot;
    private Image itemImg;

    /// <summary>
    /// ������ ���õ� ��� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    /// <param name="selectedItem">���õ� ������ ������</param>
    public delegate void ItemSelectedEvent(ItemBundle selectedItem);

    public ItemSelectedEvent itemSelectedEventHandler;

    /// <summary>
    /// ���� ��ü ��Ͻ� �ʱ�ȭ!
    /// </summary>
    /// <param name="mySlot"></param>
    public void initialize(Slot mySlot)
    {
        this.mySlot = mySlot;
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
        itemSelectedEventHandler?.Invoke(this.mySlot.bundle);
    }

    /// <summary>
    /// �� ������ ������ ���� �̺�Ʈ�� �޴� ó�����Դϴ�.
    /// </summary>
    /// <param name="updatedItem"></param>
    private void itemUpdated(ItemBundle updatedItem)
    {
        setImage(updatedItem.item);
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
            Sprite itemImage = ItemInfoManager.getItemImage(i.itemId);
            if (itemImg.sprite != itemImage)
            {
                itemImg.sprite = itemImage;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������ ���� UI�� ��Ʈ���� ����ϴ� ��Ʈ�� Ŭ�����Դϴ�.
/// </summary>
public class SlotBehavior : MonoBehaviour
{
    private int slotIdx;
    [SerializeField]
    private Image itemImg;
    [SerializeField]
    private TMPro.TMP_Text amountText;

    /// <summary>
    /// ������ ���õ� ��� �߻��ϴ� �̺�Ʈ�Դϴ�.
    /// </summary>
    /// <param name="slotIdx">���õ� ������ ��ȣ</param>
    public delegate void SlotSelectedEvent(int slotIdx);

    public SlotSelectedEvent slotSelectedEventHandler;

    /// <summary>
    /// ���� ��ü ��Ͻ� �ʱ�ȭ!
    /// </summary>
    /// <param name="mySlot"></param>
    public void initialize(int slotIdx)
    {
        this.slotIdx = slotIdx;
        amountText.text = "";
    }

    /// <summary>
    /// �� ������ Ŭ�� �̺�Ʈ�� �޴� ó�����Դϴ�.
    /// </summary>
    public void onClick()
    {
        slotSelectedEventHandler?.Invoke(slotIdx);
    }

    /// <summary>
    /// �� ������ ������ ������ UI�� �����մϴ�.
    /// <br/> ������ ������ 0 �����̸� ǥ����� �ʽ��ϴ�.
    /// </summary>
    /// <param name="updatedItem"></param>
    public void updateItemInfo(ItemIdEnum itemId, int amount)
    {
        setImage(itemId);
        amountText.text = (amount > 0 ? amount.ToString() : "");
    }

    /*=======================================================
     *                    ���� ó�� �޼ҵ�
     *=======================================================*/

    private void setImage(ItemIdEnum i)
    {
        if (i == ItemIdEnum.TEST_NULL_ITEM)
        {
            itemImg.sprite = null;
            itemImg.color = new Color(255, 255, 255, 0);
        }
        else
        {
            itemImg.color = new Color(255, 255, 255, 255);
            Sprite itemImage = ItemInfoManager.getItemImage(i);
            if (itemImg.sprite != itemImage)
            {
                itemImg.sprite = itemImage;
            }
        }
    }
}

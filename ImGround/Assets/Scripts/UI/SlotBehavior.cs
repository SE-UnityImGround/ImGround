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
    private Image itemImg;

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
        this.itemImg = transform.GetChild(0).gameObject.GetComponent<Image>();
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
        slotSelectedEventHandler?.Invoke(slotIdx);
    }

    /// <summary>
    /// �� ������ ������ ���� �̺�Ʈ�� �޴� ó�����Դϴ�.
    /// </summary>
    /// <param name="updatedItem"></param>
    public void itemUpdated(ItemIdEnum itemId)
    {
        setImage(itemId);
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

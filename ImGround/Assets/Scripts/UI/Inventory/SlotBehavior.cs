using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 내에서 슬롯 UI의 컨트롤을 담당하는 컨트롤 클래스입니다.
/// </summary>
public class SlotBehavior : MonoBehaviour
{
    private int slotIdx;
    [SerializeField]
    private Image itemImg;
    [SerializeField]
    private TMPro.TMP_Text amountText;

    /// <summary>
    /// 슬롯이 선택된 경우 발생하는 이벤트입니다.
    /// </summary>
    /// <param name="slotIdx">선택된 슬롯의 번호</param>
    public delegate void SlotSelectedEvent(int slotIdx);

    public SlotSelectedEvent slotSelectedEventHandler;

    /// <summary>
    /// 슬롯 객체 등록시 초기화!
    /// </summary>
    /// <param name="mySlot"></param>
    public void initialize(int slotIdx)
    {
        this.slotIdx = slotIdx;
        amountText.text = "";
    }

    /// <summary>
    /// 이 슬롯의 클릭 이벤트를 받는 처리기입니다.
    /// </summary>
    public void onClick()
    {
        slotSelectedEventHandler?.Invoke(slotIdx);
    }

    /// <summary>
    /// 이 슬롯의 아이템 변동을 UI에 적용합니다.
    /// <br/> 아이템 수량이 0 이하이면 표기되지 않습니다.
    /// </summary>
    /// <param name="updatedItem"></param>
    public void updateItemInfo(ItemIdEnum itemId, int amount)
    {
        setImage(itemId);
        amountText.text = (amount > 0 ? amount.ToString() : "");
    }

    /*=======================================================
     *                    내부 처리 메소드
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

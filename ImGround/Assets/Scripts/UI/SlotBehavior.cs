using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 내에서 슬롯 UI의 컨트롤을 담당하는 컨트롤 클래스입니다.
/// </summary>
public class SlotBehavior : MonoBehaviour
{
    private Slot mySlot;
    private Image itemImg;

    /// <summary>
    /// 슬롯이 선택된 경우 발생하는 이벤트입니다.
    /// </summary>
    /// <param name="selectedItem">선택된 슬롯의 아이템</param>
    public delegate void ItemSelectedEvent(ItemBundle selectedItem);

    public ItemSelectedEvent itemSelectedEventHandler;

    /// <summary>
    /// 슬롯 객체 등록시 초기화!
    /// </summary>
    /// <param name="mySlot"></param>
    public void initialize(Slot mySlot)
    {
        this.mySlot = mySlot;
        mySlot.itemUpdatedEventHandler += itemUpdated;
        itemImg = transform.GetChild(0).gameObject.GetComponent<Image>();

    }

    /// <summary>
    /// 게임오브젝트 생성시 초기화(Unity 엔진 초기화)
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
    /// 이 슬롯의 클릭 이벤트를 받는 처리기입니다.
    /// </summary>
    private void onClick()
    {
        itemSelectedEventHandler?.Invoke(this.mySlot.bundle);
    }

    /// <summary>
    /// 이 슬롯의 아이템 변동 이벤트를 받는 처리기입니다.
    /// </summary>
    /// <param name="updatedItem"></param>
    private void itemUpdated(ItemBundle updatedItem)
    {
        setImage(updatedItem.item);
    }

    /*=======================================================
     *                    내부 처리 메소드
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

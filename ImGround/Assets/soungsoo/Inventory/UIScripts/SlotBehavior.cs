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
    /// 슬롯 객체 등록시 초기화!
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
        backpack.testSlotClicked(slotIdx);
    }

    /// <summary>
    /// 이 슬롯의 아이템 변동 이벤트를 받는 처리기입니다.
    /// </summary>
    /// <param name="updatedItem"></param>
    private void itemUpdated(Item updatedItem)
    {
        setImage(updatedItem);
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
            if (itemImg.sprite != ImagesSO.getImage(i.itemId))
            {
                itemImg.sprite = ImagesSO.getImage(i.itemId);
            }
        }
    }
}

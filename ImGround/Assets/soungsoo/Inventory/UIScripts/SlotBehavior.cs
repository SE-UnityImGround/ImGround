using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBehavior : MonoBehaviour
{
    /// <summary>
    /// 테스트 매개변수 : 아직 구체화되지 않았어요!
    /// </summary>
    private GameObject itemImg;

    private BagBehavior backpack;
    private int slotIdx;

    /// <summary>
    /// 슬롯 객체 등록시 초기화!
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="slotIdx"></param>
    public void initialize(BagBehavior parent, Slot mySlot, int slotIdx)
    {
        this.backpack = parent;
        this.slotIdx = slotIdx;
        mySlot.itemUpdatedEventHandler += itemUpdated;

        // test 테스트용
        itemImg = transform.GetChild(0).gameObject;
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
        testItemUpdated(updatedItem);
    }

    /// <summary>
    /// 아이템 업데이트 처리에 대한 테스트 함수입니다.
    /// </summary>
    /// <param name="i"></param>
    private void testItemUpdated(Item i)
    {
        if (i != null) {
            itemImg.SetActive(true);
        }
    }
}

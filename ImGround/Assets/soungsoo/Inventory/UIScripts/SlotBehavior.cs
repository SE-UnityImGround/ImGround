using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotBehavior : MonoBehaviour
{
    /// <summary>
    /// ï¿½×½ï¿½Æ® ï¿½Å°ï¿½ï¿½ï¿½ï¿½ï¿½ : ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ã¼È­ï¿½ï¿½ï¿½ï¿½ ï¿½Ê¾Ò¾ï¿½ï¿½!
    /// </summary>
    private GameObject itemImg;

    private BagBehavior backpack;
    private int slotIdx;

    /// <summary>
    /// ½½·Ô °´Ã¼ µî·Ï½Ã ÃÊ±âÈ­!
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="slotIdx"></param>
    public void initialize(BagBehavior parent, Slot mySlot, int slotIdx)
    {
        this.backpack = parent;
        this.slotIdx = slotIdx;
        mySlot.itemUpdatedEventHandler += itemUpdated;

        // test ï¿½×½ï¿½Æ®ï¿½ï¿½
        itemImg = transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// °ÔÀÓ¿ÀºêÁ§Æ® »ý¼º½Ã ÃÊ±âÈ­(Unity ¿£Áø ÃÊ±âÈ­)
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
    /// ÀÌ ½½·ÔÀÇ Å¬¸¯ ÀÌº¥Æ®¸¦ ¹Þ´Â Ã³¸®±âÀÔ´Ï´Ù.
    /// </summary>
    private void onClick()
    {
        backpack.testSlotClicked(slotIdx);
    }

    /// <summary>
    /// ÀÌ ½½·ÔÀÇ ¾ÆÀÌÅÛ º¯µ¿ ÀÌº¥Æ®¸¦ ¹Þ´Â Ã³¸®±âÀÔ´Ï´Ù.
    /// </summary>
    /// <param name="updatedItem"></param>
    private void itemUpdated(Item updatedItem)
    {
        setImage(updatedItem);
    }

    /*=======================================================
     *                    ³»ºÎ Ã³¸® ¸Þ¼Òµå
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

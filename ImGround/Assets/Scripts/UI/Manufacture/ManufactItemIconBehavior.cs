using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManufactItemIconBehavior : MonoBehaviour
{
    [SerializeField]
    private Image InItemImg;
    [SerializeField]
    private TMPro.TMP_Text InItemAmount;
    [SerializeField]
    private TMPro.TMP_Text InItemName;

    private Item item;
    private int needAmount;
    private bool doCheck;

    private void checkValue(object v, string name)
    {
        if (v == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
            return;
        }
    }

    public void initialize(ItemBundle itemBundle, bool checkAmount)
    {
        checkValue(InItemImg, nameof(InItemImg));
        checkValue(InItemAmount, nameof(InItemAmount));
        checkValue(InItemName, nameof(InItemName));

        this.item = itemBundle.item;
        this.needAmount = itemBundle.count;
        this.doCheck = checkAmount;

        InItemImg.sprite = itemBundle.item.image;
        InItemName.text = itemBundle.item.name;
        updateAmount(0);
    }

    public ItemIdEnum getItemId()
    {
        return item.itemId;
    }

    /// <summary>
    /// 현재 가진 아이템 수량을 업데이트합니다. 음수는 0으로 처리합니다.
    /// </summary>
    /// <param name="amount"></param>
    public void updateAmount(int amount)
    {
        if (amount < 0)
        {
            amount = 0;
        }

        if (doCheck)
        {
            InItemAmount.text = amount + "/" + needAmount;
            if (amount >= needAmount)
                InItemAmount.color = Color.white;
            else
                InItemAmount.color = Color.red;
        }
        else
        {
            InItemAmount.text = needAmount.ToString(); ;
            InItemAmount.color = Color.white;
        }
    }
}

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

    private void checkValue(object v, string name)
    {
        if (v == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
            return;
        }
    }

    public void initialize(ItemBundle itemBundle)
    {
        checkValue(InItemImg, nameof(InItemImg));
        checkValue(InItemAmount, nameof(InItemAmount));
        checkValue(InItemName, nameof(InItemName));

        InItemImg.sprite = itemBundle.item.image;
        InItemAmount.text = itemBundle.count.ToString();
        InItemName.text = itemBundle.item.name;
    }
}

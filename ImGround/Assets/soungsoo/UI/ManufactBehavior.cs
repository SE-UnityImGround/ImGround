using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManufactBehavior : MonoBehaviour
{
    [SerializeField]
    private Image InItemImg;
    [SerializeField]
    private TMPro.TMP_Text InItemAmount;
    [SerializeField]
    private TMPro.TMP_Text InItemName;

    [SerializeField]
    private Image OutItemImg;
    [SerializeField]
    private TMPro.TMP_Text OutItemAmount;
    [SerializeField]
    private TMPro.TMP_Text OutItemName;

    private void checkValue(object v, string name)
    {
        if (v == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
            return;
        }
    }

    public void initialize(ItemBundle inItemBundle, ItemBundle outItemBundle)
    {
        checkValue(InItemImg, nameof(InItemImg));
        checkValue(InItemAmount, nameof(InItemAmount));
        checkValue(InItemName, nameof(InItemName));
        checkValue(OutItemImg, nameof(OutItemImg));
        checkValue(OutItemAmount, nameof(OutItemAmount));
        checkValue(OutItemName, nameof(OutItemName));

        InItemImg.sprite = inItemBundle.item.image;
        InItemAmount.text = inItemBundle.count.ToString();
        InItemName.text = inItemBundle.item.name;
        OutItemImg.sprite = outItemBundle.item.image;
        OutItemAmount.text = outItemBundle.count.ToString();
        OutItemName.text = outItemBundle.item.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

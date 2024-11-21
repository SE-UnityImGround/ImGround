using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    Vector3 defaultScale;

    private void Start()
    {
        defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = defaultScale*1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
    }
}

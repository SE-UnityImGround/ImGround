using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpDisplayer : MonoBehaviour
{
    [SerializeField]
    private Text expText;
    [SerializeField]
    private Slider expSlider;

    public void initialize()
    {
        if (expText == null)
        {
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, nameof(expText));
            return;
        }
        if (expSlider == null)
        {
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, nameof(expSlider));
            return;
        }
    }

    public void update(float expRatio, int level)
    {
        expText.text = "Lv. " + level;
        expSlider.value = expRatio;
    }
}

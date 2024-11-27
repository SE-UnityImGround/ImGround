using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBehavior : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;

    private void Start()
    {
        if (healthSlider == null)
        {
            Debug.LogError(nameof(healthSlider) + "가 등록되지 않았습니다!");
        }
    }

    /// <summary>
    /// 체력 비율을 받아 UI에 업데이트합니다.
    /// </summary>
    /// <param name="healthRate"></param>
    public void setHealth(float healthRate)
    {
        healthSlider.value = healthRate;
    }

    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}

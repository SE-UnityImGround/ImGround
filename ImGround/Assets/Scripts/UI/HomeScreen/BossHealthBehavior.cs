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
            Debug.LogError(nameof(healthSlider) + "�� ��ϵ��� �ʾҽ��ϴ�!");
        }
    }

    /// <summary>
    /// ü�� ������ �޾� UI�� ������Ʈ�մϴ�.
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

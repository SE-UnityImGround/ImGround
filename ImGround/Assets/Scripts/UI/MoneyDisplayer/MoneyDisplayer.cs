using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplayer : MonoBehaviour
{
    private const int FILL_SIZE = 1000000;

    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Image fillView;

    void Start()
    {
        if (moneyText == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, nameof(moneyText));
            return;
        }
        if (fillView == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, nameof(fillView));
            return;
        }

        updateValue(InventoryManager.getMoney());
        InventoryManager.onMoneyChangedHandler += updateValue;
    }

    private void updateValue(int money)
    {
        moneyText.text = string.Format("{0:N0}", money);
        fillView.fillAmount = money / (float)FILL_SIZE;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplayer : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text moneyText;

    void Start()
    {
        if (moneyText == null)
        {
            Debug.LogErrorFormat("{0}�� {1}�� ��ϵ��� �ʾҽ��ϴ�!", this.GetType().Name, nameof(moneyText));
            return;
        }

        updateValue(InventoryManager.getMoney());
        InventoryManager.onMoneyChangedHandler += updateValue;
    }

    private void updateValue(int money)
    {
        moneyText.text = string.Format("{0:N0}", money);
    }
}

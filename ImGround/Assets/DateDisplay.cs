using UnityEngine;
using UnityEngine.UI; // UI ���� ���ӽ����̽� �߰�
using System;

public class DateDisplay : MonoBehaviour
{
    public Text dateText; // UI Text ������Ʈ�� ������ ����

    void Start()
    {
        // ���� ��¥�� �ð��� �����Ͽ� ���ڿ��� ��ȯ (�����)
        string currentDate = DateTime.Now.ToString("dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

        // Text ������Ʈ�� ��¥ ���ڿ� ����
        dateText.text = currentDate;
    }
}

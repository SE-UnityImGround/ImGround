using TMPro.Examples;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopUI; // ���� UI�� GameObject�� �Ҵ��� ����
    private bool isPlayerInRange = false; // �÷��̾ ���� �ȿ� �ִ��� Ȯ���� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ������ ���� ��ü�� "Player"���� Ȯ��
        {
            isPlayerInRange = true; // �÷��̾ ������ �������� ǥ��
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // ������ ������ ��ü�� "Player"���� Ȯ��
        {
            isPlayerInRange = false; // �÷��̾ �������� �������� ǥ��
            shopUI.SetActive(false); // ������ ������ UI�� �������� ����
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F)) // ���� ������ FŰ�� �������� Ȯ��
        {
            bool isUIActive = shopUI.activeSelf;
            shopUI.SetActive(!shopUI.activeSelf); // UI�� ����Ͽ� ���� ����
        }
    }

    public void CloseShopUI()
    {
        shopUI.SetActive(false); // ���� UI�� ��Ȱ��ȭ�Ͽ� ����
    }

}

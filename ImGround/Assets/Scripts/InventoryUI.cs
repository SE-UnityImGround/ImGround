using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotContent; // ���Ե��� ����ִ� Content ������Ʈ
    public GameObject slotPrefab; // ���� ������

    private bool activeInventory = false;

    private void Start()
    {
        // ������ ���۵� �� ������ �̸� ����
        GenerateSlots();

        // �κ��丮 UI�� ���� �������� �ʱ� ���¸� ��Ȱ��ȭ
        inventoryPanel.SetActive(false);
        slotContent.SetActive(false);

        Debug.Log("Start���� �κ��丮 �гΰ� ���� ������ ��Ȱ��ȭ��.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // activeInventory ���� �����Ͽ� �κ��丮 ���� �ݱ�
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
            slotContent.SetActive(activeInventory);

            Debug.Log($"I Ű �Էµ�. activeInventory: {activeInventory}, inventoryPanel Ȱ��ȭ ����: {inventoryPanel.activeSelf}");
        }
    }

    private void GenerateSlots()
    {
        // ���ϴ� ���� ����ŭ slotContent�� ������ �߰�
        int slotCount = 20; // ��: 20���� ���� ����
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, slotContent.transform);
        }
        Debug.Log($"���� {slotCount}���� ������.");
    }
}

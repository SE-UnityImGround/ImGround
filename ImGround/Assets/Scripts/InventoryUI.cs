using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotContent; // 슬롯들이 들어있는 Content 오브젝트
    public GameObject slotPrefab; // 슬롯 프리팹

    private bool activeInventory = false;

    private void Start()
    {
        // 게임이 시작될 때 슬롯을 미리 생성
        GenerateSlots();

        // 인벤토리 UI와 슬롯 콘텐츠의 초기 상태를 비활성화
        inventoryPanel.SetActive(false);
        slotContent.SetActive(false);

        Debug.Log("Start에서 인벤토리 패널과 슬롯 콘텐츠 비활성화됨.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // activeInventory 값을 반전하여 인벤토리 열고 닫기
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
            slotContent.SetActive(activeInventory);

            Debug.Log($"I 키 입력됨. activeInventory: {activeInventory}, inventoryPanel 활성화 상태: {inventoryPanel.activeSelf}");
        }
    }

    private void GenerateSlots()
    {
        // 원하는 슬롯 수만큼 slotContent에 슬롯을 추가
        int slotCount = 20; // 예: 20개의 슬롯 생성
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, slotContent.transform);
        }
        Debug.Log($"슬롯 {slotCount}개가 생성됨.");
    }
}

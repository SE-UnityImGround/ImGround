using TMPro.Examples;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopUI; // 상점 UI의 GameObject를 할당할 변수
    private bool isPlayerInRange = false; // 플레이어가 구역 안에 있는지 확인할 변수

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 범위에 들어온 객체가 "Player"인지 확인
        {
            isPlayerInRange = true; // 플레이어가 구역에 들어왔음을 표시
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 범위를 나가는 객체가 "Player"인지 확인
        {
            isPlayerInRange = false; // 플레이어가 구역에서 나갔음을 표시
            shopUI.SetActive(false); // 구역을 나가면 UI가 닫히도록 설정
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F)) // 구역 내에서 F키를 눌렀는지 확인
        {
            bool isUIActive = shopUI.activeSelf;
            shopUI.SetActive(!shopUI.activeSelf); // UI를 토글하여 열고 닫음
        }
    }

    public void CloseShopUI()
    {
        shopUI.SetActive(false); // 상점 UI를 비활성화하여 닫음
    }

}

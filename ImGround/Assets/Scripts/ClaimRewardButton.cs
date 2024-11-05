using UnityEngine;
using UnityEngine.UI;

public class ClaimRewardButton : MonoBehaviour
{
    public int rewardAmount; // 보상 금액 (Inspector에서 설정)
    public Text rewardText; // 보상 표시 텍스트에 대한 참조
    public Button claimButton; // 보상 버튼에 대한 참조

    private bool rewardClaimed = false; // 보상이 이미 수령되었는지 여부

    void Start()
    {
        // 버튼이 클릭될 때 ClaimReward 함수가 호출되도록 설정
        claimButton.onClick.AddListener(ClaimReward);
    }

    void ClaimReward()
    {
        // 보상이 수령되지 않았을 때만 실행
        if (!rewardClaimed)
        {
            // 플레이어에게 보상을 주는 로직 추가
            Debug.Log("보상 수령됨: " + rewardAmount);

            // UI 업데이트 또는 보상 수령 상태 표시
            rewardText.text = "보상 수령 완료!";
            rewardClaimed = true; // 보상 수령 상태를 true로 변경
            claimButton.interactable = false; // 보상을 수령한 후 버튼 비활성화
        }
    }
}

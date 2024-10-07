using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [System.Serializable]
    public class Mission
    {
        public string missionName;
        public bool isCompleted;
        public int rewardAmount;
        public Button claimButton; // 보상 받기 버튼
    }

    public Mission[] missions;

    public void Start()
    {
        // 미션 초기화
        foreach (var mission in missions)
        {
            mission.claimButton.onClick.AddListener(() => ClaimReward(mission));
            UpdateMissionUI(mission);
        }
    }

    void UpdateMissionUI(Mission mission)
    {
        // 미션 UI 업데이트 (예: 버튼 활성화 또는 비활성화)
        mission.claimButton.interactable = mission.isCompleted;
    }

    public void CompleteMission(int missionIndex)
    {
        if (missionIndex < 0 || missionIndex >= missions.Length)
            return;

        missions[missionIndex].isCompleted = true;
        UpdateMissionUI(missions[missionIndex]);
    }

    void ClaimReward(Mission mission)
    {
        if (!mission.isCompleted)
            return;

        // 보상 지급 로직 추가 (예: 플레이어의 자원에 추가)
        Debug.Log($"Reward claimed for mission: {mission.missionName}, Amount: {mission.rewardAmount}");

        // 미션 완료 상태 초기화
        mission.isCompleted = false;
        UpdateMissionUI(mission);
    }
}

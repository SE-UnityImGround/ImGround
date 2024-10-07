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
        public Button claimButton; // ���� �ޱ� ��ư
    }

    public Mission[] missions;

    public void Start()
    {
        // �̼� �ʱ�ȭ
        foreach (var mission in missions)
        {
            mission.claimButton.onClick.AddListener(() => ClaimReward(mission));
            UpdateMissionUI(mission);
        }
    }

    void UpdateMissionUI(Mission mission)
    {
        // �̼� UI ������Ʈ (��: ��ư Ȱ��ȭ �Ǵ� ��Ȱ��ȭ)
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

        // ���� ���� ���� �߰� (��: �÷��̾��� �ڿ��� �߰�)
        Debug.Log($"Reward claimed for mission: {mission.missionName}, Amount: {mission.rewardAmount}");

        // �̼� �Ϸ� ���� �ʱ�ȭ
        mission.isCompleted = false;
        UpdateMissionUI(mission);
    }
}

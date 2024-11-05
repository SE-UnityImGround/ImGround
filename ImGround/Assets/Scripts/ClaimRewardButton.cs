using UnityEngine;
using UnityEngine.UI;

public class ClaimRewardButton : MonoBehaviour
{
    public int rewardAmount; // ���� �ݾ� (Inspector���� ����)
    public Text rewardText; // ���� ǥ�� �ؽ�Ʈ�� ���� ����
    public Button claimButton; // ���� ��ư�� ���� ����

    private bool rewardClaimed = false; // ������ �̹� ���ɵǾ����� ����

    void Start()
    {
        // ��ư�� Ŭ���� �� ClaimReward �Լ��� ȣ��ǵ��� ����
        claimButton.onClick.AddListener(ClaimReward);
    }

    void ClaimReward()
    {
        // ������ ���ɵ��� �ʾ��� ���� ����
        if (!rewardClaimed)
        {
            // �÷��̾�� ������ �ִ� ���� �߰�
            Debug.Log("���� ���ɵ�: " + rewardAmount);

            // UI ������Ʈ �Ǵ� ���� ���� ���� ǥ��
            rewardText.text = "���� ���� �Ϸ�!";
            rewardClaimed = true; // ���� ���� ���¸� true�� ����
            claimButton.interactable = false; // ������ ������ �� ��ư ��Ȱ��ȭ
        }
    }
}

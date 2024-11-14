using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBehavior : MonoBehaviour
{
    public Image UI_QuestImage;
    public Text UI_Description;

    public Image UI_RewardImage;
    public Text UI_RewardAmount;

    public Button UI_Claim;

    public GameObject UI_ProgressDisplay;
    public Slider UI_ProgressSlider;
    public Text UI_ProgressText;

    /// <summary>
    /// 퀘스트 객체 등록시 초기화!
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="slotIdx"></param>
    public void initialize(Quest myQuest)
    {
        UI_QuestImage.sprite = ImageManager.getImage(myQuest.questIconImage);
        UI_Description.text = myQuest.description;

        UI_RewardImage.sprite = ImageManager.getImage(myQuest.rewardIconImage);
        setRewardAmount(myQuest.rewardAmount);

        setProgress(myQuest.progressValue, myQuest.progressSize);
    }

    private void setRewardAmount(int amount)
    {
        UI_RewardAmount.text = "x " + amount;
    }

    private void setProgress(int progress, int fullValue)
    {
        UI_ProgressText.text = progress + " of " + fullValue;

        if (progress == fullValue)
        {
            UI_Claim.gameObject.SetActive(true);
            UI_ProgressDisplay.SetActive(false);
            UI_ProgressSlider.value = progress / (float)fullValue;
        }
        else
        {
            UI_Claim.gameObject.SetActive(false);
            UI_ProgressDisplay.SetActive(true);
            UI_ProgressSlider.value = progress / (float)fullValue;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

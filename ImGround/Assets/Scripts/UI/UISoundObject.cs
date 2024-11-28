using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource MainUiClick;
    [SerializeField]
    private AudioSource InGameUiClick;
    [SerializeField]
    private AudioSource DefaultClick;
    [SerializeField]
    private AudioSource StartTalking;
    [SerializeField]
    private AudioSource Trade;
    [SerializeField]
    private AudioSource RewardQuest;

    public const int MAIN_UI_CLICK = 0;
    public const int INGAME_UI_CLICK = 1;
    public const int DEFAULT_CLICK = 2;
    public const int START_TALKING = 3;
    public const int TRADE = 4;
    public const int QUEST_REWARD = 5;

    void Start()
    {
        UISoundManager.onPlayUiSound += startSound;
    }

    public void startSound(int type)
    {
        switch (type)
        {
            case MAIN_UI_CLICK:
                MainUiClick.Play();
                break;
            case INGAME_UI_CLICK:
                InGameUiClick.Play();
                break;
            case DEFAULT_CLICK:
                DefaultClick.Play();
                break;
            case START_TALKING:
                StartTalking.Play();
                break;
            case TRADE:
                Trade.Play();
                break;
            case QUEST_REWARD:
                RewardQuest.Play();
                break;
            default:
                Debug.LogError("등록되지 않은 타입의 소리를 재생하려 시도했습니다. : ui Sount Type = " + type);
                break;
        }
    }
}

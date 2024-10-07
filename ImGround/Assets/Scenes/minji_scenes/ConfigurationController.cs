using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationController : MonoBehaviour
{
    public Toggle toggleMusic; // 음악 토글
    public Toggle toggleEffects; // 효과음 토글

    void Start()
    {
        // 초기 상태 설정
        toggleMusic.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        toggleEffects.isOn = PlayerPrefs.GetInt("EffectsEnabled", 1) == 1;

        // 이벤트 리스너 추가
        toggleMusic.onValueChanged.AddListener(OnMusicToggleChanged);
        toggleEffects.onValueChanged.AddListener(OnEffectsToggleChanged);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        // 추가적인 음악 관련 로직을 여기에 추가
        Debug.Log("Music toggled: " + isOn);
    }

    private void OnEffectsToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("EffectsEnabled", isOn ? 1 : 0);
        // 추가적인 효과음 관련 로직을 여기에 추가
        Debug.Log("Effects toggled: " + isOn);
    }
}

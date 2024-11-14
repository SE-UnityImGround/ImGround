using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle toggleMusic; // 음악 토글
    public Toggle toggleEffects; // 효과음 토글

    void Start()
    {
        // PlayerPrefs에서 초기 상태 불러오기
        toggleMusic.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        toggleEffects.isOn = PlayerPrefs.GetInt("EffectsEnabled", 1) == 1;

        // 이벤트 등록
        toggleMusic.onValueChanged.AddListener(OnMusicToggleChanged);
        toggleEffects.onValueChanged.AddListener(OnEffectsToggleChanged);
    }

    void OnMusicToggleChanged(bool isOn)
    {
        // 음악 설정 저장
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        // 실제 음악 플레이어 로직 추가 (예: AudioManager)
        Debug.Log("Music Toggled: " + isOn);
    }

    void OnEffectsToggleChanged(bool isOn)
    {
        // 효과음 설정 저장
        PlayerPrefs.SetInt("EffectsEnabled", isOn ? 1 : 0);
        // 실제 효과음 플레이어 로직 추가 (예: AudioManager)
        Debug.Log("Effects Toggled: " + isOn);
    }
}

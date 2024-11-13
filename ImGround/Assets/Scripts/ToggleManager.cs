using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle toggleMusic; // ���� ���
    public Toggle toggleEffects; // ȿ���� ���

    void Start()
    {
        // PlayerPrefs���� �ʱ� ���� �ҷ�����
        toggleMusic.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        toggleEffects.isOn = PlayerPrefs.GetInt("EffectsEnabled", 1) == 1;

        // �̺�Ʈ ���
        toggleMusic.onValueChanged.AddListener(OnMusicToggleChanged);
        toggleEffects.onValueChanged.AddListener(OnEffectsToggleChanged);
    }

    void OnMusicToggleChanged(bool isOn)
    {
        // ���� ���� ����
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        // ���� ���� �÷��̾� ���� �߰� (��: AudioManager)
        Debug.Log("Music Toggled: " + isOn);
    }

    void OnEffectsToggleChanged(bool isOn)
    {
        // ȿ���� ���� ����
        PlayerPrefs.SetInt("EffectsEnabled", isOn ? 1 : 0);
        // ���� ȿ���� �÷��̾� ���� �߰� (��: AudioManager)
        Debug.Log("Effects Toggled: " + isOn);
    }
}

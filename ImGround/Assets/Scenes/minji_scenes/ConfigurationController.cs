using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationController : MonoBehaviour
{
    public Toggle toggleMusic; // ���� ���
    public Toggle toggleEffects; // ȿ���� ���

    void Start()
    {
        // �ʱ� ���� ����
        toggleMusic.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        toggleEffects.isOn = PlayerPrefs.GetInt("EffectsEnabled", 1) == 1;

        // �̺�Ʈ ������ �߰�
        toggleMusic.onValueChanged.AddListener(OnMusicToggleChanged);
        toggleEffects.onValueChanged.AddListener(OnEffectsToggleChanged);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        // �߰����� ���� ���� ������ ���⿡ �߰�
        Debug.Log("Music toggled: " + isOn);
    }

    private void OnEffectsToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("EffectsEnabled", isOn ? 1 : 0);
        // �߰����� ȿ���� ���� ������ ���⿡ �߰�
        Debug.Log("Effects toggled: " + isOn);
    }
}

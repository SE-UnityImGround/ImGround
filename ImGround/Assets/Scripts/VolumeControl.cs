using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource�� �����մϴ�.
    public Slider volumeSlider; // �����̴��� �����մϴ�.

    void Start()
    {
        // �����̴��� �ʱ� �� ����
        volumeSlider.value = audioSource.volume;

        // �����̴� �� ���� �� ȣ��� �޼ҵ� ���
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // ���� ���� �޼ҵ�
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume; // �����̴� ���� ���� ���� ����
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource를 연결합니다.
    public Slider volumeSlider; // 슬라이더를 연결합니다.

    void Start()
    {
        // 슬라이더의 초기 값 설정
        volumeSlider.value = audioSource.volume;

        // 슬라이더 값 변경 시 호출될 메소드 등록
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // 볼륨 변경 메소드
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume; // 슬라이더 값에 따라 볼륨 조절
    }
}

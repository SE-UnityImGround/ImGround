using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmAudioSource; // BGM 오디오 소스

    private void Start()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.loop = true; // BGM 반복 재생 설정
            bgmAudioSource.Play(); // BGM 재생 시작
        }
        else
        {
            Debug.LogError("BGM AudioSource가 설정되지 않았습니다!");
        }
    }
}
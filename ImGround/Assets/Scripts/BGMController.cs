using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmAudioSource; // BGM ����� �ҽ�

    private void Start()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.loop = true; // BGM �ݺ� ��� ����
            bgmAudioSource.Play(); // BGM ��� ����
        }
        else
        {
            Debug.LogError("BGM AudioSource�� �������� �ʾҽ��ϴ�!");
        }
    }
}
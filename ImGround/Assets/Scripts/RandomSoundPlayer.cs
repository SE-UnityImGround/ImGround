using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource effectsound; // AudioSource ������Ʈ
    private float nextPlayTime; // ���� ��� �ð�

    // ���� ������ ������ ������Ʈ���� �ٸ��� �����ϱ� ���� ���� 
    public float minDelay; // �ּ� ��� ����
    public float maxDelay; // �ִ� ��� ����

    void Start()
    {
        if (effectsound == null)
        {
            effectsound = gameObject.AddComponent<AudioSource>();
        }

        ScheduleNextPlay();
    }

    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            PlaySound();
            ScheduleNextPlay();
        }
    }

    private void PlaySound()
    {
        effectsound.Play();
    }

    private void ScheduleNextPlay()
    {
        // ������Ʈ���� ������ ���� ���� ������ ���� �ð� ���
        float randomDelay = Random.Range(minDelay, maxDelay);
        nextPlayTime = Time.time + randomDelay;
    }
}

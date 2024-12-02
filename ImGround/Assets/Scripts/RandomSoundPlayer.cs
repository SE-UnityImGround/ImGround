using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource effectsound; // AudioSource 컴포넌트
    private float nextPlayTime; // 다음 재생 시간

    // 랜덤 간격의 범위를 오브젝트마다 다르게 설정하기 위한 변수 
    public float minDelay; // 최소 재생 간격
    public float maxDelay; // 최대 재생 간격

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
        // 오브젝트마다 고유한 랜덤 범위 내에서 지연 시간 계산
        float randomDelay = Random.Range(minDelay, maxDelay);
        nextPlayTime = Time.time + randomDelay;
    }
}

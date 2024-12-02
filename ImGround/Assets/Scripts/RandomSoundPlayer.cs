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

        // 각 오브젝트에 고유의 재생 간격 범위를 설정
        SetRandomRange();

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

    private void SetRandomRange()
    {
        // 오브젝트마다 다른 간격 범위를 설정 (격차를 크게 설정)
        float baseTime = Random.Range(5f, 50f); // 기본 시간 범위를 랜덤으로 설정
        minDelay = baseTime; // 최소 지연 시간
        maxDelay = baseTime + Random.Range(10f, 30f); // 최대 지연 시간 (최소 시간 + 추가 시간)
    }
}

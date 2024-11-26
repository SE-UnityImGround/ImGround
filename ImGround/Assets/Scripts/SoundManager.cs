using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] bgm; // BGM 배열
    private bool isNight = false;
    private bool previousIsNight = false; // 이전 상태 저장
    public DayAndNight dayAndNightScript; // DayAndNight 스크립트 참조

    public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간
    private float maxVolume1 = 0.5f; // 1번 효과음의 최대 볼륨
    private float maxVolume2 = 0.5f; // 2번 효과음의 최대 볼륨 (필요시 조정)

    // Start is called before the first frame update
    void Start()
    {

        bgm[5].Play();

        if (!isNight)
        {
            StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1)); // 1번 효과음
            StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2)); // 2번 효과음
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight; // isNight 상태 동기화
        }

        // 밤 상태 처리
        if (isNight)
        {
            if (!bgm[2].isPlaying) bgm[2].Play(); // 3번 효과음 지속 재생
            if (!bgm[3].isPlaying) bgm[3].Play(); // 4번 효과음 밤에만 재생

            // 낮 배경음 페이드 아웃
            if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
            if (bgm[1].isPlaying) StartCoroutine(FadeOut(bgm[1], fadeDuration));
        }
        else
        {
            if (bgm[2].isPlaying) bgm[2].Stop(); // 밤 효과음 중지
            if (bgm[3].isPlaying) bgm[3].Stop(); // 4번 효과음 낮에 중지

            if (previousIsNight && !bgm[4].isPlaying)
            {
                bgm[4].Play();
            }

            // 낮 배경음 페이드 인
            if (!bgm[0].isPlaying) StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1));
            if (!bgm[1].isPlaying) StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2));
        }

        // 이전 상태 업데이트
        previousIsNight = isNight;
    }

    // 페이드 인 (최대 볼륨 제한)
    private IEnumerator FadeInWithLimit(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float startVolume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // 페이드 아웃
    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }
}

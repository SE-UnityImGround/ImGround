using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] bgm; // BGM �迭
    private bool isNight = false;
    private bool previousIsNight = false; // ���� ���� ����
    public DayAndNight dayAndNightScript; // DayAndNight ��ũ��Ʈ ����

    public float fadeDuration = 1.0f; // ���̵� ��/�ƿ� ���� �ð�
    private float maxVolume1 = 0.5f; // 1�� ȿ������ �ִ� ����
    private float maxVolume2 = 0.5f; // 2�� ȿ������ �ִ� ���� (�ʿ�� ����)

    // Start is called before the first frame update
    void Start()
    {

        bgm[5].Play();

        if (!isNight)
        {
            StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1)); // 1�� ȿ����
            StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2)); // 2�� ȿ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight; // isNight ���� ����ȭ
        }

        // �� ���� ó��
        if (isNight)
        {
            if (!bgm[2].isPlaying) bgm[2].Play(); // 3�� ȿ���� ���� ���
            if (!bgm[3].isPlaying) bgm[3].Play(); // 4�� ȿ���� �㿡�� ���

            // �� ����� ���̵� �ƿ�
            if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
            if (bgm[1].isPlaying) StartCoroutine(FadeOut(bgm[1], fadeDuration));
        }
        else
        {
            if (bgm[2].isPlaying) bgm[2].Stop(); // �� ȿ���� ����
            if (bgm[3].isPlaying) bgm[3].Stop(); // 4�� ȿ���� ���� ����

            if (previousIsNight && !bgm[4].isPlaying)
            {
                bgm[4].Play();
            }

            // �� ����� ���̵� ��
            if (!bgm[0].isPlaying) StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1));
            if (!bgm[1].isPlaying) StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2));
        }

        // ���� ���� ������Ʈ
        previousIsNight = isNight;
    }

    // ���̵� �� (�ִ� ���� ����)
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

    // ���̵� �ƿ�
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

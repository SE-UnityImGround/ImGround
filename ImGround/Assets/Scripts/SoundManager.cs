using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] bgm; // BGM �迭
    private bool isNight = false;
    private bool previousIsNight = false; // ���� ���� ����
    public DayAndNight dayAndNightScript; // DayAndNight ��ũ��Ʈ ����

    public GameObject player; // �÷��̾� ������Ʈ
    public static Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z �ּҰ�
    public static Vector3 maxBounds = new Vector3(183, 20, 148);

    public float fadeDuration = 1.0f; // ���̵� ��/�ƿ� ���� �ð�
    private float maxVolume1 = 0.4f; // 1�� ȿ������ �ִ� ����
    private float maxVolume2 = 0.4f; // 2�� ȿ������ �ִ� ���� (�ʿ�� ����)

    // Update is called once per frame
    void Update()
    {
        

        bool isWithinBounds = IsPlayerWithinBounds();

        if (isWithinBounds)
        {
            if (!bgm[5].isPlaying) bgm[5].Play();
            if (bgm[6].isPlaying) bgm[6].Stop();

            if (dayAndNightScript != null)
            {
                isNight = dayAndNightScript.isNight; // isNight ���� ����ȭ
            }

            
            if (!isNight)
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

            // �� ���� ó��
            if (isNight)
            {
                if (!bgm[2].isPlaying) bgm[2].Play(); // 3�� ȿ���� ���� ���
                if (!bgm[3].isPlaying) bgm[3].Play(); // 4�� ȿ���� �㿡�� ���

                // �� ����� ���̵� �ƿ�
                if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
                if (bgm[1].isPlaying) StartCoroutine(FadeOut(bgm[1], fadeDuration));
            }
            // ���� ���� ������Ʈ
            previousIsNight = isNight;
            Debug.Log("����");
        }
        else
        {
            if (!bgm[6].isPlaying) StartCoroutine(FadeInWithLimit(bgm[6], fadeDuration, maxVolume1));
            if (bgm[0].isPlaying) bgm[0].Stop();
            if (bgm[1].isPlaying) bgm[1].Stop();
            if (bgm[2].isPlaying) bgm[2].Stop();
            if (bgm[3].isPlaying) bgm[3].Stop();
            if (bgm[4].isPlaying) bgm[4].Stop();
            if (bgm[5].isPlaying) bgm[5].Stop();
            Debug.Log("����");
        }
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
    private bool IsPlayerWithinBounds()
    {
        if (player == null) return false;

        Vector3 playerPosition = player.transform.position;
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.y >= minBounds.y && playerPosition.y <= maxBounds.y &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }
}


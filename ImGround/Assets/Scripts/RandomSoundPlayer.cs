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

        // �� ������Ʈ�� ������ ��� ���� ������ ����
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
        // ������Ʈ���� ������ ���� ���� ������ ���� �ð� ���
        float randomDelay = Random.Range(minDelay, maxDelay);
        nextPlayTime = Time.time + randomDelay;
    }

    private void SetRandomRange()
    {
        // ������Ʈ���� �ٸ� ���� ������ ���� (������ ũ�� ����)
        float baseTime = Random.Range(5f, 50f); // �⺻ �ð� ������ �������� ����
        minDelay = baseTime; // �ּ� ���� �ð�
        maxDelay = baseTime + Random.Range(10f, 30f); // �ִ� ���� �ð� (�ּ� �ð� + �߰� �ð�)
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiRangeSoundTrigger : MonoBehaviour
{
    [System.Serializable]
    public class SoundRange
    {
        public string rangeName; // ���� �̸� (������)
        public Vector3 center; // �ڽ� �߽� ��ǥ
        public Vector3 size; // �ڽ� ũ��
        public AudioSource[] effectSounds; // �� �������� ����� ȿ���� �迭
    }

    public Transform player; // �÷��̾� Transform
    public SoundRange[] soundRanges; // ���� ������ ���� ȿ���� ����

    private Dictionary<AudioSource, bool> isPlayingDict = new Dictionary<AudioSource, bool>(); // ȿ���� ��� ���� ����
    private List<AudioSource> currentPlayingSounds = new List<AudioSource>(); // ���� ��� ���� ȿ���� ����Ʈ

    void Start()
    {
        // ��� ȿ���� �ʱ�ȭ (��� ���� false)
        foreach (var range in soundRanges)
        {
            foreach (var sound in range.effectSounds)
            {
                if (sound != null)
                {
                    isPlayingDict[sound] = false;
                }
            }
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned!");
            return;
        }

        bool anyRangeActive = false; // �÷��̾ � ���� �ȿ� �ִ��� Ȯ��
        SoundRange activeRange = null; // ���� �÷��̾ �ִ� ����

        foreach (var range in soundRanges)
        {
            // �ڽ� ���� ���
            Bounds bounds = new Bounds(range.center, range.size);
            bool isInRange = bounds.Contains(player.position);

            Debug.Log($"Checking range: {range.rangeName}, In Range: {isInRange}");

            if (isInRange)
            {
                anyRangeActive = true;
                activeRange = range;
                break; // ���� ������ ã�����Ƿ� �ݺ��� ����
            }
        }

        // ������ �ٲ���ų� �������� ������ �� ó��
        if (activeRange != null)
        {
            StopAllCurrentSounds(); // ���� ��� ���� �Ҹ� ����

            // ���ο� ���� �Ҹ� ���
            foreach (var sound in activeRange.effectSounds)
            {
                if (sound != null && !isPlayingDict[sound])
                {
                    sound.Play();
                    isPlayingDict[sound] = true;
                    currentPlayingSounds.Add(sound);
                    Debug.Log($"Playing sound in range: {activeRange.rangeName}");
                }
            }
        }
        else
        {
            // �÷��̾ � �������� ������ ���� �� ��� �Ҹ� ����
            StopAllCurrentSounds();
        }
    }

    private void StopAllCurrentSounds()
    {
        foreach (var sound in currentPlayingSounds)
        {
            if (sound != null)
            {
                sound.Stop();
                isPlayingDict[sound] = false;
            }
        }
        currentPlayingSounds.Clear();
    }

    // ����׿����� �ڽ� �ð�ȭ
    private void OnDrawGizmos()
    {
        if (soundRanges != null)
        {
            Gizmos.color = Color.green;
            foreach (var range in soundRanges)
            {
                Gizmos.DrawWireCube(range.center, range.size);
            }
        }
    }
}

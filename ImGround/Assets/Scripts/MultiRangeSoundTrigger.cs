using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiRangeSoundTrigger : MonoBehaviour
{
    [System.Serializable]
    public class SoundRange
    {
        public string rangeName; // 범위 이름 (디버깅용)
        public Vector3 center; // 박스 중심 좌표
        public Vector3 size; // 박스 크기
        public AudioSource[] effectSounds; // 이 범위에서 재생할 효과음 배열
    }

    public Transform player; // 플레이어 Transform
    public SoundRange[] soundRanges; // 여러 범위와 고유 효과음 정의

    private Dictionary<AudioSource, bool> isPlayingDict = new Dictionary<AudioSource, bool>(); // 효과음 재생 상태 관리
    private List<AudioSource> currentPlayingSounds = new List<AudioSource>(); // 현재 재생 중인 효과음 리스트

    void Start()
    {
        // 모든 효과음 초기화 (재생 상태 false)
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

        bool anyRangeActive = false; // 플레이어가 어떤 범위 안에 있는지 확인
        SoundRange activeRange = null; // 현재 플레이어가 있는 범위

        foreach (var range in soundRanges)
        {
            // 박스 범위 계산
            Bounds bounds = new Bounds(range.center, range.size);
            bool isInRange = bounds.Contains(player.position);

            Debug.Log($"Checking range: {range.rangeName}, In Range: {isInRange}");

            if (isInRange)
            {
                anyRangeActive = true;
                activeRange = range;
                break; // 현재 범위를 찾았으므로 반복문 종료
            }
        }

        // 범위가 바뀌었거나 범위에서 나갔을 때 처리
        if (activeRange != null)
        {
            StopAllCurrentSounds(); // 현재 재생 중인 소리 멈춤

            // 새로운 범위 소리 재생
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
            // 플레이어가 어떤 범위에도 속하지 않을 때 모든 소리 중지
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

    // 디버그용으로 박스 시각화
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

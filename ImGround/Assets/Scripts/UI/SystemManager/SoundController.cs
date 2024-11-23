using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    SoundType soundType;
    // audio, initial volume
    private (AudioSource, float)[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        string log_founded = "";
        bool log_hasNonAudioSource = false;

        List<(AudioSource, float)> soundObjects = new List<(AudioSource, float)>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            AudioSource audio = child.GetComponent<AudioSource>();
            if (audio != null)
            {
                soundObjects.Add((audio, audio.volume));

                log_founded += gameObject.name + "/" + child.name + "\n";
            }
            else
            {
                log_founded += "● 등록 실패(" + nameof(AudioSource) + "가 없음) : " + gameObject.name + "/" + child.name + "\n";
                log_hasNonAudioSource = true;
            }
        }
        sounds = soundObjects.ToArray();

        string logMessagePrefix = "● [object : " + gameObject.name + "]오디오 등록 결과(type : " + soundType.ToString() + ") : ";
        if (sounds.Length == 0)
        {
            Debug.LogError(logMessagePrefix + gameObject.name + "의 자식 오브젝트 중 " + nameof(AudioSource) + "를 가진 오브젝트가 없습니다!");
        }
        else
        {
            string message = logMessagePrefix + "\n" + log_founded;
            if (log_hasNonAudioSource)
                Debug.LogWarning(message);
            else
                Debug.Log(message);
        }

        SettingManager.assignSound(soundType, this);
    }

    /// <summary>
    /// 현재 스크립트가 관리하는 음악을 켜거나 끕니다.
    /// </summary>
    public void setVolume(bool isOn)
    {
        setVolume((isOn ? 1.0f : 0.0f));
    }

    /// <summary>
    /// 현재 스크립트가 관리하는 음악의 볼륨을 조절합니다.
    /// 0~1 사이의 값만 인식하며, 그렇지 않을 경우 0 미만은 0, 1 초과는 1로 처리합니다.
    /// </summary>
    public void setVolume(float volume)
    {
        if (volume < 0) volume = 0.0f;
        if (volume > 1) volume = 1.0f;

        foreach ((AudioSource, float) sound in sounds)
        {
            sound.Item1.volume = sound.Item2 * (volume / 1.0f);
        }
    }
}

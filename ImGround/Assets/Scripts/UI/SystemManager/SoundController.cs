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
                log_founded += "�� ��� ����(" + nameof(AudioSource) + "�� ����) : " + gameObject.name + "/" + child.name + "\n";
                log_hasNonAudioSource = true;
            }
        }
        sounds = soundObjects.ToArray();

        string logMessagePrefix = "�� [object : " + gameObject.name + "]����� ��� ���(type : " + soundType.ToString() + ") : ";
        if (sounds.Length == 0)
        {
            Debug.LogError(logMessagePrefix + gameObject.name + "�� �ڽ� ������Ʈ �� " + nameof(AudioSource) + "�� ���� ������Ʈ�� �����ϴ�!");
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
    /// ���� ��ũ��Ʈ�� �����ϴ� ������ �Ѱų� ���ϴ�.
    /// </summary>
    public void setVolume(bool isOn)
    {
        setVolume((isOn ? 1.0f : 0.0f));
    }

    /// <summary>
    /// ���� ��ũ��Ʈ�� �����ϴ� ������ ������ �����մϴ�.
    /// 0~1 ������ ���� �ν��ϸ�, �׷��� ���� ��� 0 �̸��� 0, 1 �ʰ��� 1�� ó���մϴ�.
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

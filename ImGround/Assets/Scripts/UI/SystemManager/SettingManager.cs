using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    // �Ҹ� Ÿ��, (�Ҹ�, on/off, ���� ����)
    private static Dictionary<SoundType, (List<SoundController> sounds, bool isOn, float volume)> soundPool = new Dictionary<SoundType, (List<SoundController>, bool, float)>();

    /// <summary>
    /// ������ �Ҹ� ���ҽ��� ����մϴ�.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="controller"></param>
    public static void assignSound(SoundType type, SoundController controller)
    {
        if (!soundPool.ContainsKey(type))
        {
            soundPool.Add(type, (new List<SoundController>(), true, 1.0f));
        }

        soundPool[type].sounds.Add(controller);
    }

    /// <summary>
    /// �Ҹ� ���ҽ��� �Ѱų� ���ϴ�.
    /// </summary>
    /// <param name="type"></param>
    public static void setSoundVolume(SoundType type, bool isOn)
    {
        if (!soundPool.ContainsKey(type))
        {
            // �Ҹ� ���� ���ӿ�����Ʈ���� �ݵ�� SoundController ��ũ��Ʈ�� �پ����� ��.
            Debug.LogWarning("�Ҹ� �ѱ�/���� : Ÿ�� " + type.ToString() + "�� �Ҹ��� �ϳ��� ��ϵ��� �ʾҽ��ϴ�.\n" + nameof(SoundController) + "��ũ��Ʈ�� ���� ���ӿ�����Ʈ�� ��ϵǾ��ִ��� Ȯ�����ּ���!");
            return;
        }

        (List<SoundController> sounds, bool isOn, float volume) soundData = soundPool[type];
        soundData.isOn = isOn;
        soundPool[type] = soundData;

        setTotalSoundVolume(type);
    }

    /// <summary>
    /// �Ҹ� ���ҽ��� ������ �����մϴ�.
    /// </summary>
    /// <param name="type"></param>
    public static void setSoundVolume(SoundType type, float volume)
    {
        if (!soundPool.ContainsKey(type))
        {
            // �Ҹ� ���� ���ӿ�����Ʈ���� �ݵ�� SoundController ��ũ��Ʈ�� �پ����� ��.
            Debug.LogWarning("�Ҹ� ���� : Ÿ�� " + type.ToString() + "�� �Ҹ��� �ϳ��� ��ϵ��� �ʾҽ��ϴ�.\n" + nameof(SoundController) + "��ũ��Ʈ�� ���� ���ӿ�����Ʈ�� ��ϵǾ��ִ��� Ȯ�����ּ���!");
            return;
        }

        (List<SoundController> sounds, bool isOn, float volume) soundData = soundPool[type];
        soundData.volume = volume;
        soundPool[type] = soundData;

        setTotalSoundVolume(type);
    }

    private static void setTotalSoundVolume(SoundType type)
    {
        (List<SoundController> sounds, bool isOn, float volume) soundData = soundPool[type];

        foreach (SoundController sound in soundData.sounds)
        {
            if (sound == null)
                continue;

            if (soundData.isOn)
                sound.setVolume(soundData.volume);
            else
                sound.setVolume(soundData.isOn);
        }
    }
}

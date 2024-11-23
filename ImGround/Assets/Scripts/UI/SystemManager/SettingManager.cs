using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    private static Dictionary<SoundType, List<SoundController>> soundPool = new Dictionary<SoundType, List<SoundController>>();

    /// <summary>
    /// ������ �Ҹ� ���ҽ��� ����մϴ�.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="controller"></param>
    public static void assignSound(SoundType type, SoundController controller)
    {
        if (!soundPool.ContainsKey(type))
        {
            soundPool.Add(type, new List<SoundController>());
        }

        soundPool[type].Add(controller);
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

        foreach (SoundController sound in soundPool[type])
        {
            sound.setVolume(isOn);
        }
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

        foreach (SoundController sound in soundPool[type])
        {
            sound.setVolume(volume);
        }
    }
}

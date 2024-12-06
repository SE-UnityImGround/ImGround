using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    // 소리 타입, (소리, on/off, 현재 볼륨)
    private static Dictionary<SoundType, (List<SoundController> sounds, bool isOn, float volume)> soundPool = new Dictionary<SoundType, (List<SoundController>, bool, float)>();

    /// <summary>
    /// 조절할 소리 리소스를 등록합니다.
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
    /// 소리 리소스를 켜거나 끕니다.
    /// </summary>
    /// <param name="type"></param>
    public static void setSoundVolume(SoundType type, bool isOn)
    {
        if (!soundPool.ContainsKey(type))
        {
            // 소리 관련 게임오브젝트에는 반드시 SoundController 스크립트가 붙어있을 것.
            Debug.LogWarning("소리 켜기/끄기 : 타입 " + type.ToString() + "의 소리가 하나도 등록되지 않았습니다.\n" + nameof(SoundController) + "스크립트가 사운드 게임오브젝트에 등록되어있는지 확인해주세요!");
            return;
        }

        (List<SoundController> sounds, bool isOn, float volume) soundData = soundPool[type];
        soundData.isOn = isOn;
        soundPool[type] = soundData;

        setTotalSoundVolume(type);
    }

    /// <summary>
    /// 소리 리소스의 볼륨을 조절합니다.
    /// </summary>
    /// <param name="type"></param>
    public static void setSoundVolume(SoundType type, float volume)
    {
        if (!soundPool.ContainsKey(type))
        {
            // 소리 관련 게임오브젝트에는 반드시 SoundController 스크립트가 붙어있을 것.
            Debug.LogWarning("소리 조절 : 타입 " + type.ToString() + "의 소리가 하나도 등록되지 않았습니다.\n" + nameof(SoundController) + "스크립트가 사운드 게임오브젝트에 등록되어있는지 확인해주세요!");
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

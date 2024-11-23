using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    private static Dictionary<SoundType, List<SoundController>> soundPool = new Dictionary<SoundType, List<SoundController>>();

    /// <summary>
    /// 조절할 소리 리소스를 등록합니다.
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

        foreach (SoundController sound in soundPool[type])
        {
            sound.setVolume(isOn);
        }
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

        foreach (SoundController sound in soundPool[type])
        {
            sound.setVolume(volume);
        }
    }
}

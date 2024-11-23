using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBehavior : MonoBehaviour
{
    public void onMusicSettingChanged(ToggleButton btn)
    {
        SettingManager.setSoundVolume(SoundType.BACKGROUND, btn.value);
    }

    public void onEffectSettingChanged(ToggleButton btn)
    {
        SettingManager.setSoundVolume(SoundType.EFFECT, btn.value);
    }

    public void onSliderChanged(Slider slider)
    {
        SettingManager.setSoundVolume(SoundType.BACKGROUND, slider.value);
    }
}

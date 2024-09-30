using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSettings : MonoBehaviour
{
    public void LoadChangeSettings()
    {
        // "Setting_scenes"라는 이름의 씬이 존재하는지 확인
        if (Application.CanStreamedLevelBeLoaded("Setting_scenes"))
        {
            SceneManager.LoadScene("Setting_scenes");
        }
        else
        {
            Debug.LogError("Scene 'Setting_scenes' not found!");
        }
    }
}



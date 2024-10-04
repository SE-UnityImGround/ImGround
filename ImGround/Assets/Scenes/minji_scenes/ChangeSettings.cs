using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSettings : MonoBehaviour
{
    public void LoadChangeSettings()
    {
        // "Setting_scenes"��� �̸��� ���� �����ϴ��� Ȯ��
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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMission : MonoBehaviour
{
    public void LoadMissionScene()
    {
        // "Mission_scenes"��� �̸��� ���� �����ϴ��� Ȯ��
        if (Application.CanStreamedLevelBeLoaded("Mission_scenes"))
        {
            SceneManager.LoadScene("Mission_scenes");
        }
        else
        {
            Debug.LogError("Scene 'Mission_scenes' not found!");
        }
    }
}
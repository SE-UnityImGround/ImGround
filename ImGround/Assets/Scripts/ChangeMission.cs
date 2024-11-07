using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMission : MonoBehaviour
{
    public void LoadMissionScene()
    {
        // "Mission_scenes"��� �̸��� ���� �����ϴ��� Ȯ��
        if (Application.CanStreamedLevelBeLoaded("Quest_scenes"))
        {
            SceneManager.LoadScene("Quest_scenes");
        }
        else
        {
            Debug.LogError("Scene 'Quest_scenes" +
                "' not found!");
        }
    }
}
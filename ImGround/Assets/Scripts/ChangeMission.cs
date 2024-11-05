using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMission : MonoBehaviour
{
    public void LoadMissionScene()
    {
        // "Mission_scenes"라는 이름의 씬이 존재하는지 확인
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMission : MonoBehaviour
{
    public void LoadMissionScene()
    {
        // "Mission_scenes"라는 이름의 씬이 존재하는지 확인
        if (Application.CanStreamedLevelBeLoaded("Achievements_scenes"))
        {
            SceneManager.LoadScene("Achievements_scenes");
        }
        else
        {
            Debug.LogError("Scene 'Achievements" +
                "' not found!");
        }
    }
}
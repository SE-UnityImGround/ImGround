using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMission : MonoBehaviour
{
    public void LoadMissionScene()
    {
        // "Mission_scenes"��� �̸��� ���� �����ϴ��� Ȯ��
        if (Application.CanStreamedLevelBeLoaded("Achievements"))
        {
            SceneManager.LoadScene("Achievements");
        }
        else
        {
            Debug.LogError("Scene 'Achievements' not found!");
        }
    }
}
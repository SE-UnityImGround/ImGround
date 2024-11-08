using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeInventory: MonoBehaviour
{
    public void LoadInventoryScenes()
    {
        // "Mission_scenes"라는 이름의 씬이 존재하는지 확인
        if (Application.CanStreamedLevelBeLoaded("Eunju_PlayScene"))
        {
            SceneManager.LoadScene("Eunju_PlayScene");
        }
        else
        {
            Debug.LogError("Scene 'Eunju_PlayScene' not found!");
        }
    }
}

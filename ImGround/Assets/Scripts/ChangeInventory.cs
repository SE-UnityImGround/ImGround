using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeInventory: MonoBehaviour
{
    public void LoadInventoryScenes()
    {
        // "Mission_scenes"��� �̸��� ���� �����ϴ��� Ȯ��
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

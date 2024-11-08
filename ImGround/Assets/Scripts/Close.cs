using UnityEngine;

public class CloseGame : MonoBehaviour
{
    void Update()
    {
        // 'Escape' 키를 눌렀을 때 게임을 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow();
        }
    }

    public void CloseWindow()
    {
        // 게임 종료
        Application.Quit();

        // 에디터에서 테스트할 때
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


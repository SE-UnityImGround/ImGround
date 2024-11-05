using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : MonoBehaviour
{
    public void OnCloseButtonClick()
    {
        // 첫 번째 화면으로 돌아가기
        SceneManager.LoadScene("main_scene");
    }
}

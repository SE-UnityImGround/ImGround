using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : MonoBehaviour
{
    public void OnCloseButtonClick()
    {
        // ù ��° ȭ������ ���ư���
        SceneManager.LoadScene("main_scene");
    }
}

using UnityEngine;

public class CloseGame : MonoBehaviour
{
    void Update()
    {
        // 'Escape' Ű�� ������ �� ������ ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow();
        }
    }

    public void CloseWindow()
    {
        // ���� ����
        Application.Quit();

        // �����Ϳ��� �׽�Ʈ�� ��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeToOutside : MonoBehaviour
{
    public GameObject player;  // Player ������Ʈ�� ������ ����
    public Vector3 targetPosition;  // �̵��� ��ġ

    void Start()
    {
        // ���� �ε�� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (player != null)
        {
            // Player�� ���� x, z ��ġ�� �޾Ƽ� ���
            Vector3 playerPos = player.transform.position;
            Debug.Log("Player Position - X: " + playerPos.x + ", Z: " + playerPos.z);

            if (playerPos.x <= 3.8 && playerPos.x >= 3.5 && playerPos.z <= 5.1 && playerPos.z >= 3.1)
            {
                // �� �ε� (�񵿱������� �ε�)
                SceneManager.LoadSceneAsync("yujin_Environment");
            }
        }
    }

    // ���� �ε�� �Ŀ� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_Environment" && player != null)
        {
            // ���� �ε�� �� �÷��̾� ��ġ�� ���ϴ� ��ġ�� ����
            player.transform.position = targetPosition;
        }
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

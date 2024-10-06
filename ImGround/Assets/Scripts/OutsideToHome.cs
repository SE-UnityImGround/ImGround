/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
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

            if (playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                // �� �ε�
                SceneManager.LoadSceneAsync("yujin_house");
            }
        }
    }

    // ���� �ε�� �Ŀ� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_house" && player != null)
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
{
    public Vector3 targetPosition;  // �̵��� ��ġ
    private Player player;          // Player ������Ʈ�� ����

    void Start()
    {
        // ���� �ε�� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = Player.GetInstance();  // �̱��� �������� �÷��̾� ����
    }

    void Update()
    {
        if (player != null)
        {
            // Player�� ���� x, z ��ġ�� �޾Ƽ� Ȯ��
            Vector3 playerPos = player.transform.position;

            if (playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                // �� �ε� (�񵿱������� �ε�)
                SceneManager.LoadSceneAsync("yujin_house");
            }
        }
    }

    // ���� �ε�� �Ŀ� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_house")
        {
            // ���� �ε�� �� �÷��̾� ��ġ�� ���ϴ� ��ġ�� ����
            if (player != null)
            {
                player.transform.position = targetPosition;
            }
        }
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

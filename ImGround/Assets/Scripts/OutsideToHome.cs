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

/*using System.Collections;
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
            StartCoroutine(SetPlayerPositionAfterLoad());
        }
    }

    // �ڷ�ƾ�� ����� �� �ε� �� �÷��̾� ��ġ ����
    IEnumerator SetPlayerPositionAfterLoad()
    {
        yield return new WaitForSeconds(0.1f); // �ణ�� �����̸� �� (���� ������ �ε�Ǳ� ���� ��ġ ���� ����)

        if (player != null)
        {
            player.transform.position = targetPosition;
            Debug.Log("Player moved to target position: " + targetPosition);
        }
        else
        {
            Debug.LogWarning("Player object not found in the new scene!");
        }
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
*/


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
{
    private Player player;  // Player ������Ʈ�� ������ ����
    public Vector3 targetPosition;  // �̵��� ��ġ
    private bool isLoadingScene = false; // �� �ε� ������ ���θ� Ȯ��
    private float sceneChangeCooldown = 1.0f;  // �� ��ȯ �� ������ �ð�
    private float lastSceneChangeTime = -1.0f; // ������ �� ��ȯ ����

    void Start()
    {
        // ���� �ε�� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = Player.GetInstance(); // Player �̱��� ������ ����
    }

    void Update()
    {
        if (player != null && !isLoadingScene && Time.time > lastSceneChangeTime + sceneChangeCooldown)
        {
            // Player�� ���� x, z ��ġ�� �޾Ƽ� Ȯ��
            Vector3 playerPos = player.transform.position;

            if (playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                // �� �ε�
                isLoadingScene = true;  // �� �ε� ������ ǥ��
                lastSceneChangeTime = Time.time;  // ������ �� ��ȯ �ð� ������Ʈ
                StartCoroutine(LoadSceneWithDelay("yujin_house"));
            }
        }
    }

    // �񵿱� �� �ε带 �����ϴ� �ڷ�ƾ
    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);  // 0.1�� ������
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // ���� ������ �ε�� ������ ���
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        isLoadingScene = false;  // �� �ε尡 �Ϸ�Ǿ����� ǥ��
    }

    // ���� �ε�� �Ŀ� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� �� Player �̱��� �ν��Ͻ��� �����Ǵ��� Ȯ��
        Player player = Player.GetInstance();
        Debug.Log("Player instance ID after scene load: " + player.GetInstanceID());

        // �� �̸��� 'yujin_house'�� ��쿡 �÷��̾� ��ġ�� �̵�
        if (scene.name == "yujin_house" && player != null)
        {
            player.transform.position = targetPosition;
            Debug.Log("Player moved to target position: " + targetPosition);
        }
    }


    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
*/


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
{
    public Player player;  // Player ������Ʈ�� ������ ����
    //public Vector3 targetPosition;  // �̵��� ��ġ

    void Start()
    {
        // ���� �ε�� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    void Update()
    {
        Debug.Log("��ġ" + player.transform.position);
        player = Player.GetInstance();
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
        Player player = Player.GetInstance();
        *//*if (scene.name == "yujin_house" && player != null)
        {
            // ���� �ε�� �� �÷��̾� ��ġ�� ���ϴ� ��ġ�� ����
            //player.transform.position = targetPosition;
        }*//*
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
{
    private Player player;  // Player ������Ʈ�� ������ ����

    void Start()
    {
        // Player �̱��� ��������
        player = Player.GetInstance();
        if (player == null)
        {
            Debug.LogError("Player ��ü�� ã�� ���߽��ϴ�.");
        }
        else
        {
            Debug.Log("Player ��ü �ʱ�ȭ ����: " + player.transform.position);
        }

        // ���� �ε�� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        // Player �ν��Ͻ��� ���� ��� �ٽ� ������
        if (player == null)
        {
            player = Player.GetInstance();
            if (player == null)
            {
                Debug.LogError("Update���� Player ��ü�� ã�� ���߽��ϴ�.");
                return;  // player�� null�̸� ���� �ڵ带 �������� ����
            }
        }

        // player�� ���������� �Ҵ�Ǿ��� ��� ��ġ Ȯ��
        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
            Debug.Log("Player ��ġ: " + playerPos);

            if (playerPos.x <= 52.2f && playerPos.x >= 51.1f && playerPos.z <= 87.9f && playerPos.z >= 86.5f)
            {
                // �� �ε�
                SceneManager.LoadSceneAsync("yujin_house");
            }
        }
    }

    // ���� �ε�� �Ŀ� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = Player.GetInstance(); // ���� �ε�� �Ŀ��� player ��ü �ٽ� ��������
        if (player == null)
        {
            Debug.LogError("�� �ε� �� Player ��ü�� ã�� ���߽��ϴ�.");
        }
        else
        {
            Debug.Log("�� �ε� �� player ��ġ: " + player.transform.position);
        }
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

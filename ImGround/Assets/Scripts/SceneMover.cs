using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    public GameObject player;  // Player ������Ʈ�� ������ ����
    public Vector3 targetPosition;  // �̵��� ��ġ


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Player�� ���� x, z ��ġ�� �޾Ƽ� ���
            Vector3 playerPos = player.transform.position;
            //Debug.Log("Player Position - X: " + playerPos.x + ", Z: " + playerPos.z);
            if(playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                //Debug.Log("����");
                SceneManager.LoadScene("yujin_house");
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_house" && player != null)
        {
            // ���� �ε�� �� �÷��̾� ��ġ�� ���ϴ� ��ġ�� ����
            player.transform.position = targetPosition;
        }
    }





}

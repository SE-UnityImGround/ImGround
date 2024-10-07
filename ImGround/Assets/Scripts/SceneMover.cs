using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    public GameObject player;  // Player 오브젝트를 참조할 변수
    public Vector3 targetPosition;  // 이동할 위치


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Player의 현재 x, z 위치를 받아서 출력
            Vector3 playerPos = player.transform.position;
            //Debug.Log("Player Position - X: " + playerPos.x + ", Z: " + playerPos.z);
            if(playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                //Debug.Log("입장");
                SceneManager.LoadScene("yujin_house");
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_house" && player != null)
        {
            // 씬이 로드된 후 플레이어 위치를 원하는 위치로 설정
            player.transform.position = targetPosition;
        }
    }





}

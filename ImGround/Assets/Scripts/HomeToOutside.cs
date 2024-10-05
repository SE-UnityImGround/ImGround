using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeToOutside : MonoBehaviour
{
    public GameObject player;  // Player 오브젝트를 참조할 변수
    public Vector3 targetPosition;  // 이동할 위치

    void Start()
    {
        // 씬이 로드될 때 호출되는 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (player != null)
        {
            // Player의 현재 x, z 위치를 받아서 출력
            Vector3 playerPos = player.transform.position;
            Debug.Log("Player Position - X: " + playerPos.x + ", Z: " + playerPos.z);

            if (playerPos.x <= 3.8 && playerPos.x >= 3.5 && playerPos.z <= 5.1 && playerPos.z >= 3.1)
            {
                // 씬 로드 (비동기적으로 로드)
                SceneManager.LoadSceneAsync("yujin_Environment");
            }
        }
    }

    // 씬이 로드된 후에 호출되는 이벤트 핸들러
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_Environment" && player != null)
        {
            // 씬이 로드된 후 플레이어 위치를 원하는 위치로 설정
            player.transform.position = targetPosition;
        }
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

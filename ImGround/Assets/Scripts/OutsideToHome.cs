/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
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

            if (playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                // 씬 로드
                SceneManager.LoadSceneAsync("yujin_house");
            }
        }
    }

    // 씬이 로드된 후에 호출되는 이벤트 핸들러
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_house" && player != null)
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
{
    public Vector3 targetPosition;  // 이동할 위치
    private Player player;          // Player 오브젝트를 참조

    void Start()
    {
        // 씬이 로드될 때 호출되는 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = Player.GetInstance();  // 싱글톤 패턴으로 플레이어 참조
    }

    void Update()
    {
        if (player != null)
        {
            // Player의 현재 x, z 위치를 받아서 확인
            Vector3 playerPos = player.transform.position;

            if (playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                // 씬 로드 (비동기적으로 로드)
                SceneManager.LoadSceneAsync("yujin_house");
            }
        }
    }

    // 씬이 로드된 후에 호출되는 이벤트 핸들러
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "yujin_house")
        {
            // 씬이 로드된 후 플레이어 위치를 원하는 위치로 설정
            if (player != null)
            {
                player.transform.position = targetPosition;
            }
        }
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

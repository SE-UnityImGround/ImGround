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

/*using System.Collections;
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
            StartCoroutine(SetPlayerPositionAfterLoad());
        }
    }

    // 코루틴을 사용해 씬 로드 후 플레이어 위치 설정
    IEnumerator SetPlayerPositionAfterLoad()
    {
        yield return new WaitForSeconds(0.1f); // 약간의 딜레이를 줌 (씬이 완전히 로드되기 전에 위치 설정 방지)

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
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
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
    private Player player;  // Player 오브젝트를 참조할 변수
    public Vector3 targetPosition;  // 이동할 위치
    private bool isLoadingScene = false; // 씬 로드 중인지 여부를 확인
    private float sceneChangeCooldown = 1.0f;  // 씬 전환 후 딜레이 시간
    private float lastSceneChangeTime = -1.0f; // 마지막 씬 전환 시점

    void Start()
    {
        // 씬이 로드될 때 호출되는 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = Player.GetInstance(); // Player 싱글톤 참조로 수정
    }

    void Update()
    {
        if (player != null && !isLoadingScene && Time.time > lastSceneChangeTime + sceneChangeCooldown)
        {
            // Player의 현재 x, z 위치를 받아서 확인
            Vector3 playerPos = player.transform.position;

            if (playerPos.x <= 52.2 && playerPos.x >= 51.1 && playerPos.z <= 87.9 && playerPos.z >= 86.5)
            {
                // 씬 로드
                isLoadingScene = true;  // 씬 로드 중임을 표시
                lastSceneChangeTime = Time.time;  // 마지막 씬 전환 시간 업데이트
                StartCoroutine(LoadSceneWithDelay("yujin_house"));
            }
        }
    }

    // 비동기 씬 로드를 수행하는 코루틴
    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);  // 0.1초 딜레이
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 씬이 완전히 로드될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        isLoadingScene = false;  // 씬 로드가 완료되었음을 표시
    }

    // 씬이 로드된 후에 호출되는 이벤트 핸들러
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 Player 싱글톤 인스턴스가 유지되는지 확인
        Player player = Player.GetInstance();
        Debug.Log("Player instance ID after scene load: " + player.GetInstanceID());

        // 씬 이름이 'yujin_house'인 경우에 플레이어 위치를 이동
        if (scene.name == "yujin_house" && player != null)
        {
            player.transform.position = targetPosition;
            Debug.Log("Player moved to target position: " + targetPosition);
        }
    }


    void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
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
    public Player player;  // Player 오브젝트를 참조할 변수
    //public Vector3 targetPosition;  // 이동할 위치

    void Start()
    {
        // 씬이 로드될 때 호출되는 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    void Update()
    {
        Debug.Log("위치" + player.transform.position);
        player = Player.GetInstance();
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
        Player player = Player.GetInstance();
        *//*if (scene.name == "yujin_house" && player != null)
        {
            // 씬이 로드된 후 플레이어 위치를 원하는 위치로 설정
            //player.transform.position = targetPosition;
        }*//*
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutsideToHome : MonoBehaviour
{
    private Player player;  // Player 오브젝트를 참조할 변수

    void Start()
    {
        // Player 싱글톤 가져오기
        player = Player.GetInstance();
        if (player == null)
        {
            Debug.LogError("Player 객체를 찾지 못했습니다.");
        }
        else
        {
            Debug.Log("Player 객체 초기화 성공: " + player.transform.position);
        }

        // 씬이 로드될 때 호출되는 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        // Player 인스턴스가 없을 경우 다시 가져옴
        if (player == null)
        {
            player = Player.GetInstance();
            if (player == null)
            {
                Debug.LogError("Update에서 Player 객체를 찾지 못했습니다.");
                return;  // player가 null이면 이후 코드를 실행하지 않음
            }
        }

        // player가 정상적으로 할당되었을 경우 위치 확인
        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
            Debug.Log("Player 위치: " + playerPos);

            if (playerPos.x <= 52.2f && playerPos.x >= 51.1f && playerPos.z <= 87.9f && playerPos.z >= 86.5f)
            {
                // 씬 로드
                SceneManager.LoadSceneAsync("yujin_house");
            }
        }
    }

    // 씬이 로드된 후에 호출되는 이벤트 핸들러
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = Player.GetInstance(); // 씬이 로드된 후에도 player 객체 다시 가져오기
        if (player == null)
        {
            Debug.LogError("씬 로드 후 Player 객체를 찾지 못했습니다.");
        }
        else
        {
            Debug.Log("씬 로드 후 player 위치: " + player.transform.position);
        }
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

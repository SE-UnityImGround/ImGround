using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstpersonCamera : MonoBehaviour
{
    public Transform target; // 카메라의 타겟(플레이어의 Face)
    public Vector3 focusOffset = new Vector3(0, 0, 0); // 카메라가 타겟의 원점에 위치하도록 오프셋 설정
    public float mouseSensitivity = 100f; // 마우스 감도
    public Transform playerBody; // 플레이어 몸체 (회전 처리용)

    private float xRotation = 0f; // 카메라의 x축 회전 값 (위/아래 보기)
    public float moveSpeed = 5f; // 플레이어의 이동 속도

    private void Awake()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // 마우스 커서를 화면 중앙에 고정하고 숨김 처리
        Cursor.lockState = CursorLockMode.Locked;

        // 씬이 로드될 때마다 플레이어를 찾아 타겟을 설정
        FindPlayerTarget();
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 시마다 플레이어를 찾아서 타겟 설정
        FindPlayerTarget();
    }

    void FindPlayerTarget()
    {
        // DontDestroy된 플레이어를 찾아 타겟으로 설정
        Player player = Player.GetInstance();
        if (player != null)
        {
            // Mesh 안에 있는 Face를 타겟으로 설정
            Transform faceTransform = player.transform.Find("Mesh/Face");
            if (faceTransform != null)
            {
                target = faceTransform;
                playerBody = player.transform; // 플레이어 전체 몸체를 참조
            }
            else
            {
                Debug.LogWarning("Face object not found inside the Player's Mesh!");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    void Update()
    {
        if (target != null)
        {
            // 마우스 입력을 받음
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // 위아래 카메라 회전 (x축 기준)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 위/아래 회전 각도 제한

            // 카메라 회전 적용 (카메라만 위아래 회전)
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // 플레이어 몸체의 좌우 회전 (y축 기준)
            playerBody.Rotate(Vector3.up * mouseX);

            // 카메라 위치를 얼굴 위치로 고정
            transform.position = target.position + focusOffset;

            // 플레이어의 이동 처리
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        // 플레이어의 이동 처리: WASD 키로 앞뒤, 좌우 이동 처리
        float moveX = Input.GetAxis("Horizontal"); // A, D 또는 좌, 우 방향 이동
        float moveZ = Input.GetAxis("Vertical");   // W, S 또는 전진, 후진

        // 이동 벡터 생성 (앞/뒤, 좌/우)
        Vector3 moveDirection = playerBody.transform.right * moveX + playerBody.transform.forward * moveZ;

        // 이동 처리 (Time.deltaTime 사용하여 프레임 독립적인 이동 처리)
        playerBody.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}

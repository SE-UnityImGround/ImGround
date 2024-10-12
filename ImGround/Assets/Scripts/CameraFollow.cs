/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 focusOffset = new Vector3(0,2,0); // 카메라가 주목할 타겟의 원점 오프셋
    public float cameraDistance = 5; // 카메라의 거리
    [SerializeField]
    private float accel = 120.0f; // 회전 가속량

    // x회전축의 최소/최대 각도
    private const float MIN_X_ROTATION = 0.0f; 
    private const float MAX_X_ROTATION = 80.0f;

    private const float DECAY_MULTIPLY = 25.0f; // 회전속도 보간 가속량
    private float xVelocity; // x회전축 회전속도(위아래)
    private float yVelocity; // y회전축 회전속도(좌우)

    void Update()
    {
        yVelocity += Input.GetAxisRaw("Mouse X"); // 마우스 X 성분은 좌우 회전을 담당
        xVelocity -= Input.GetAxisRaw("Mouse Y"); // 마우스 Y 성분에 음수 곱하여 방향 반전 (위로 올리면 시점이 위로, 아래로 내리면 시점이 아래로)

        // 좌우 회전 (Y축 회전)
        transform.Rotate(0.0f, yVelocity * accel * Time.deltaTime, 0.0f, Space.World);

        // 위아래 회전 (X축 회전) - 최소/최대 회전 한계를 검사
        if (MIN_X_ROTATION > transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(MIN_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            xVelocity = 0.0f;
        }
        else if (MAX_X_ROTATION < transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(MAX_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            xVelocity = 0.0f;
        }
        else
        {
            transform.Rotate(xVelocity * accel * Time.deltaTime, 0.0f, 0.0f, Space.Self);
        }

        // 플레이어로부터 카메라의 최종 위치 계산
        transform.position = target.position + focusOffset - (transform.rotation * Vector3.forward * cameraDistance);

        // 선형 보간으로 자연스러운 속도 감소 적용
        xVelocity = Mathf.Lerp(xVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
        yVelocity = Mathf.Lerp(yVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
    }

}
*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 focusOffset = new Vector3(0, 2, 0); // 카메라가 주목할 타겟의 원점 오프셋
    public float cameraDistance = 5; // 카메라의 거리
    [SerializeField]
    private float accel = 120.0f; // 회전 가속량

    // x회전축의 최소/최대 각도
    private const float MIN_X_ROTATION = 0.0f;
    private const float MAX_X_ROTATION = 80.0f;

    private const float DECAY_MULTIPLY = 25.0f; // 회전속도 보간 가속량
    private float xVelocity; // x회전축 회전속도(위아래)
    private float yVelocity; // y회전축 회전속도(좌우)

    private static CameraFollow instance;

    void Awake()
    {
        // 카메라 중복 방지 및 DontDestroyOnLoad 적용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 카메라 파괴
        }

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerTarget();
    }

    void FindPlayerTarget()
    {
        // 플레이어 오브젝트를 레이어로 찾아서 타겟 설정
        GameObject[] players = FindObjectsOfType<GameObject>();

        foreach (GameObject player in players)
        {
            if (player.layer == LayerMask.NameToLayer("Player"))
            {
                target = player.transform;
                return;
            }
        }

        Debug.LogWarning("Player object not found!");
    }

    void Update()
    {
        if (target != null)
        {
            yVelocity += Input.GetAxisRaw("Mouse X"); // 마우스 X 성분은 좌우 회전을 담당
            xVelocity -= Input.GetAxisRaw("Mouse Y"); // 마우스 Y 성분에 음수 곱하여 방향 반전 (위로 올리면 시점이 위로, 아래로 내리면 시점이 아래로)

            // 좌우 회전 (Y축 회전)
            transform.Rotate(0.0f, yVelocity * accel * Time.deltaTime, 0.0f, Space.World);

            // 위아래 회전 (X축 회전) - 최소/최대 회전 한계를 검사
            if (MIN_X_ROTATION > transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
            {
                transform.rotation = Quaternion.Euler(MIN_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                xVelocity = 0.0f;
            }
            else if (MAX_X_ROTATION < transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
            {
                transform.rotation = Quaternion.Euler(MAX_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                xVelocity = 0.0f;
            }
            else
            {
                transform.Rotate(xVelocity * accel * Time.deltaTime, 0.0f, 0.0f, Space.Self);
            }

            // 플레이어로부터 카메라의 최종 위치 계산
            transform.position = target.position + focusOffset - (transform.rotation * Vector3.forward * cameraDistance);

            // 선형 보간으로 자연스러운 속도 감소 적용
            xVelocity = Mathf.Lerp(xVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
            yVelocity = Mathf.Lerp(yVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
        }
    }
}
*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 focusOffset = new Vector3(0, 2, 0); // 카메라가 주목할 타겟의 원점 오프셋
    public float cameraDistance = 5; // 카메라의 거리
    [SerializeField]
    private float accel = 120.0f; // 회전 가속량

    // x회전축의 최소/최대 각도
    private const float MIN_X_ROTATION = 0.0f;
    private const float MAX_X_ROTATION = 80.0f;

    private const float DECAY_MULTIPLY = 25.0f; // 회전속도 보간 가속량
    private float xVelocity; // x회전축 회전속도(위아래)
    private float yVelocity; // y회전축 회전속도(좌우)

    private Player player;

    void Awake()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
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
        player = Player.GetInstance();
        if (player != null)
        {
            target = player.transform;
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
            yVelocity += Input.GetAxisRaw("Mouse X"); // 마우스 X 성분은 좌우 회전을 담당
            xVelocity -= Input.GetAxisRaw("Mouse Y"); // 마우스 Y 성분에 음수 곱하여 방향 반전 (위로 올리면 시점이 위로, 아래로 내리면 시점이 아래로)

            // 좌우 회전 (Y축 회전)
            transform.Rotate(0.0f, yVelocity * accel * Time.deltaTime, 0.0f, Space.World);

            // 위아래 회전 (X축 회전) - 최소/최대 회전 한계를 검사
            if (MIN_X_ROTATION > transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
            {
                transform.rotation = Quaternion.Euler(MIN_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                xVelocity = 0.0f;
            }
            else if (MAX_X_ROTATION < transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
            {
                transform.rotation = Quaternion.Euler(MAX_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                xVelocity = 0.0f;   
            }
            else
            {
                transform.Rotate(xVelocity * accel * Time.deltaTime, 0.0f, 0.0f, Space.Self);
            }

            // 플레이어로부터 카메라의 최종 위치 계산
            transform.position = target.position + focusOffset - (transform.rotation * Vector3.forward * cameraDistance);

            // 선형 보간으로 자연스러운 속도 감소 적용
            xVelocity = Mathf.Lerp(xVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
            yVelocity = Mathf.Lerp(yVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
        }
    }
}

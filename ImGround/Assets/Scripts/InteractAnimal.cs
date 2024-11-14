using UnityEngine;
using UnityEngine.UI;

public class InteractAnimal : MonoBehaviour
{
    public GameObject gaugeBarPrefab;       // 게이지바 프리팹
    public GameObject itemPrefab;           // 아이템 프리팹
    private GameObject gaugeBarInstance;
    private float gaugeProgress = 0f;
    private bool isInteracting = false;
    private static bool isInteractingTriggered = false; // 하나의 동물과만 상호작용 가능
    private float interactingTimeRequired = 4f;  // 4초 동안 상호작용
    public Transform canvasTransform;       // 게이지바를 붙일 Canvas의 Transform
    public Vector3 itemSize = new Vector3(3f, 3f, 3f);  // 드롭될 아이템의 크기
    public Transform playerTransform;       // 플레이어의 Transform
    public float allowedDistance = 2f;      // 상호작용 범위
    public LayerMask animalLayer;              // 동물 레이어 마스크

    void Update()
    {
        // 마우스 오른쪽 버튼을 누르고 있는 동안
        if (Input.GetMouseButton(1)) // 오른쪽 마우스 버튼
        {
            // 맨손 범위 내에 있는 모든 동물을 감지
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, animalLayer);

            // 가장 가까운 정면 방향의 동물을 찾기 위한 변수
            Transform closestAnimal = null;
            float closestAngle = 45f;  // 정면 기준 허용 각도

            // 감지된 동물 중에서 플레이어가 바라보는 방향에 가까운 동물을 선택
            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToAnimal = (hitCollider.transform.position - playerTransform.position).normalized;
                float angleToAnimal = Vector3.Angle(playerTransform.forward, directionToAnimal);

                // 정면 각도 내에 있는 동물이면서 가장 가까운 각도의 동물을 선택
                if (angleToAnimal < closestAngle)
                {
                    closestAngle = angleToAnimal;
                    closestAnimal = hitCollider.transform;
                }
            }

            // 가장 가까운 동물이 현재 오브젝트이고, 아직 상호작용이 시작되지 않은 경우
            if (closestAnimal == transform && !isInteractingTriggered)
            {
                StartInteracting();
            }

            // 상호작용 진행 중이라면 게이지바를 업데이트
            if (isInteracting)
            {
                gaugeProgress += Time.deltaTime; // 마우스를 누르고 있는 동안 진행
                UpdateGauge(1 - (gaugeProgress / interactingTimeRequired));  // 게이지 감소

                // 상호작용 완료 처리
                if (gaugeProgress >= interactingTimeRequired)
                {
                    FinishInteracting();
                }
            }
        }
        // 마우스 오른쪽 버튼을 뗐을 때
        else if (Input.GetMouseButtonUp(1))
        {
            StopInteracting(); // 게이지바 제거 및 초기화
        }
    }

    void StartInteracting()
    {
        if (!isInteracting)
        {
            isInteracting = true;
            isInteractingTriggered = true;

            // 게이지바 생성 및 초기화, Canvas에 추가
            gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
            gaugeBarInstance.SetActive(true); // 게이지바 활성화
            UpdateGauge(1); // 처음에는 게이지가 가득 차 있음
        }
    }

    void StopInteracting()
    {
        isInteracting = false; // 상호작용 멈춤
        isInteractingTriggered = false; // 다시 시도할 수 있도록 초기화
        gaugeProgress = 0f; // 진행도 초기화

        // 게이지바 제거
        if (gaugeBarInstance != null)
        {
            Destroy(gaugeBarInstance);  // 게이지바 제거
        }
    }

    void UpdateGauge(float value)
    {
        if (gaugeBarInstance != null)
        {
            Slider slider = gaugeBarInstance.GetComponentInChildren<Slider>();
            if (slider != null)
            {
                slider.value = value;
            }
        }
    }

    void FinishInteracting()
    {
        isInteracting = false;
        isInteractingTriggered = false;

        Destroy(gaugeBarInstance);   // 게이지바 제거

        if (itemPrefab != null)
        {
            // 아이템 생성 위치 설정
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = transform.position.y + 0.1f;

            // 아이템 생성
            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;

            // FloatingItem 스크립트 추가 및 초기 위치 설정
            FloatingItem floatingItem = newItem.AddComponent<FloatingItem>();
            floatingItem.Initialize(spawnPosition);
        }
    }

    // Gizmos를 사용해 맨손 범위를 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}


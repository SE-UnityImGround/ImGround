using UnityEngine;
using UnityEngine.UI;

public class MiningOre : MonoBehaviour
{
    public GameObject gaugeBarPrefab;       // 게이지바 프리팹
    public GameObject itemPrefab;           // 아이템 프리팹
    private GameObject gaugeBarInstance;
    private float miningProgress = 0f;
    private bool isMining = false;
    private static bool isMiningTriggered = false; // 하나의 광석만 채굴 가능
    private float miningTimeRequired = 6f;  // 6초 동안 마이닝
    public Transform canvasTransform;       // 게이지바를 붙일 Canvas의 Transform
    public Vector3 itemSize = new Vector3(0.1f, 0.1f, 0.1f);  // 드롭될 아이템의 크기
    public Transform playerTransform;       // 플레이어의 Transform
    public float allowedDistance = 2f;      // 곡괭이 범위
    public LayerMask oreLayer;              // 광석 레이어 마스크

    void Update()
    {
        // 마우스 오른쪽 버튼을 누르고 있는 동안
        if (Input.GetMouseButton(1)) // 오른쪽 마우스 버튼
        {
            // 곡괭이 범위 내에 있는 모든 광석을 감지
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, oreLayer);

            // 가장 가까운 정면 방향의 광석을 찾기 위한 변수
            Transform closestOre = null;
            float closestAngle = 45f;  // 정면 기준 허용 각도

            // 감지된 광석 중에서 플레이어가 바라보는 방향에 가까운 광석을 선택
            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToOre = (hitCollider.transform.position - playerTransform.position).normalized;
                float angleToOre = Vector3.Angle(playerTransform.forward, directionToOre);

                // 정면 각도 내에 있는 광석이면서 가장 가까운 각도의 광석을 선택
                if (angleToOre < closestAngle)
                {
                    closestAngle = angleToOre;
                    closestOre = hitCollider.transform;
                }
            }

            // 가장 가까운 광석이 현재 오브젝트이고, 아직 마이닝이 시작되지 않은 경우
            if (closestOre == transform && !isMiningTriggered)
            {
                StartMining();
            }

            // 마이닝 진행 중이라면 게이지바를 업데이트
            if (isMining)
            {
                miningProgress += Time.deltaTime; // 마우스를 누르고 있는 동안 진행
                UpdateGauge(1 - (miningProgress / miningTimeRequired));  // 게이지 감소

                // 마이닝 완료 처리
                if (miningProgress >= miningTimeRequired)
                {
                    FinishMining();
                }
            }
        }
        // 마우스 오른쪽 버튼을 뗐을 때
        else if (Input.GetMouseButtonUp(1))
        {
            StopMining(); // 게이지바 제거 및 초기화
        }
    }

    void StartMining()
    {
        if (!isMining)
        {
            isMining = true;
            isMiningTriggered = true;

            // 게이지바 생성 및 초기화, Canvas에 추가
            gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
            gaugeBarInstance.SetActive(true); // 게이지바 활성화
            UpdateGauge(1); // 처음에는 게이지가 가득 차 있음
        }
    }

    void StopMining()
    {
        isMining = false; // 마이닝을 멈춤
        isMiningTriggered = false; // 다시 시도할 수 있도록 초기화
        miningProgress = 0f; // 진행도 초기화

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

    void FinishMining()
    {
        isMining = false;
        isMiningTriggered = false;

        Destroy(gaugeBarInstance);   // 게이지바 제거
        Destroy(gameObject);         // 광석 오브젝트 제거

        if (itemPrefab != null)
        {
            // 아이템 생성 위치 설정
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = transform.position.y + 0.2f;

            // 아이템 생성
            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;

            // FloatingItem 스크립트 추가 및 초기 위치 설정
            FloatingItem floatingItem = newItem.AddComponent<FloatingItem>();
            floatingItem.Initialize(spawnPosition);
        }
    }

    // Gizmos를 사용해 곡괭이 범위를 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}

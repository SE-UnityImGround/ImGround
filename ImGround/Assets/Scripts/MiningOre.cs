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

    private Vector3 originalPosition;       // 광석의 초기 위치 저장
    private Quaternion originalRotation;    // 광석의 초기 회전 저장

    private bool isRespawning = false;      // 광석이 리스폰 중인지 확인
    private Player player;                  // Player 참조

    void Awake()
    {
        // 초기 위치와 회전 저장
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isRespawning) return; // 리스폰 중에는 작동하지 않음

        if (player.pBehavior.ToolIndex != 3)
        {
            Debug.Log($"ToolIndex is not 3. Current ToolIndex: {player.pBehavior.ToolIndex}");
            StopMining();
            return;
        }

        // 플레이어가 없거나 ToolIndex가 3이 아니면 마이닝 중단
        if (player == null || player.pBehavior == null || player.pBehavior.ToolIndex != 3)
        {
            if (isMining) // 마이닝이 진행 중일 때만 중단 처리
            {
                StopMining();
            }
            return;
        }

        // 마우스 오른쪽 버튼 누름
        if (InputManager.GetMouseButton(1))
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, oreLayer);

            Transform closestOre = null;
            float closestAngle = 45f;  // 정면 기준 허용 각도

            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToOre = (hitCollider.transform.position - playerTransform.position).normalized;
                float angleToOre = Vector3.Angle(playerTransform.forward, directionToOre);

                if (angleToOre < closestAngle)
                {
                    closestAngle = angleToOre;
                    closestOre = hitCollider.transform;
                }
            }

            if (closestOre == transform && !isMiningTriggered)
            {
                StartMining();
            }

            if (isMining)
            {
                miningProgress += Time.deltaTime;
                UpdateGauge(1 - (miningProgress / miningTimeRequired));

                if (miningProgress >= miningTimeRequired)
                {
                    FinishMining();
                }
            }
        }
        else if (InputManager.GetMouseButtonUp(1))
        {
            StopMining();
        }
    }

    void StartMining()
    {
        if (!isMining)
        {
            isMining = true;
            isMiningTriggered = true;

            gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
            gaugeBarInstance.SetActive(true);
            UpdateGauge(1);
        }
    }

    void StopMining()
    {
        isMining = false;
        isMiningTriggered = false;
        miningProgress = 0f;

        if (gaugeBarInstance != null)
        {
            Destroy(gaugeBarInstance);
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

        Destroy(gaugeBarInstance);
        gameObject.SetActive(false); // 광석 비활성화

        if (itemPrefab != null)
        {
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = playerTransform.position.y; // 플레이어의 높이를 Y축 값으로 설정

            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;

            FloatingItem floatingItem = newItem.AddComponent<FloatingItem>();
            floatingItem.Initialize(spawnPosition);
        }

        // 10초 뒤에 리스폰
        Invoke(nameof(RespawnOre), 10f);
    }

    void RespawnOre()
    {
        // 리스폰 상태로 설정
        isRespawning = true;

        // 광석의 상태 초기화
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        miningProgress = 0f; // 채굴 진행 초기화
        isMining = false;    // 채굴 상태 초기화
        isMiningTriggered = false; // 채굴 가능 상태로 초기화

        // 광석 재활성화
        gameObject.SetActive(true);

        // 리스폰 상태 종료
        isRespawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            playerTransform = player.transform; // 플레이어의 Transform 저장
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            playerTransform = null; // 플레이어의 Transform 해제
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}

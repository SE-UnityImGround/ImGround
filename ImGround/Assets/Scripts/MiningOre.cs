using UnityEngine;
using UnityEngine.UI;

public class MiningOre : MonoBehaviour
{
    public GameObject gaugeBarPrefab;
    public GameObject itemPrefab;
    private GameObject gaugeBarInstance;
    private float miningProgress = 0f;
    private bool isMining = false;
    private static bool isMiningTriggered = false;
    private float miningTimeRequired = 6f;
    public Transform canvasTransform;
    public Vector3 itemSize = new Vector3(0.1f, 0.1f, 0.1f);
    public static float allowedDistance = 3f; // 광석 감지 범위
    public LayerMask oreLayer; // 광석 감지 레이어

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isRespawning = false;
    private Player player;
    private Transform playerTransform;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 플레이어 초기화
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            player = playerTransform.GetComponent<Player>();
        }
    }

    void Update()
    {
        if (isRespawning) return;

        // 플레이어 상태나 도구 확인
        if (player == null || player.pBehavior == null || player.pBehavior.ToolIndex != 3)
        {
            StopMining();
            return;
        }

        // 우클릭 상태 확인
        if (InputManager.GetMouseButton(1))
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, oreLayer);

            Transform closestOre = null;
            float closestAngle = 45f; // 플레이어 정면 기준 최대 허용 각도
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Ore")) // 'Ore' 태그가 있는 광석만 탐지
                {
                    Vector3 directionToOre = (hitCollider.transform.position - playerTransform.position).normalized;
                    float angle = Vector3.Angle(playerTransform.forward, directionToOre);

                    if (angle < closestAngle)
                    {
                        closestAngle = angle;
                        closestOre = hitCollider.transform;
                    }
                }
            }

            // 가장 적합한 광석만 게이지 표시
            if (closestOre == transform)
            {
                if (!isMiningTriggered)
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
            else
            {
                StopMining(); // 다른 광석이 활성화된 경우 게이지 제거
            }
        }
        else // 우클릭이 해제되면 마이닝 중단
        {
            StopMining();
        }
    }

    void StartMining()
    {
        if (isMining) return; // 중복 실행 방지

        isMining = true;
        isMiningTriggered = true;

        // IsDigging 활성화
        if (player != null && player.pBehavior != null)
        {
            player.pBehavior.IsDigging = true;
        }

        gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
        gaugeBarInstance.SetActive(true);
        UpdateGauge(1);
    }

    void StopMining()
    {
        if (!isMining) return; // 이미 멈췄다면 실행하지 않음

        isMining = false;
        isMiningTriggered = false;
        miningProgress = 0f;

        // IsDigging 비활성화
        if (player != null && player.pBehavior != null)
        {
            player.pBehavior.IsDigging = false;
        }

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
        gameObject.SetActive(false);

        // 아이템 드롭 처리
        if (itemPrefab != null)
        {
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = playerTransform.position.y;

            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;
        }

        // 리스폰 처리
        Invoke(nameof(RespawnOre), 10f);
    }

    void RespawnOre()
    {
        isRespawning = true;

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        miningProgress = 0f;
        isMining = false;
        isMiningTriggered = false;

        gameObject.SetActive(true);
        isRespawning = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, allowedDistance);
    }
}

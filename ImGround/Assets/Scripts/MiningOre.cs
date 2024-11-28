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

        if (player == null || player.pBehavior == null || player.pBehavior.ToolIndex != 3)
        {
            if (isMining)
            {
                StopMining();
            }
            return;
        }

        // 오버랩스피어로 주변 광석 탐색
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
            if (InputManager.GetMouseButton(1))
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
        }
        else
        {
            StopMining(); // 다른 광석이 활성화된 경우 게이지 제거
        }

        if (InputManager.GetMouseButtonUp(1))
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
        gameObject.SetActive(false);

        if (itemPrefab != null)
        {
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = playerTransform.position.y;

            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;
        }

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

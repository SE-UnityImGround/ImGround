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
    public static float allowedDistance = 3f; // ���� ���� ����
    public LayerMask oreLayer; // ���� ���� ���̾�

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isRespawning = false;
    private Player player;
    private Transform playerTransform;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // �÷��̾� �ʱ�ȭ
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

        // �÷��̾� ���³� ���� Ȯ��
        if (player == null || player.pBehavior == null || player.pBehavior.ToolIndex != 3)
        {
            StopMining();
            return;
        }

        // ��Ŭ�� ���� Ȯ��
        if (InputManager.GetMouseButton(1))
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, oreLayer);

            Transform closestOre = null;
            float closestAngle = 45f; // �÷��̾� ���� ���� �ִ� ��� ����
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Ore")) // 'Ore' �±װ� �ִ� ������ Ž��
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

            // ���� ������ ������ ������ ǥ��
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
                StopMining(); // �ٸ� ������ Ȱ��ȭ�� ��� ������ ����
            }
        }
        else // ��Ŭ���� �����Ǹ� ���̴� �ߴ�
        {
            StopMining();
        }
    }

    void StartMining()
    {
        if (isMining) return; // �ߺ� ���� ����

        isMining = true;
        isMiningTriggered = true;

        // IsDigging Ȱ��ȭ
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
        if (!isMining) return; // �̹� ����ٸ� �������� ����

        isMining = false;
        isMiningTriggered = false;
        miningProgress = 0f;

        // IsDigging ��Ȱ��ȭ
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

        // ������ ��� ó��
        if (itemPrefab != null)
        {
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = playerTransform.position.y;

            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;
        }

        // ������ ó��
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

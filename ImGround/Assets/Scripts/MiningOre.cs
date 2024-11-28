using UnityEngine;
using UnityEngine.UI;

public class MiningOre : MonoBehaviour
{
    public GameObject gaugeBarPrefab;       // �������� ������
    public GameObject itemPrefab;           // ������ ������
    private GameObject gaugeBarInstance;
    private float miningProgress = 0f;
    private bool isMining = false;
    private static bool isMiningTriggered = false; // �ϳ��� ������ ä�� ����
    private float miningTimeRequired = 6f;  // 6�� ���� ���̴�
    public Transform canvasTransform;       // �������ٸ� ���� Canvas�� Transform
    public Vector3 itemSize = new Vector3(0.1f, 0.1f, 0.1f);  // ��ӵ� �������� ũ��
    public Transform playerTransform;       // �÷��̾��� Transform
    public float allowedDistance = 2f;      // ��� ����
    public LayerMask oreLayer;              // ���� ���̾� ����ũ

    private Vector3 originalPosition;       // ������ �ʱ� ��ġ ����
    private Quaternion originalRotation;    // ������ �ʱ� ȸ�� ����

    private bool isRespawning = false;      // ������ ������ ������ Ȯ��
    private Player player;                  // Player ����

    void Awake()
    {
        // �ʱ� ��ġ�� ȸ�� ����
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isRespawning) return; // ������ �߿��� �۵����� ����

        if (player.pBehavior.ToolIndex != 3)
        {
            Debug.Log($"ToolIndex is not 3. Current ToolIndex: {player.pBehavior.ToolIndex}");
            StopMining();
            return;
        }

        // �÷��̾ ���ų� ToolIndex�� 3�� �ƴϸ� ���̴� �ߴ�
        if (player == null || player.pBehavior == null || player.pBehavior.ToolIndex != 3)
        {
            if (isMining) // ���̴��� ���� ���� ���� �ߴ� ó��
            {
                StopMining();
            }
            return;
        }

        // ���콺 ������ ��ư ����
        if (InputManager.GetMouseButton(1))
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, oreLayer);

            Transform closestOre = null;
            float closestAngle = 45f;  // ���� ���� ��� ����

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
        gameObject.SetActive(false); // ���� ��Ȱ��ȭ

        if (itemPrefab != null)
        {
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = playerTransform.position.y; // �÷��̾��� ���̸� Y�� ������ ����

            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;

            FloatingItem floatingItem = newItem.AddComponent<FloatingItem>();
            floatingItem.Initialize(spawnPosition);
        }

        // 10�� �ڿ� ������
        Invoke(nameof(RespawnOre), 10f);
    }

    void RespawnOre()
    {
        // ������ ���·� ����
        isRespawning = true;

        // ������ ���� �ʱ�ȭ
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        miningProgress = 0f; // ä�� ���� �ʱ�ȭ
        isMining = false;    // ä�� ���� �ʱ�ȭ
        isMiningTriggered = false; // ä�� ���� ���·� �ʱ�ȭ

        // ���� ��Ȱ��ȭ
        gameObject.SetActive(true);

        // ������ ���� ����
        isRespawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            playerTransform = player.transform; // �÷��̾��� Transform ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            playerTransform = null; // �÷��̾��� Transform ����
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}

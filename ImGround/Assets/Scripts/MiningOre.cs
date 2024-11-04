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

    void Update()
    {
        // ���콺 ������ ��ư�� ������ �ִ� ����
        if (Input.GetMouseButton(1)) // ������ ���콺 ��ư
        {
            // ��� ���� ���� �ִ� ��� ������ ����
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, oreLayer);

            // ���� ����� ���� ������ ������ ã�� ���� ����
            Transform closestOre = null;
            float closestAngle = 45f;  // ���� ���� ��� ����

            // ������ ���� �߿��� �÷��̾ �ٶ󺸴� ���⿡ ����� ������ ����
            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToOre = (hitCollider.transform.position - playerTransform.position).normalized;
                float angleToOre = Vector3.Angle(playerTransform.forward, directionToOre);

                // ���� ���� ���� �ִ� �����̸鼭 ���� ����� ������ ������ ����
                if (angleToOre < closestAngle)
                {
                    closestAngle = angleToOre;
                    closestOre = hitCollider.transform;
                }
            }

            // ���� ����� ������ ���� ������Ʈ�̰�, ���� ���̴��� ���۵��� ���� ���
            if (closestOre == transform && !isMiningTriggered)
            {
                StartMining();
            }

            // ���̴� ���� ���̶�� �������ٸ� ������Ʈ
            if (isMining)
            {
                miningProgress += Time.deltaTime; // ���콺�� ������ �ִ� ���� ����
                UpdateGauge(1 - (miningProgress / miningTimeRequired));  // ������ ����

                // ���̴� �Ϸ� ó��
                if (miningProgress >= miningTimeRequired)
                {
                    FinishMining();
                }
            }
        }
        // ���콺 ������ ��ư�� ���� ��
        else if (Input.GetMouseButtonUp(1))
        {
            StopMining(); // �������� ���� �� �ʱ�ȭ
        }
    }

    void StartMining()
    {
        if (!isMining)
        {
            isMining = true;
            isMiningTriggered = true;

            // �������� ���� �� �ʱ�ȭ, Canvas�� �߰�
            gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
            gaugeBarInstance.SetActive(true); // �������� Ȱ��ȭ
            UpdateGauge(1); // ó������ �������� ���� �� ����
        }
    }

    void StopMining()
    {
        isMining = false; // ���̴��� ����
        isMiningTriggered = false; // �ٽ� �õ��� �� �ֵ��� �ʱ�ȭ
        miningProgress = 0f; // ���൵ �ʱ�ȭ

        // �������� ����
        if (gaugeBarInstance != null)
        {
            Destroy(gaugeBarInstance);  // �������� ����
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

        Destroy(gaugeBarInstance);   // �������� ����
        Destroy(gameObject);         // ���� ������Ʈ ����

        if (itemPrefab != null)
        {
            // ������ ���� ��ġ ����
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = transform.position.y + 0.2f;

            // ������ ����
            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;

            // FloatingItem ��ũ��Ʈ �߰� �� �ʱ� ��ġ ����
            FloatingItem floatingItem = newItem.AddComponent<FloatingItem>();
            floatingItem.Initialize(spawnPosition);
        }
    }

    // Gizmos�� ����� ��� ������ �ð�ȭ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}

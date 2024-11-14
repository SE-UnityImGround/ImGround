using UnityEngine;
using UnityEngine.UI;

public class InteractAnimal : MonoBehaviour
{
    public GameObject gaugeBarPrefab;       // �������� ������
    public GameObject itemPrefab;           // ������ ������
    private GameObject gaugeBarInstance;
    private float gaugeProgress = 0f;
    private bool isInteracting = false;
    private static bool isInteractingTriggered = false; // �ϳ��� �������� ��ȣ�ۿ� ����
    private float interactingTimeRequired = 4f;  // 4�� ���� ��ȣ�ۿ�
    public Transform canvasTransform;       // �������ٸ� ���� Canvas�� Transform
    public Vector3 itemSize = new Vector3(3f, 3f, 3f);  // ��ӵ� �������� ũ��
    public Transform playerTransform;       // �÷��̾��� Transform
    public float allowedDistance = 2f;      // ��ȣ�ۿ� ����
    public LayerMask animalLayer;              // ���� ���̾� ����ũ

    void Update()
    {
        // ���콺 ������ ��ư�� ������ �ִ� ����
        if (Input.GetMouseButton(1)) // ������ ���콺 ��ư
        {
            // �Ǽ� ���� ���� �ִ� ��� ������ ����
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, animalLayer);

            // ���� ����� ���� ������ ������ ã�� ���� ����
            Transform closestAnimal = null;
            float closestAngle = 45f;  // ���� ���� ��� ����

            // ������ ���� �߿��� �÷��̾ �ٶ󺸴� ���⿡ ����� ������ ����
            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToAnimal = (hitCollider.transform.position - playerTransform.position).normalized;
                float angleToAnimal = Vector3.Angle(playerTransform.forward, directionToAnimal);

                // ���� ���� ���� �ִ� �����̸鼭 ���� ����� ������ ������ ����
                if (angleToAnimal < closestAngle)
                {
                    closestAngle = angleToAnimal;
                    closestAnimal = hitCollider.transform;
                }
            }

            // ���� ����� ������ ���� ������Ʈ�̰�, ���� ��ȣ�ۿ��� ���۵��� ���� ���
            if (closestAnimal == transform && !isInteractingTriggered)
            {
                StartInteracting();
            }

            // ��ȣ�ۿ� ���� ���̶�� �������ٸ� ������Ʈ
            if (isInteracting)
            {
                gaugeProgress += Time.deltaTime; // ���콺�� ������ �ִ� ���� ����
                UpdateGauge(1 - (gaugeProgress / interactingTimeRequired));  // ������ ����

                // ��ȣ�ۿ� �Ϸ� ó��
                if (gaugeProgress >= interactingTimeRequired)
                {
                    FinishInteracting();
                }
            }
        }
        // ���콺 ������ ��ư�� ���� ��
        else if (Input.GetMouseButtonUp(1))
        {
            StopInteracting(); // �������� ���� �� �ʱ�ȭ
        }
    }

    void StartInteracting()
    {
        if (!isInteracting)
        {
            isInteracting = true;
            isInteractingTriggered = true;

            // �������� ���� �� �ʱ�ȭ, Canvas�� �߰�
            gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
            gaugeBarInstance.SetActive(true); // �������� Ȱ��ȭ
            UpdateGauge(1); // ó������ �������� ���� �� ����
        }
    }

    void StopInteracting()
    {
        isInteracting = false; // ��ȣ�ۿ� ����
        isInteractingTriggered = false; // �ٽ� �õ��� �� �ֵ��� �ʱ�ȭ
        gaugeProgress = 0f; // ���൵ �ʱ�ȭ

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

    void FinishInteracting()
    {
        isInteracting = false;
        isInteractingTriggered = false;

        Destroy(gaugeBarInstance);   // �������� ����

        if (itemPrefab != null)
        {
            // ������ ���� ��ġ ����
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = transform.position.y + 0.1f;

            // ������ ����
            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;

            // FloatingItem ��ũ��Ʈ �߰� �� �ʱ� ��ġ ����
            FloatingItem floatingItem = newItem.AddComponent<FloatingItem>();
            floatingItem.Initialize(spawnPosition);
        }
    }

    // Gizmos�� ����� �Ǽ� ������ �ð�ȭ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    public LayerMask animalLayer;           // ���� ���̾� ����ũ
    private bool hasInteracted = false;     // ��ȣ�ۿ� �Ϸ� ����
    private bool canInteract = true;        // ��ȣ�ۿ� ���� ����

    void Update()
    {
        // ��ȣ�ۿ� ���� �������� Ȯ��
        if (!canInteract) return;

        // ���콺 ������ ��ư�� ������ �ִ� ����
        if (Input.GetMouseButton(1)) // ������ ���콺 ��ư
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, animalLayer);

            Transform closestAnimal = null;
            float closestAngle = 45f;  // ���� ���� ��� ����

            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToAnimal = (hitCollider.transform.position - playerTransform.position).normalized;
                float angleToAnimal = Vector3.Angle(playerTransform.forward, directionToAnimal);

                if (angleToAnimal < closestAngle)
                {
                    closestAngle = angleToAnimal;
                    closestAnimal = hitCollider.transform;
                }
            }

            if (closestAnimal == transform && !isInteractingTriggered)
            {
                StartInteracting();
            }

            if (isInteracting && gaugeProgress < interactingTimeRequired)
            {
                gaugeProgress += Time.deltaTime; // ���콺�� ������ �ִ� ���� ����
                UpdateGauge(1 - (gaugeProgress / interactingTimeRequired));  // ������ ����

                if (gaugeProgress >= interactingTimeRequired)
                {
                    FinishInteracting();
                }
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopInteracting();
        }
    }

    void StartInteracting()
    {
        if (!isInteracting)
        {
            isInteracting = true;
            isInteractingTriggered = true;
            hasInteracted = false; // ��ȣ�ۿ� �Ϸ� ���� �ʱ�ȭ

            gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
            gaugeBarInstance.SetActive(true); // �������� Ȱ��ȭ
            UpdateGauge(1); // ó������ �������� ���� �� ����
        }
    }

    void StopInteracting()
    {
        isInteracting = false;
        isInteractingTriggered = false;
        gaugeProgress = 0f; // ���൵ �ʱ�ȭ

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

    void FinishInteracting()
    {
        if (hasInteracted) return; // �ߺ� ���� ����
        hasInteracted = true; // ��ȣ�ۿ� �Ϸ� ���� ����

        isInteracting = false;
        isInteractingTriggered = false;

        Destroy(gaugeBarInstance);

        if (itemPrefab != null)
        {
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;
            spawnPosition.y = transform.position.y + 0.1f;

            GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            newItem.transform.localScale = itemSize;

            FloatingItem floatingItem = newItem.AddComponent<FloatingItem>();
            floatingItem.Initialize(spawnPosition);
        }

        // ��ȣ�ۿ� ������ �ڷ�ƾ ����
        StartCoroutine(InteractionCooldown());
    }

    IEnumerator InteractionCooldown()
    {
        canInteract = false; // ��ȣ�ۿ� ��Ȱ��ȭ
        yield return new WaitForSeconds(3f); // 3�� ���
        canInteract = true; // ��ȣ�ۿ� Ȱ��ȭ
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}

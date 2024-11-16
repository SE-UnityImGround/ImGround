using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    public LayerMask animalLayer;           // 동물 레이어 마스크
    private bool hasInteracted = false;     // 상호작용 완료 여부
    private bool canInteract = true;        // 상호작용 가능 여부

    void Update()
    {
        // 상호작용 가능 상태인지 확인
        if (!canInteract) return;

        // 마우스 오른쪽 버튼을 누르고 있는 동안
        if (Input.GetMouseButton(1)) // 오른쪽 마우스 버튼
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, allowedDistance, animalLayer);

            Transform closestAnimal = null;
            float closestAngle = 45f;  // 정면 기준 허용 각도

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
                gaugeProgress += Time.deltaTime; // 마우스를 누르고 있는 동안 진행
                UpdateGauge(1 - (gaugeProgress / interactingTimeRequired));  // 게이지 감소

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
            hasInteracted = false; // 상호작용 완료 여부 초기화

            gaugeBarInstance = Instantiate(gaugeBarPrefab, canvasTransform);
            gaugeBarInstance.SetActive(true); // 게이지바 활성화
            UpdateGauge(1); // 처음에는 게이지가 가득 차 있음
        }
    }

    void StopInteracting()
    {
        isInteracting = false;
        isInteractingTriggered = false;
        gaugeProgress = 0f; // 진행도 초기화

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
        if (hasInteracted) return; // 중복 실행 방지
        hasInteracted = true; // 상호작용 완료 상태 설정

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

        // 상호작용 딜레이 코루틴 시작
        StartCoroutine(InteractionCooldown());
    }

    IEnumerator InteractionCooldown()
    {
        canInteract = false; // 상호작용 비활성화
        yield return new WaitForSeconds(3f); // 3초 대기
        canInteract = true; // 상호작용 활성화
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, allowedDistance);
    }
}

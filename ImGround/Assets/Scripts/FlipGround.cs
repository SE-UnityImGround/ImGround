using UnityEngine;

public class FlipGround : MonoBehaviour
{
    [SerializeField]
    private GameObject ground; // 일반 땅 자식 오브젝트
    [SerializeField]
    private GameObject farmGround; // 경작지 자식 오브젝트
    private bool isCultivated = false; // 기본 상태는 일반 땅
    private Crops crops;

    private void Start()
    {
        crops = GetComponentInChildren<Crops>(); // Crops 컴포넌트 참조 가져오기
        ground.SetActive(!isCultivated);
        farmGround.SetActive(isCultivated);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dig"))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isCultivated = !isCultivated;
        ground.SetActive(!isCultivated);
        farmGround.SetActive(isCultivated);
        if (isCultivated)
        {
            crops?.ResetCrops(isCultivated); // 경작지로 전환되면 농사 시작
        }
        else
        {
            crops?.ResetCrops(isCultivated); // 일반 땅으로 복원되면 농사 초기화
        }
        
    }
}

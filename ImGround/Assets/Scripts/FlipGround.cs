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
        if (other.CompareTag("Hoe"))
        {
            Flip();
        }
        else if (other.CompareTag("Dig"))
        {
            Restore();
        }
    }

    private void Flip()
    {
        isCultivated = true;
        ground.SetActive(false);
        farmGround.SetActive(true);
        crops?.ResetCrops(true);
    }
    private void Restore()
    {
        isCultivated = false;
        ground.SetActive(true);
        farmGround.SetActive(false);
        crops?.ResetCrops(false);
    }
}

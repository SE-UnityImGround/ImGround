using UnityEngine;

public class FlipGround : MonoBehaviour
{
    [SerializeField]
    private GameObject ground; // 일반 땅 자식 오브젝트
    [SerializeField]
    private GameObject farmGround; // 경작지 자식 오브젝트
    private bool isCultivated = false; // 기본 상태는 일반 땅
    private bool isBoxExist = false;
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
        else if (other.CompareTag("crop"))
        {
            isBoxExist = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("crop"))
        {
            isBoxExist = false;
        }
    }


    private void Flip()
    {
        // 수확작물이 존재하면 경작을 중단
        if (crops != null && crops.IsCropExist() && farmGround.activeSelf)
        {
            Debug.Log("작물이 재배중이거나 수확작물이 존재하여 경작을 시작할 수 없습니다.");
            return;
        }
        if (isBoxExist)
        {
            Debug.Log("현재 땅에 수확된 박스가 존재하여 경작을 시작할 수 없습니다.");
            return;
        }
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

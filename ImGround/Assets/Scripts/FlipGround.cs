using UnityEngine;

public class FlipGround : MonoBehaviour
{
    [SerializeField]
    private GameObject ground; // �Ϲ� �� �ڽ� ������Ʈ
    [SerializeField]
    private GameObject farmGround; // ������ �ڽ� ������Ʈ
    private bool isCultivated = false; // �⺻ ���´� �Ϲ� ��
    private Crops crops;

    private void Start()
    {
        crops = GetComponentInChildren<Crops>(); // Crops ������Ʈ ���� ��������
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

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
            crops?.ResetCrops(isCultivated); // �������� ��ȯ�Ǹ� ��� ����
        }
        else
        {
            crops?.ResetCrops(isCultivated); // �Ϲ� ������ �����Ǹ� ��� �ʱ�ȭ
        }
        
    }
}

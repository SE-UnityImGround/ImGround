using UnityEngine;

public class FlipGround : MonoBehaviour
{
    [SerializeField]
    private GameObject ground; // �Ϲ� �� �ڽ� ������Ʈ
    [SerializeField]
    private GameObject farmGround; // ������ �ڽ� ������Ʈ
    private bool isCultivated = false; // �⺻ ���´� �Ϲ� ��
    private bool isBoxExist = false;
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
        // ��Ȯ�۹��� �����ϸ� ������ �ߴ�
        if (crops != null && crops.IsCropExist() && farmGround.activeSelf)
        {
            Debug.Log("�۹��� ������̰ų� ��Ȯ�۹��� �����Ͽ� ������ ������ �� �����ϴ�.");
            return;
        }
        if (isBoxExist)
        {
            Debug.Log("���� ���� ��Ȯ�� �ڽ��� �����Ͽ� ������ ������ �� �����ϴ�.");
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

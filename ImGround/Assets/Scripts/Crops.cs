using System.Collections;
using UnityEngine;

public class Crops : MonoBehaviour
{
    public CropData cropData;
    public Transform[] spots;
    private GameObject[] currentCrops;
    [SerializeField]
    private GameObject particle;

    private void Start()
    {
        currentCrops = new GameObject[spots.Length];
        for (int i = 0; i < spots.Length; i++)
        {
            currentCrops[i] = Instantiate(cropData.growthStages[0], spots[i].position, Quaternion.identity);
            currentCrops[i].transform.SetParent(spots[i]);
            StartCoroutine(GrowCrop(i));
        }
    }

    private IEnumerator GrowCrop(int index)
    {
        int currentStage = 0;

        while (currentStage < cropData.growthStages.Length)
        {
            yield return new WaitForSeconds(cropData.growthTimePerStage[currentStage]);

            if (currentStage != 3)
                Destroy(currentCrops[index]);
            else
                break;

            currentStage++;

            if (currentStage < cropData.growthStages.Length)
            {
                currentCrops[index] = Instantiate(cropData.growthStages[currentStage], spots[index].position, Quaternion.identity);
                currentCrops[index].transform.SetParent(spots[index]);
                if (currentStage == cropData.growthStages.Length - 1)
                    AllGrown();
            }
        }
    }

    private void AllGrown()
    {
        GameObject particleInstance = Instantiate(particle, transform.position, Quaternion.Euler(-90, 0, 0));
        ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
            particleSystem.Play();
    }

    // "Harvest" �±׿� �浹 �� �۹��� ��Ѹ��� ���
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Harvest"))
        {
            ScatterCrops();
        }
    }

    private void ScatterCrops()
    {
        foreach (var crop in currentCrops)
        {
            if (crop != null)
            {
                // Rigidbody �߰�
                Rigidbody rb = crop.AddComponent<Rigidbody>();
                Collider collider = rb.GetComponent<Collider>();
                // ���� ��¦ Ƣ�� ������ ���� �ʱ� ��
                float jumpForce = 6f; // ���� Ƣ�� ������ ���� ũ��
                collider.isTrigger = false;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // ���� ��ٸ� �� ��Ѹ��� �� ����
                StartCoroutine(ApplyScatterForce(rb));
            }
        }
    }

    private IEnumerator ApplyScatterForce(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f); // ��¦ Ƣ�� ���� �ð��� ��ٸ�

        // ��Ѹ��� ���� ���� �������� ���ϱ�
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
        float scatterForce = Random.Range(2f, 5f); // ��Ѹ��� ���� ũ��
        rb.AddForce(randomDirection * scatterForce, ForceMode.Impulse);
    }

}

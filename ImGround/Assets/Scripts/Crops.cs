using System;
using System.Collections;
using UnityEngine;

public class Crops : MonoBehaviour
{
    public CropData cropData;
    [Header("Spots")]
    public Transform[] spots;
    public Transform bigSpot;

    private GameObject[] currentCrops;
    [SerializeField]
    private GameObject particle;
    private ParticleSystem particleSystem;

    private void Start()
    {
        if (!cropData.isBig)
        {
            currentCrops = new GameObject[spots.Length];
            for (int i = 0; i < spots.Length; i++)
            {
                currentCrops[i] = Instantiate(cropData.growthStages[0], spots[i].position, Quaternion.identity);
                currentCrops[i].transform.SetParent(spots[i]);
                StartCoroutine(GrowCrop(i));
            }
        }
        else if(cropData.isBig)
        {
            currentCrops = new GameObject[1];
            currentCrops[0] = Instantiate(cropData.growthStages[0], bigSpot.position, Quaternion.identity);
            currentCrops[0].transform.SetParent(bigSpot);
            StartCoroutine(GrowBigCrop());
        }
    }

    private IEnumerator GrowCrop(int index)
    {
        int currentStage = 0;

        while (currentStage < cropData.growthStages.Length)
        {
            yield return new WaitForSeconds(cropData.growthTimePerStage[currentStage]);

            if (currentStage != cropData.growthStages.Length - 1)
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

    private IEnumerator GrowBigCrop()
    {
        int currentStage = 0;

        while (currentStage < cropData.growthStages.Length)
        {
            yield return new WaitForSeconds(cropData.growthTimePerStage[currentStage]);

            if (currentStage != cropData.growthStages.Length - 1)
                Destroy(currentCrops[0]);
            else
                break;

            currentStage++;

            if (currentStage < cropData.growthStages.Length)
            {
                currentCrops[0] = Instantiate(cropData.growthStages[currentStage], bigSpot.position, Quaternion.identity);
                currentCrops[0].transform.SetParent(bigSpot);
                if (currentStage == cropData.growthStages.Length - 1)
                    AllGrown();
            }
        }
    }

    private void AllGrown(bool harvest = false)
    {
        if (!harvest)
        {
            GameObject particleInstance = Instantiate(particle, transform.position, Quaternion.Euler(-90, 0, 0));
            particleSystem = particleInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
                particleSystem.Play();
        }
        else if(harvest)
        {
            //particleSystem.Pause();
            Destroy(particle);
        }
    }

    // "Harvest" 태그와 충돌 시 작물을 흩뿌리는 기능
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Harvest"))
        {
            AllGrown(true);
            ScatterCrops();
        }
    }

    private void ScatterCrops()
    {
        foreach (var crop in currentCrops)
        {
            if (crop != null)
            {
                // Rigidbody 추가
                Rigidbody rb = crop.AddComponent<Rigidbody>();
                Collider collider = rb.GetComponent<Collider>();
                // 위로 살짝 튀어 오르기 위한 초기 힘
                float jumpForce = 3f; // 위로 튀어 오르는 힘의 크기
                
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // 조금 기다린 후 흩뿌리는 힘 적용
                StartCoroutine(ApplyScatterForce(rb, collider));
            }
        }
    }

    private IEnumerator ApplyScatterForce(Rigidbody rb, Collider collider)
    {
        yield return new WaitForSeconds(0.5f); // 살짝 튀어 오를 시간을 기다림
        collider.isTrigger = false;
        // 흩뿌리는 힘을 랜덤 방향으로 가하기
        Vector3 randomDirection = new Vector3(
            UnityEngine.Random.Range(-0.2f, 0.2f),
            UnityEngine.Random.Range(0.1f, 0.1f),
            UnityEngine.Random.Range(-0.1f, 0.1f)
        ).normalized;
        float scatterForce = UnityEngine.Random.Range(0.5f, 1f); // 흩뿌리는 힘의 크기
        rb.AddForce(randomDirection * scatterForce, ForceMode.Impulse);
    }

}

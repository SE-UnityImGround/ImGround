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
    private GameObject particleInstance;
    private GameObject cropInstance;
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
            if (particleInstance == null)
            {
                particleInstance = Instantiate(particle, transform.position, Quaternion.Euler(-90, 0, 0));
                particleSystem = particleInstance.GetComponent<ParticleSystem>();
            }
            if (particleSystem != null)
                particleSystem.Play();
        }
        else
        {
            if (particleSystem != null && particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }
    }

    // "Harvest" 태그와 충돌 시 작물을 흩뿌리는 기능
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Harvest"))
        {
            AllGrown(true);
            HarvestCrops();
        }
    }

    private void HarvestCrops()
    {
        foreach (var crop in currentCrops)
        {
            if (crop != null)
            {
                Destroy(crop);
                if(cropInstance == null)
                    cropInstance = Instantiate(cropData.cropH, bigSpot.position, Quaternion.identity);
            }
        }
    }
}

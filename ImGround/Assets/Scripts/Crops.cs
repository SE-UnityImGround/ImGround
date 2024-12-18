using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Crops : MonoBehaviour
{
    [SerializeField]
    private CropData[] cropData;
    private int cropIndex = -1;
    public int CropIndex { set {  cropIndex = value; } }
    [Header("Spots")]
    public Transform[] spots;
    public Transform bigSpot;

    private GameObject[] currentCrops;
    [SerializeField]
    private GameObject particle;
    private GameObject particleInstance;
    private GameObject cropInstance;
    private ParticleSystem particleSystem;
    private bool isGrowing = false;
    public bool IsGrowing {  get { return isGrowing; } }
    private Coroutine[] growCoroutines; // 각 작물의 성장 코루틴을 추적하는 배열
    public GameObject CropInstance {  set { cropInstance = value; } }

    private void Update()
    {
        if (cropIndex == -1 || isGrowing)
            return;
        else
        {
            if (cropData[cropIndex].isBig)
            {
                StartBigCrop();
            }
            else
            {
                StartSmallCrops();
            }
        }
    }
    private void StartSmallCrops()
    {
        currentCrops = new GameObject[spots.Length];
        growCoroutines = new Coroutine[spots.Length];
        isGrowing = true;
        for (int i = 0; i < spots.Length; i++)
        {
            currentCrops[i] = Instantiate(cropData[cropIndex].growthStages[0], spots[i].position, Quaternion.identity);
            currentCrops[i].transform.SetParent(spots[i]);
            growCoroutines[i] = StartCoroutine(GrowCrop(i));
        }
    }

    private void StartBigCrop()
    {
        currentCrops = new GameObject[1];
        growCoroutines = new Coroutine[1];
        isGrowing = true;
        currentCrops[0] = Instantiate(cropData[cropIndex].growthStages[0], bigSpot.position, Quaternion.identity);
        currentCrops[0].transform.SetParent(bigSpot);
        growCoroutines[0] = StartCoroutine(GrowBigCrop());
    }

    private IEnumerator GrowCrop(int index)
    {
        int currentStage = 0;

        while (currentStage < cropData[cropIndex].growthStages.Length)
        {
            yield return new WaitForSeconds(cropData[cropIndex].growthTimePerStage[currentStage]);

            if (currentStage != cropData[cropIndex].growthStages.Length - 1)
                Destroy(currentCrops[index]);
            else
                break;

            currentStage++;

            if (currentStage < cropData[cropIndex].growthStages.Length)
            {
                currentCrops[index] = Instantiate(cropData[cropIndex].growthStages[currentStage], spots[index].position, Quaternion.identity);
                currentCrops[index].transform.SetParent(spots[index]);
                if (currentStage == cropData[cropIndex].growthStages.Length - 1)
                    AllGrown();
            }
        }
    }

    private IEnumerator GrowBigCrop()
    {
        int currentStage = 0;

        while (currentStage < cropData[cropIndex].growthStages.Length)
        {
            yield return new WaitForSeconds(cropData[cropIndex].growthTimePerStage[currentStage]);

            if (currentStage != cropData[cropIndex].growthStages.Length - 1)
                Destroy(currentCrops[0]);
            else
                break;

            currentStage++;

            if (currentStage < cropData[cropIndex].growthStages.Length)
            {
                currentCrops[0] = Instantiate(cropData[cropIndex].growthStages[currentStage], bigSpot.position, Quaternion.identity);
                currentCrops[0].transform.SetParent(bigSpot);
                if (currentStage == cropData[cropIndex].growthStages.Length - 1)
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
            particleSystem?.Play();
        }
        else
        {
            particleSystem?.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Harvest"))
        {
            AllGrown(true);
            HarvestCrops();
            isGrowing = false;
        }
    }
    private void HarvestCrops()
    {
        foreach (var crop in currentCrops)
        {
            if (crop != null)
            {
                Destroy(crop);
                if (cropInstance == null)
                    cropInstance = Instantiate(cropData[cropIndex].cropH, bigSpot.position, Quaternion.identity);
            }
        }
        cropIndex = -1;
    }

    public bool IsCropExist()
    {
        return cropInstance != null || isGrowing; // 수확작물 프리팹이 존재하는지 확인
    }

    // 농사를 초기화하는 메서드
    public void ResetCrops(bool isCultivated)
    {
        // 1. 모든 코루틴을 중지합니다.
        StopAllCoroutines();
        // 2. 현재 작물 오브젝트가 존재하면 모두 삭제합니다.
        if (currentCrops != null)
        {
            for (int i = 0; i < currentCrops.Length; i++)
            {
                if (currentCrops[i] != null)
                {
                    Destroy(currentCrops[i]);
                    currentCrops[i] = null; // 참조 제거
                }
            }
        }

        // 3. 파티클 인스턴스가 존재하면 삭제합니다.
        if (particleInstance != null)
        {
            Destroy(particleInstance);
            particleInstance = null; // 참조 제거
        }

        // 4. 경작지 상태에서만 농사 재시작
        if (isCultivated)
        {
            cropInstance = null;
            isGrowing = false;
            cropIndex = -1;
            //if (cropData[cropIndex].isBig)
            //{
            //    StartBigCrop();
            //}
            //else
            //{
            //    StartSmallCrops();
            //}
        }
    }
}

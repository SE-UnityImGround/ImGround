using System.Collections;
using UnityEngine;

public class Crops : MonoBehaviour
{
    public CropData cropData;  // ScriptableObject에서 작물 데이터를 받음
    public Transform[] spots;  // 작물이 자랄 위치 배열
    private GameObject[] currentCrops;  // 현재 심어진 작물들의 배열

    private void Start()
    {
        currentCrops = new GameObject[spots.Length];

        // 각 spot 위치에 작물을 생성하고 성장시키기
        for (int i = 0; i < spots.Length; i++)
        {
            // 첫 번째 단계의 작물 프리팹을 각 spot에 생성
            currentCrops[i] = Instantiate(cropData.growthStages[0], spots[i].position, Quaternion.identity);
            currentCrops[i].transform.SetParent(spots[i]);

            // 각 작물에 대해 성장 코루틴 시작
            StartCoroutine(GrowCrop(i));
        }
    }

    // 각 spot에 있는 작물을 성장시키는 코루틴
    private IEnumerator GrowCrop(int index)
    {
        int currentStage = 0;

        while (currentStage < cropData.growthStages.Length)
        {
            // 각 성장 단계별 대기 시간만큼 대기
            yield return new WaitForSeconds(cropData.growthTimePerStage[currentStage]);

            // 현재 단계의 작물을 삭제하고 다음 단계의 작물을 생성
            if(currentStage != 3)
                Destroy(currentCrops[index]);
            else
            {
                break;
            }

            currentStage++;

            if (currentStage < cropData.growthStages.Length)
            {
                currentCrops[index] = Instantiate(cropData.growthStages[currentStage], spots[index].position, Quaternion.identity);
                currentCrops[index].transform.SetParent(spots[index]);
            }
        }
    }
}

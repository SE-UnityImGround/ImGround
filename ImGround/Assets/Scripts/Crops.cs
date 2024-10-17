using System.Collections;
using UnityEngine;

public class Crops : MonoBehaviour
{
    public CropData cropData;  // ScriptableObject���� �۹� �����͸� ����
    public Transform[] spots;  // �۹��� �ڶ� ��ġ �迭
    private GameObject[] currentCrops;  // ���� �ɾ��� �۹����� �迭

    private void Start()
    {
        currentCrops = new GameObject[spots.Length];

        // �� spot ��ġ�� �۹��� �����ϰ� �����Ű��
        for (int i = 0; i < spots.Length; i++)
        {
            // ù ��° �ܰ��� �۹� �������� �� spot�� ����
            currentCrops[i] = Instantiate(cropData.growthStages[0], spots[i].position, Quaternion.identity);
            currentCrops[i].transform.SetParent(spots[i]);

            // �� �۹��� ���� ���� �ڷ�ƾ ����
            StartCoroutine(GrowCrop(i));
        }
    }

    // �� spot�� �ִ� �۹��� �����Ű�� �ڷ�ƾ
    private IEnumerator GrowCrop(int index)
    {
        int currentStage = 0;

        while (currentStage < cropData.growthStages.Length)
        {
            // �� ���� �ܰ躰 ��� �ð���ŭ ���
            yield return new WaitForSeconds(cropData.growthTimePerStage[currentStage]);

            // ���� �ܰ��� �۹��� �����ϰ� ���� �ܰ��� �۹��� ����
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

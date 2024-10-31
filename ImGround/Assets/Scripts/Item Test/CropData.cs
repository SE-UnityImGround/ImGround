using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farm/Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public GameObject[] growthStages;  // 성장 단계별 모델 (1~4단계)
    public float[] growthTimePerStage; // 각 단계별 성장 시간
}
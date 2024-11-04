using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farm/Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public GameObject[] growthStages;  // 성장 단계별 모델 (1~N단계)
    public float[] growthTimePerStage; // 각 단계별 성장 시간
    [Header("All Grown Crop")]
    public GameObject cropH;    // 다 자란 작물 프리펩
    public bool isBig;
}
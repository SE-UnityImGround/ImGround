using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farm/Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public GameObject[] growthStages;  // ���� �ܰ躰 �� (1~N�ܰ�)
    public float[] growthTimePerStage; // �� �ܰ躰 ���� �ð�
    [Header("All Grown Crop")]
    public GameObject cropH;    // �� �ڶ� �۹� ������
    public bool isBig;
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farm/Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public GameObject[] growthStages;  // ���� �ܰ躰 �� (1~N�ܰ�)
    public float[] growthTimePerStage; // �� �ܰ躰 ���� �ð�
    public bool isBig;
}
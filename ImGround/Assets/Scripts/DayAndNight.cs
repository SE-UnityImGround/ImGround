using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;

    private bool isNight = false;

    [SerializeField] private float FogDensityCalc; //�Ȱ� ������ ����

    [SerializeField] private float NightFogDensity; //���� �� �Ȱ�
    private float DayFogDensity; //���� �� �Ȱ�
    private float currentFogDensity; //�Ȱ� ���

    // Start is called before the first frame update
    void Start()
    {
        DayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTime * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
        {
            isNight = true;
        }
        else //if (transform.eulerAngles.x >= 340)
        {
            isNight = false;
        }

        if (isNight)
        {
            if (currentFogDensity <= NightFogDensity)
            {
                currentFogDensity += 0.1f * FogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= DayFogDensity)
            {
                currentFogDensity -= 0.1f * FogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}

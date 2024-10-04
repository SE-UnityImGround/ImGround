using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;

    private bool isNight = false;

    [SerializeField] private float FogDensityCalc; //안개 증감량 비율

    [SerializeField] private float NightFogDensity; //밤일 때 안개
    private float DayFogDensity; //낮일 때 안개
    private float currentFogDensity; //안개 계산

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

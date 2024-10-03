/*using System.Collections;
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;

    private bool isNight = false;

    [SerializeField] private float NightFogDensityCalc; // 안개 증감량 비율
    [SerializeField] private float DayFogDensityCalc;
    [SerializeField] private float NightFogDensity; // 밤일 때 안개 밀도
    private float DayFogDensity; // 낮일 때 안개 밀도
    private float currentFogDensity; // 현재 안개 밀도

    // Start is called before the first frame update
    void Start()
    {
        DayFogDensity = RenderSettings.fogDensity;
        currentFogDensity = DayFogDensity; // 시작 시 현재 안개 밀도를 낮 시간으로 설정
    }

    // Update is called once per frame
    void Update()
    {
        // x축 회전 각도를 360도 내로 유지
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTime * Time.deltaTime);

        // 회전 각도를 제한 (360도를 넘어가면 다시 0도로 설정)
        Vector3 currentRotation = transform.eulerAngles;
        if (currentRotation.x >= 360f)
        {
            currentRotation.x = 0f;
        }
        transform.eulerAngles = currentRotation;

        // 밤과 낮을 구분 (170도 이상이면 밤, 그 이외는 낮)
        if (currentRotation.x >= 170 && currentRotation.x <= 360)
        {
            isNight = true;
        }
        else
        {
            isNight = false;
        }

        // 안개 밀도 조정
        if (isNight)
        {
            // 현재 안개 밀도가 밤 안개 밀도보다 작으면 증가
            if (currentFogDensity < NightFogDensity)
            {
                currentFogDensity += NightFogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Clamp(currentFogDensity, DayFogDensity, NightFogDensity);
            }
        }
        else
        {
            // 현재 안개 밀도가 낮 안개 밀도보다 크면 감소
            if (currentFogDensity > DayFogDensity)
            {
                currentFogDensity -= DayFogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Clamp(currentFogDensity, DayFogDensity, NightFogDensity);
            }
        }
    }
}

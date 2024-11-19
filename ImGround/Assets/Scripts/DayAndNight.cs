using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;
    static public float inGameTime = 0.0f; // 단위 : 초, 초기값 : 0초

    public bool isNight = false;

    [SerializeField] private float NightFogDensityCalc; // 안개 증감량 비율
    [SerializeField] private float DayFogDensityCalc;

    [SerializeField] private float NightFogDensity; // 밤일 때 안개 밀도

    private float DayFogDensity; // 낮일 때 안개 밀도
    private float currentFogDensity; // 현재 안개 밀도

    // Start is called before the first frame update
    void Start()
    {
        DayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        inGameTime += 24.0f * secondPerRealTime * Time.deltaTime; // secondPerRealTime = 6 으로 주어졌을 때 실제 5분 = 게임시간 12시간이 되도록 조정
        transform.rotation = Quaternion.Euler(inGameTime * 360.0f / 86400.0f, 0.0f, 0.0f); // second -> angle(degree)
        //Debug.Log(transform.rotation.eulerAngles.x);
        // 밤과 낮을 구분 (170도 이상이면 밤, 그 이외는 낮)
        if (transform.eulerAngles.x >= 170 && transform.eulerAngles.x <= 360)
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
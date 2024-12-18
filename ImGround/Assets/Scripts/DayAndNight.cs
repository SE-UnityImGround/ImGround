using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    public AudioSource[] effectSound;
    [SerializeField] private float secondPerRealTime;
    static public float inGameTime = 0.0f; // 단위 : 초, 초기값 : 0초

    public bool isNight = false;

    [SerializeField] private float NightFogDensityCalc; // 안개 증감량 비율
    [SerializeField] private float DayFogDensityCalc;

    [SerializeField] private float NightFogDensity; // 밤일 때 안개 밀도

    private float DayFogDensity; // 낮일 때 안개 밀도
    private float currentFogDensity; // 현재 안개 밀도

    private float nextEffectTime1 = 0f; // 효과음 1번 재생 시간
    private float nextEffectTime2 = 0f; // 효과음 2번 재생 시간

    public GameObject player; // 플레이어 오브젝트
    public static Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z 최소값
    public static Vector3 maxBounds = new Vector3(183, 20, 148);

    void Start()
    {
        DayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {

        inGameTime += 24.0f * secondPerRealTime * Time.deltaTime; // secondPerRealTime = 6 으로 주어졌을 때 실제 5분 = 게임시간 12시간이 되도록 조정
        transform.rotation = Quaternion.Euler(inGameTime * 360.0f / 86400.0f, 0.0f, 0.0f); // second -> angle(degree)

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
            if (currentFogDensity < NightFogDensity)
            {
                currentFogDensity += NightFogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Clamp(currentFogDensity, DayFogDensity, NightFogDensity);
            }
            
            // 밤일 때 효과음 재생
            PlayNightEffects();
        }
        else
        {
           
            if (currentFogDensity > DayFogDensity)
            {
                currentFogDensity -= DayFogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Clamp(currentFogDensity, DayFogDensity, NightFogDensity);
            }
        }
        
    }

    void PlayNightEffects()
    {
        // 현재 시간이 효과음 1번의 다음 재생 시간보다 크면 재생
        if (Time.time >= nextEffectTime1 && effectSound.Length > 0 && effectSound[0] != null)
        {
            effectSound[0].Play();
            nextEffectTime1 = Time.time + Random.Range(1f, 3f); // 1~5초 사이의 랜덤 시간 설정
        }

        // 현재 시간이 효과음 2번의 다음 재생 시간보다 크면 재생
        if (Time.time >= nextEffectTime2 && effectSound.Length > 1 && effectSound[1] != null)
        {
            effectSound[1].Play();
            nextEffectTime2 = Time.time + Random.Range(1f, 3f); // 1~5초 사이의 랜덤 시간 설정
        }
    }


}

using UnityEngine;

public class LightController : MonoBehaviour
{
    private GameObject[] lightObjects;
    public float intensityChangeSpeed = 1f; // 조절할 수 있는 속도 변수
    public float maxIntensity = 11f; // 최대 Intensity 값
    public float minIntensity = 0f; // 최소 Intensity 값

    private DayAndNight dayAndNight;  // DayAndNight 스크립트를 참조할 변수

    void Start()
    {
        lightObjects = GameObject.FindGameObjectsWithTag("lamp_light");
        dayAndNight = FindObjectOfType<DayAndNight>();  // DayAndNight 스크립트 찾기
    }

    void Update()
    {
        foreach (GameObject lightObject in lightObjects)
        {
            Light pointLight = lightObject.GetComponent<Light>();

            if (dayAndNight != null)
            {
                if (dayAndNight.isNight)
                {
                    pointLight.enabled = true;
                    pointLight.intensity = Mathf.Lerp(pointLight.intensity, maxIntensity, Time.deltaTime * intensityChangeSpeed);
                }
                else
                {
                    pointLight.intensity = Mathf.Lerp(pointLight.intensity, minIntensity, Time.deltaTime * intensityChangeSpeed);

                    if (pointLight.intensity <= minIntensity + 0.01f)
                    {
                        pointLight.enabled = false;
                    }
                }
            }
        }
    }
}

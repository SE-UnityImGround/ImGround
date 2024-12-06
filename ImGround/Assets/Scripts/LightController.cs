using UnityEngine;
using UnityEngine.SceneManagement;

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

        // 씬이 로드될 때마다 오브젝트 다시 찾기
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        foreach (GameObject lightObject in lightObjects)
        {
            if (lightObject == null)
                continue;

            Light pointLight = lightObject.GetComponent<Light>();
            if (pointLight == null)
                continue;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드되면 다시 lightObjects를 찾음
        lightObjects = GameObject.FindGameObjectsWithTag("lamp_light");
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

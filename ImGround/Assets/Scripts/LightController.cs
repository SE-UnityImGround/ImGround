using UnityEngine;

public class LightController : MonoBehaviour
{
    private GameObject[] lightObjects;
    public float intensityChangeSpeed = 1f; // ������ �� �ִ� �ӵ� ����
    public float maxIntensity = 11f; // �ִ� Intensity ��
    public float minIntensity = 0f; // �ּ� Intensity ��

    private DayAndNight dayAndNight;  // DayAndNight ��ũ��Ʈ�� ������ ����

    void Start()
    {
        lightObjects = GameObject.FindGameObjectsWithTag("lamp_light");
        dayAndNight = FindObjectOfType<DayAndNight>();  // DayAndNight ��ũ��Ʈ ã��
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

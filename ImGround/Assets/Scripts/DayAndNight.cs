using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;
    static public float inGameTime = 0.0f; // ���� : ��, �ʱⰪ : 0��

    public bool isNight = false;

    [SerializeField] private float NightFogDensityCalc; // �Ȱ� ������ ����
    [SerializeField] private float DayFogDensityCalc;

    [SerializeField] private float NightFogDensity; // ���� �� �Ȱ� �е�

    private float DayFogDensity; // ���� �� �Ȱ� �е�
    private float currentFogDensity; // ���� �Ȱ� �е�

    // Start is called before the first frame update
    void Start()
    {
        DayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogFormat("���� �ð� {0} �����ð� {1}", inGameTime/3600, inGameTime/ 24.0f / secondPerRealTime);

        inGameTime += 24.0f * secondPerRealTime * Time.deltaTime;
        transform.rotation = Quaternion.Euler(inGameTime * 360.0f / 86400.0f, 0.0f, 0.0f); // second -> angle(degree)
        //Debug.Log(transform.rotation.eulerAngles.x);
        // ��� ���� ���� (170�� �̻��̸� ��, �� �ܴ̿� ��)
        if (transform.eulerAngles.x >= 170 && transform.eulerAngles.x <= 360)
        {
            isNight = true;
        }
        else
        {
            isNight = false;
        }

        // �Ȱ� �е� ����
        if (isNight)
        {
            // ���� �Ȱ� �е��� �� �Ȱ� �е����� ������ ����
            if (currentFogDensity < NightFogDensity)
            {
                currentFogDensity += NightFogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Clamp(currentFogDensity, DayFogDensity, NightFogDensity);
            }
        }
        else
        {
            // ���� �Ȱ� �е��� �� �Ȱ� �е����� ũ�� ����
            if (currentFogDensity > DayFogDensity)
            {
                currentFogDensity -= DayFogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Clamp(currentFogDensity, DayFogDensity, NightFogDensity);
            }
        }
    }
}
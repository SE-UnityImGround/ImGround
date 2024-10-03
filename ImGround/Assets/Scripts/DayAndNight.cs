using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;

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

        transform.Rotate(0.1f * secondPerRealTime * Time.deltaTime, 0f, 0f);
        Debug.Log(transform.rotation.eulerAngles.x);
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
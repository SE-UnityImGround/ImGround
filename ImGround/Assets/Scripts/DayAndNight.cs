/*using System.Collections;
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;

    private bool isNight = false;

    [SerializeField] private float NightFogDensityCalc; // �Ȱ� ������ ����
    [SerializeField] private float DayFogDensityCalc;
    [SerializeField] private float NightFogDensity; // ���� �� �Ȱ� �е�
    private float DayFogDensity; // ���� �� �Ȱ� �е�
    private float currentFogDensity; // ���� �Ȱ� �е�

    // Start is called before the first frame update
    void Start()
    {
        DayFogDensity = RenderSettings.fogDensity;
        currentFogDensity = DayFogDensity; // ���� �� ���� �Ȱ� �е��� �� �ð����� ����
    }

    // Update is called once per frame
    void Update()
    {
        // x�� ȸ�� ������ 360�� ���� ����
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTime * Time.deltaTime);

        // ȸ�� ������ ���� (360���� �Ѿ�� �ٽ� 0���� ����)
        Vector3 currentRotation = transform.eulerAngles;
        if (currentRotation.x >= 360f)
        {
            currentRotation.x = 0f;
        }
        transform.eulerAngles = currentRotation;

        // ��� ���� ���� (170�� �̻��̸� ��, �� �ܴ̿� ��)
        if (currentRotation.x >= 170 && currentRotation.x <= 360)
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

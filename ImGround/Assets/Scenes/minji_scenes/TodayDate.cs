using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TodayDate : MonoBehaviour
{
    [SerializeField] Text Text_Day;

    void Update()
    {
        Text_Day.text = DateTime.Now.ToString("yyyy³â MM¿ù ddÀÏ\ndddd");
    }
}

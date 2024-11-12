using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DateDisplayer : MonoBehaviour
{
    [SerializeField]
    private Text displayText;

    private DateTime startDate;
    private DateTimeFormatInfo dateInfo = new DateTimeFormatInfo();

    // Start is called before the first frame update
    void Start()
    {
        DateTime current = DateTime.Now;
        startDate = 
            new DateTime(
                current.Year,
                current.Month,
                current.Day);
    }

    // Update is called once per frame
    void Update()
    {
        DateTime displayTime = startDate.AddSeconds(DayAndNight.inGameTime);
        displayText.text = 
            string.Format(
                "{0} {1}\n{2}", 
                displayTime.Day, 
                dateInfo.MonthNames[displayTime.Month - 1], 
                displayTime.Year);
    }
}

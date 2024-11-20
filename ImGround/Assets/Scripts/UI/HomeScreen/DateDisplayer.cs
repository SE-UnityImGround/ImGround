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

    public void initialize()
    {
        if (displayText == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, nameof(displayText));
            return;
        }

        DateTime current = DateTime.Now;
        startDate =
            new DateTime(
                current.Year,
                current.Month,
                current.Day);
    }

    public void update()
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

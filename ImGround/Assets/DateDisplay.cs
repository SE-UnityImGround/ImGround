using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스 추가
using System;

public class DateDisplay : MonoBehaviour
{
    public Text dateText; // UI Text 컴포넌트를 연결할 변수

    void Start()
    {
        // 현재 날짜와 시간을 포맷하여 문자열로 변환 (영어로)
        string currentDate = DateTime.Now.ToString("dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

        // Text 컴포넌트에 날짜 문자열 설정
        dateText.text = currentDate;
    }
}

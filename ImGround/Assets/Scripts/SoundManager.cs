using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource []bgm;
    protected bool isNight = false;
    private DayAndNight dayAndNightScript;

    // Start is called before the first frame update
    void Start()
    {
        bgm[0].Play();

        if (!isNight)
        {
            bgm[1].Play();
        }
        else if (isNight){
            bgm[1].Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight; // isNight 상태 동기화
        }
    }
}

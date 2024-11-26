/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] bgm; // BGM 배열
    private bool isNight = false;
    private bool previousIsNight = false; // 이전 상태 저장
    public DayAndNight dayAndNightScript; // DayAndNight 스크립트 참조

    public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간
    private float maxVolume1 = 0.5f; // 1번 효과음의 최대 볼륨
    private float maxVolume2 = 0.5f; // 2번 효과음의 최대 볼륨 (필요시 조정)

    // Start is called before the first frame update
    void Start()
    {

        bgm[5].Play();

        if (!isNight)
        {
            StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1)); // 1번 효과음
            StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2)); // 2번 효과음
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight; // isNight 상태 동기화
        }

        // 밤 상태 처리
        if (isNight)
        {
            if (!bgm[2].isPlaying) bgm[2].Play(); // 3번 효과음 지속 재생
            if (!bgm[3].isPlaying) bgm[3].Play(); // 4번 효과음 밤에만 재생

            // 낮 배경음 페이드 아웃
            if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
            if (bgm[1].isPlaying) StartCoroutine(FadeOut(bgm[1], fadeDuration));
        }
        else
        {
            if (bgm[2].isPlaying) bgm[2].Stop(); // 밤 효과음 중지
            if (bgm[3].isPlaying) bgm[3].Stop(); // 4번 효과음 낮에 중지

            if (previousIsNight && !bgm[4].isPlaying)
            {
                bgm[4].Play();
            }

            // 낮 배경음 페이드 인
            if (!bgm[0].isPlaying) StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1));
            if (!bgm[1].isPlaying) StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2));
        }

        // 이전 상태 업데이트
        previousIsNight = isNight;
    }

    // 페이드 인 (최대 볼륨 제한)
    private IEnumerator FadeInWithLimit(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float startVolume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // 페이드 아웃
    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] bgm; // BGM 배열
    private bool isNight = false;
    private bool previousIsNight = false; // 이전 상태 저장
    public DayAndNight dayAndNightScript; // DayAndNight 스크립트 참조

    public GameObject player; // 플레이어 오브젝트
    public Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z 최소값
    public Vector3 maxBounds = new Vector3(183, 20, 148);  // x, y, z 최대값

    public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간
    private float maxVolume1 = 0.5f; // 1번 효과음의 최대 볼륨
    private float maxVolume2 = 0.5f; // 2번 효과음의 최대 볼륨 (필요시 조정)

    // Start is called before the first frame update
    void Start()
    {
        bgm[5].Play();

        if (!isNight && IsPlayerWithinBounds())
        {
            StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1)); // 1번 효과음
            StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2)); // 2번 효과음
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            bool isWithinBounds = IsPlayerWithinBounds();

            Debug.Log($"Player Position: {playerPosition}, IsWithinBounds: {isWithinBounds}");
        }

        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight;
        }

        if (isNight && IsPlayerWithinBounds())
        {
            if (!bgm[2].isPlaying) bgm[2].Play();
            if (!bgm[3].isPlaying) bgm[3].Play();

            if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
            if (bgm[1].isPlaying) StartCoroutine(FadeOut(bgm[1], fadeDuration));
        }
        else
        {
            if (bgm[2].isPlaying) bgm[2].Stop();
            if (bgm[3].isPlaying) bgm[3].Stop();

            if (previousIsNight && !bgm[4].isPlaying && IsPlayerWithinBounds())
            {
                bgm[4].Play();
            }

            if (!bgm[0].isPlaying && IsPlayerWithinBounds()) StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1));
            if (!bgm[1].isPlaying && IsPlayerWithinBounds()) StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2));
        }

        previousIsNight = isNight;
    }



    // 플레이어가 지정된 범위 안에 있는지 확인
    private bool IsPlayerWithinBounds()
    {
        if (player == null) return false;

        Vector3 playerPosition = player.transform.position;
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.y >= minBounds.y && playerPosition.y <= maxBounds.y &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }

    // 페이드 인 (최대 볼륨 제한)
    private IEnumerator FadeInWithLimit(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float startVolume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // 페이드 아웃
    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
*/


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundController[] soundControllers; // SoundController 배열
    private bool isNight = false;
    private bool previousIsNight = false; // 이전 상태 저장
    public DayAndNight dayAndNightScript; // DayAndNight 스크립트 참조

    public GameObject player; // 플레이어 오브젝트
    public Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z 최소값
    public Vector3 maxBounds = new Vector3(183, 20, 148);  // x, y, z 최대값

    public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            bool isWithinBounds = IsPlayerWithinBounds();

            // 디버그 출력
            Debug.Log($"Update: IsWithinBounds={isWithinBounds}, IsNight={isNight}");

            // 범위와 밤/낮 상태에 따라 볼륨 제어
            foreach (SoundController controller in soundControllers)
            {
                if (isNight && isWithinBounds)
                {
                    controller.setVolume(1.0f); // 밤 + 범위 안에서 음악 활성화
                }
                else
                {
                    controller.setVolume(0.0f); // 범위 밖이거나 낮이면 음악 비활성화
                }
            }
        }

        if (dayAndNightScript != null)
        {
            isNight = dayAndNightScript.isNight;
        }
    }


    // 밤 음악 활성화
    private void ActivateNightSounds()
    {
        foreach (SoundController controller in soundControllers)
        {
            // 밤일 때 특정 SoundController의 음악 활성화
            controller.setVolume(1.0f);
        }
    }

    // 낮 음악 활성화
    private void ActivateDaySounds()
    {
        foreach (SoundController controller in soundControllers)
        {
            // 낮일 때 특정 SoundController의 음악 활성화
            controller.setVolume(1.0f);
        }
    }

    // 플레이어가 지정된 범위 안에 있는지 확인
    private bool IsPlayerWithinBounds()
    {
        if (player == null) return false;

        Vector3 playerPosition = player.transform.position;
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.y >= minBounds.y && playerPosition.y <= maxBounds.y &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] bgm; // BGM 배열
    private bool isNight = false;
    private bool previousIsNight = false; // 이전 상태 저장
    public DayAndNight dayAndNightScript; // DayAndNight 스크립트 참조

    public static GameObject player; // 플레이어 오브젝트
    public static Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z 최소값
    public static Vector3 maxBounds = new Vector3(183, 20, 148);

    public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간
    private float maxVolume1 = 0.5f; // 1번 효과음의 최대 볼륨
    private float maxVolume2 = 0.5f; // 2번 효과음의 최대 볼륨 (필요시 조정)
    private bool isWithinBounds = IsPlayerWithinBounds();
    
    // Start is called before the first frame update
    void Start()
    {
        bgm[5].Play();

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && isWithinBounds)
        {   
            if (!isNight)
            {
                if (bgm[6].isPlaying) StartCoroutine(FadeOut(bgm[6], fadeDuration));
                StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1)); // 1번 효과음
                StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2)); // 2번 효과음
            }
            if (dayAndNightScript != null)
            {
                isNight = dayAndNightScript.isNight; // isNight 상태 동기화
            }

            // 밤 상태 처리
            if (isNight)
            {
                if (!bgm[2].isPlaying) bgm[2].Play(); // 3번 효과음 지속 재생
                if (!bgm[3].isPlaying) bgm[3].Play(); // 4번 효과음 밤에만 재생

                // 낮 배경음 페이드 아웃
                if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
                if (bgm[1].isPlaying) StartCoroutine(FadeOut(bgm[1], fadeDuration));
            }
            else
            {
                if (bgm[2].isPlaying) bgm[2].Stop(); // 밤 효과음 중지
                if (bgm[3].isPlaying) bgm[3].Stop(); // 4번 효과음 낮에 중지

                if (previousIsNight && !bgm[4].isPlaying)
                {
                    bgm[4].Play();
                }

                // 낮 배경음 페이드 인
                if (!bgm[0].isPlaying) StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1));
                if (!bgm[1].isPlaying) StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2));
            }

            // 이전 상태 업데이트
            previousIsNight = isNight;
        }
        else if (player != null && !isWithinBounds)
        {
            if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
            StartCoroutine(FadeInWithLimit(bgm[6], fadeDuration, maxVolume1));
        }
    }

    // 페이드 인 (최대 볼륨 제한)
    private IEnumerator FadeInWithLimit(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float startVolume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // 페이드 아웃
    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }
    private static bool IsPlayerWithinBounds()
    {
        if (player == null) return false;

        Vector3 playerPosition = player.transform.position;
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.y >= minBounds.y && playerPosition.y <= maxBounds.y &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] bgm; // BGM 배열
    private bool isNight = false;
    private bool previousIsNight = false; // 이전 상태 저장
    public DayAndNight dayAndNightScript; // DayAndNight 스크립트 참조

    public GameObject player; // 플레이어 오브젝트
    public static Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z 최소값
    public static Vector3 maxBounds = new Vector3(183, 20, 148);

    public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간
    private float maxVolume1 = 0.5f; // 1번 효과음의 최대 볼륨
    private float maxVolume2 = 0.5f; // 2번 효과음의 최대 볼륨 (필요시 조정)

    // Update is called once per frame
    void Update()
    {
        

        bool isWithinBounds = IsPlayerWithinBounds();

        if (isWithinBounds)
        {
            if (!bgm[5].isPlaying) bgm[5].Play();
            if (bgm[6].isPlaying) bgm[6].Stop();

            if (dayAndNightScript != null)
            {
                isNight = dayAndNightScript.isNight; // isNight 상태 동기화
            }

            
            if (!isNight)
            {
                if (bgm[2].isPlaying) bgm[2].Stop(); // 밤 효과음 중지
                if (bgm[3].isPlaying) bgm[3].Stop(); // 4번 효과음 낮에 중지

                if (previousIsNight && !bgm[4].isPlaying)
                {
                    bgm[4].Play();
                }

                // 낮 배경음 페이드 인
                if (!bgm[0].isPlaying) StartCoroutine(FadeInWithLimit(bgm[0], fadeDuration, maxVolume1));
                if (!bgm[1].isPlaying) StartCoroutine(FadeInWithLimit(bgm[1], fadeDuration, maxVolume2));
            }

            // 밤 상태 처리
            if (isNight)
            {
                if (!bgm[2].isPlaying) bgm[2].Play(); // 3번 효과음 지속 재생
                if (!bgm[3].isPlaying) bgm[3].Play(); // 4번 효과음 밤에만 재생

                // 낮 배경음 페이드 아웃
                if (bgm[0].isPlaying) StartCoroutine(FadeOut(bgm[0], fadeDuration));
                if (bgm[1].isPlaying) StartCoroutine(FadeOut(bgm[1], fadeDuration));
            }
            // 이전 상태 업데이트
            previousIsNight = isNight;
            Debug.Log("마을");
        }
        else
        {
            if (!bgm[6].isPlaying) StartCoroutine(FadeInWithLimit(bgm[6], fadeDuration, maxVolume1));
            if (bgm[0].isPlaying) bgm[0].Stop();
            if (bgm[1].isPlaying) bgm[1].Stop();
            if (bgm[2].isPlaying) bgm[2].Stop();
            if (bgm[3].isPlaying) bgm[3].Stop();
            if (bgm[4].isPlaying) bgm[4].Stop();
            if (bgm[5].isPlaying) bgm[5].Stop();
            Debug.Log("동굴");
        }
    }

    // 페이드 인 (최대 볼륨 제한)
    private IEnumerator FadeInWithLimit(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float startVolume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // 페이드 아웃
    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }
    private bool IsPlayerWithinBounds()
    {
        if (player == null) return false;

        Vector3 playerPosition = player.transform.position;
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.y >= minBounds.y && playerPosition.y <= maxBounds.y &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }
}


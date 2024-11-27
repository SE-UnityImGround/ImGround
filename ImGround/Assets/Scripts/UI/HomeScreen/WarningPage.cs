using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPage : MonoBehaviour
{
    [SerializeField]
    private float SHOW_DURATION = 1.0f;
    [SerializeField]
    private float fade_duration = 0.2f;
    private float timerStd;
    private CanvasRenderer uiRenderer;

    // Start is called before the first frame update
    void Start()
    {
        uiRenderer = gameObject.GetComponent<CanvasRenderer>();
        uiRenderer.SetAlpha(0.0f);
        gameObject.SetActive(false);

        WarningManager.assignWarningStartHandler(show);
    }

    // Update is called once per frame
    void Update()
    {
        setAlpha();
    }

    public void show()
    {
        timerStd = Time.time;
        uiRenderer.SetAlpha(0.0f);
        gameObject.SetActive(true);
    }

    private void setAlpha()
    {
        float time = Time.time - timerStd;
        
        if (time > SHOW_DURATION)
        {
            uiRenderer.SetAlpha(0.0f);
            gameObject.SetActive(false);
            return;
        }

        if (time > SHOW_DURATION - fade_duration)
        {
            uiRenderer.SetAlpha(1.0f * ((SHOW_DURATION - time) / fade_duration));
            return;
        }
        
        if (time > fade_duration)
        {
            uiRenderer.SetAlpha(1.0f);
            return;
        }

        uiRenderer.SetAlpha(1.0f * (time / fade_duration));
        return;
    }
}

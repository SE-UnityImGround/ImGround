using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;

    public Button soundButton;
    public Text soundButtonText; // sound ��ư�� text ������Ʈ ����
    
    private bool isSoundOn = true; // �ʱ� ���� ����


    private void Start()
    {
        defaultScale = buttonScale.localScale;
        UpdateSoundButtonText();
    }

    public void OnBtnClick()
    {
        switch(currentType)
        {
            case BTNType.New:
                Sceneload.LoadSceneHandle("Eunju_PlayScene", 0);
                break;
            case BTNType.Continue:
                Sceneload.LoadSceneHandle("Eunju_PlayScene", 1);
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Sound:
                ToggleSound();
                break;
            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case BTNType.Quit:
                Application.Quit();
                Debug.Log("������");
                break;
        }
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        UpdateSoundButtonText();
        if (isSoundOn)
        {
            Debug.Log("����ON");
        }
        else
        {
            Debug.Log("����OFF");
        }
    }

    private void UpdateSoundButtonText()
    {
        if (soundButtonText != null)
        {
            soundButtonText.text = isSoundOn ? "Sound ON" : "Sound OFF";
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale*1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}

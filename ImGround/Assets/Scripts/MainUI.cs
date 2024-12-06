using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private GameObject MainView;
    [SerializeField]
    private GameObject Main_SettingView;

    [SerializeField]
    private GameObject LoadingView;

    [SerializeField]
    private string GameSceneName;

    private void Start()
    {
        changeView(ViewMode.MAIN);
    }

    private void Update()
    {
        
    }

    private void changeView(ViewMode mode)
    {
        MainView.SetActive(mode == ViewMode.MAIN
            || mode == ViewMode.SETTING);
        Main_SettingView.SetActive(mode == ViewMode.SETTING);
        LoadingView.SetActive(mode == ViewMode.LOADING);
    }

    public void onButtonClick(BtnType button)
    {
        UISoundManager.playUiSound(UISoundObject.MAIN_UI_CLICK);
        switch (button.currentType)
        {
            case BTNType.New:
                changeView(ViewMode.LOADING);
                LoadingView.GetComponent<Sceneload>().LoadSceneHandle(GameSceneName, 0);
                break;
            case BTNType.Continue:
                SaveManager.setLoadState();
                changeView(ViewMode.LOADING);
                LoadingView.GetComponent<Sceneload>().LoadSceneHandle(GameSceneName, 1);
                break;
            case BTNType.Option:
                changeView(ViewMode.SETTING);
                break;
            case BTNType.Sound:
                Debug.LogError("������ �ʴ� ��ưŸ��");
                break;
            case BTNType.Back:
                changeView(ViewMode.MAIN);
                break;
            case BTNType.Quit:
                Debug.Log("������");
                Application.Quit();
                break;
        }
    }
}

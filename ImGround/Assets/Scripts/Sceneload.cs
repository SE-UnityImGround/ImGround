using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Sceneload : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    public static string loadScene;
    public static int loadType;

    private void Start()
    {
        //�񵿱�ε�(Scene �ҷ��� �� �� ���߰� �ٸ� �۾� ����)�� ����� ���� �ڷ�ƾ ���

        StartCoroutine(LoadScene());
    }

    public static void LoadSceneHandle(string _name, int _loadType)
    {
        loadScene = _name;
        loadType = _loadType;
        SceneManager.LoadScene("Eunju_LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Eunju_PlayScene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            if (loadType == 0)
                Debug.Log("������");
            else if (loadType == 1)
                Debug.Log("�����");

            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }


            if (progressbar.value >= 1f)
            {
                loadtext.text = "Press Space Bar";
            }

            if (Input.GetKeyDown(KeyCode.Space) && progressbar.value > 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }

    }
}

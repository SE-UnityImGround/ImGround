using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private InGameViewBehavior inGameUI;

    // Start is called before the first frame update
    void Start()
    {
        if (inGameUI == null)
        {
            Debug.LogError(nameof(PlayerUIController) + "내에 " + nameof(inGameUI) + "가 등록되지 않았습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            inGameUI.toggleView(InGameViewMode.MANUFACT);
        }
    }
}

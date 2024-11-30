using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    public InGameViewBehavior inGameViewBehavior;
    [SerializeField]
    private bool hideOrDisplay;
    [SerializeField]
    private InGameViewMode LinkedView;

    public void onClick()
    {
        UISoundManager.onPlayUiSound(UISoundObject.INGAME_UI_CLICK);
        if (hideOrDisplay)
            inGameViewBehavior.hideView(LinkedView);
        else
            inGameViewBehavior.displayView(LinkedView);
    }
}

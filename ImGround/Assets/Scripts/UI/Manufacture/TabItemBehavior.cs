using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemBehavior : MonoBehaviour
{
    private static Color32 COLOR_SELECTED = new Color32(255, 213, 0, 255);
    private static Color32 COLOR_UNSELECTED = new Color32(255, 248, 188, 159);

    [SerializeField]
    private ManufactListBehavior manufactUi;
    [SerializeField]
    private ManufactCategory category;
    [SerializeField]
    private Image buttonImage;

    public void onClick()
    {
        manufactUi.onTabButtonClick(category);
    }

    public void updateSelection(ManufactCategory selected)
    {
        if (category == selected)
        {
            buttonImage.color = COLOR_SELECTED;
        }
        else
        {
            buttonImage.color = COLOR_UNSELECTED;
        }
    }
}

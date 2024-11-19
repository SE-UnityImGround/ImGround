using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManufactBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject inputItemDisplayer;
    [SerializeField]
    private GameObject outputItemDisplayer;
    [SerializeField]
    private GameObject itemDisplayPrefab;

    private ManufactInfo manufactInfo;

    public delegate void doMakeClick(ManufactInfo manufactInfo);
    public doMakeClick doMakeClickHandler;

    public void doMakeClickTrigger()
    {
        doMakeClickHandler?.Invoke(manufactInfo);
    }

    public void initialize(ManufactInfo manufactInfo)
    {
        if (inputItemDisplayer == null)
        {
            Debug.LogError(nameof(inputItemDisplayer) + " 없음.");
        }
        if (outputItemDisplayer == null)
        {
            Debug.LogError(nameof(outputItemDisplayer) + " 없음.");
        }
        if (itemDisplayPrefab == null)
        {
            Debug.LogError(nameof(itemDisplayPrefab) + " 없음.");
        }

        this.manufactInfo = manufactInfo;
        foreach(ItemBundle item in manufactInfo.inputItems)
        {
            addItemIcon(item, inputItemDisplayer.transform);
        }
        addItemIcon(manufactInfo.outputItem, outputItemDisplayer.transform);
    }

    private void addItemIcon(ItemBundle item, Transform parent)
    {
        Instantiate(itemDisplayPrefab, parent).GetComponent<ManufactItemIconBehavior>().initialize(item);
    }
}

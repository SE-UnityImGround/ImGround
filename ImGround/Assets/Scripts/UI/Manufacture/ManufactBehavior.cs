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

    private List<ManufactItemIconBehavior> manufactIcons = new List<ManufactItemIconBehavior>();
    private ManufactInfo manufactInfo;

    public delegate void doMakeClick(ManufactInfo manufactInfo);
    public doMakeClick doMakeClickHandler;

    public void doMakeClickTrigger()
    {
        UISoundManager.playUiSound(UISoundObject.TRADE);
        doMakeClickHandler?.Invoke(manufactInfo);
    }

    public void initialize(ManufactInfo manufactInfo)
    {
        if (inputItemDisplayer == null)
        {
            Debug.LogError(nameof(inputItemDisplayer) + " ����.");
        }
        if (outputItemDisplayer == null)
        {
            Debug.LogError(nameof(outputItemDisplayer) + " ����.");
        }
        if (itemDisplayPrefab == null)
        {
            Debug.LogError(nameof(itemDisplayPrefab) + " ����.");
        }

        this.manufactInfo = manufactInfo;
        foreach(ItemBundle item in manufactInfo.inputItems)
        {
            ManufactItemIconBehavior icon =
                Instantiate(itemDisplayPrefab, inputItemDisplayer.transform)
                .GetComponent<ManufactItemIconBehavior>();
            icon.initialize(item, true);
            manufactIcons.Add(icon);
        }
        Instantiate(itemDisplayPrefab, outputItemDisplayer.transform)
            .GetComponent<ManufactItemIconBehavior>().initialize(manufactInfo.outputItem, false);
    }

    /// <summary>
    /// ������ �������� ������ ������Ʈ�մϴ�. null�� �κ��丮�� ������� ǥ���մϴ�.
    /// </summary>
    /// <param name="inventoryInfo"></param>
    public void updateInfo(Dictionary<ItemIdEnum, int> inventoryInfo)
    {
        foreach (ManufactItemIconBehavior icon in manufactIcons)
        {
            ItemIdEnum itemid = icon.getItemId();
            if (inventoryInfo != null && inventoryInfo.ContainsKey(itemid))
            {
                icon.updateAmount(inventoryInfo[itemid]);
            }
            else
            {
                icon.updateAmount(0);
            }
        }
    }
}

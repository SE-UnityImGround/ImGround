using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    private InventoryBehavior myBag = null;
    private QuestListBehavior myQuest;

    // Start is called before the first frame update
    void Start()
    {
        InventoryBehavior[] inventoryUIGameObjects = FindObjectsOfType<InventoryBehavior>(true);
        if (inventoryUIGameObjects.Length == 0)
        {
            Debug.LogError("�κ��丮 UI ���ӿ�����Ʈ�� ���� �����ϴ�!");
        }
        else if (inventoryUIGameObjects.Length > 1)
        {
            Debug.LogError("�κ��丮 UI ���ӿ�����Ʈ�� �ʹ� �����ϴ�! : " + inventoryUIGameObjects.Length + "��");
        }
        else
        {
            myBag = inventoryUIGameObjects[0];
        }

        QuestListBehavior[] questUIGameObjects = FindObjectsOfType<QuestListBehavior>(true);
        if (questUIGameObjects.Length == 0)
        {
            Debug.LogError("����Ʈ UI ���ӿ�����Ʈ�� ���� �����ϴ�!");
        }
        else if (questUIGameObjects.Length > 1)
        {
            Debug.LogError("����Ʈ UI ���ӿ�����Ʈ�� �ʹ� �����ϴ�! : " + questUIGameObjects.Length + "��");
        }
        else
        {
            myQuest = questUIGameObjects[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            // �׽�Ʈ : 
            myBag.addItem(new Item(ItemIdEnum.TEST_NULL_ITEM, 2));
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            myBag.setActive(!myBag.getActive());
        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            myQuest.setActive(!myQuest.getActive());
        }
    }
}

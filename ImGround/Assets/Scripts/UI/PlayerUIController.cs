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
            Debug.LogError("인벤토리 UI 게임오브젝트가 씬에 없습니다!");
        }
        else if (inventoryUIGameObjects.Length > 1)
        {
            Debug.LogError("인벤토리 UI 게임오브젝트가 너무 많습니다! : " + inventoryUIGameObjects.Length + "개");
        }
        else
        {
            myBag = inventoryUIGameObjects[0];
        }

        QuestListBehavior[] questUIGameObjects = FindObjectsOfType<QuestListBehavior>(true);
        if (questUIGameObjects.Length == 0)
        {
            Debug.LogError("퀘스트 UI 게임오브젝트가 씬에 없습니다!");
        }
        else if (questUIGameObjects.Length > 1)
        {
            Debug.LogError("퀘스트 UI 게임오브젝트가 너무 많습니다! : " + questUIGameObjects.Length + "개");
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
            // 테스트 : 
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

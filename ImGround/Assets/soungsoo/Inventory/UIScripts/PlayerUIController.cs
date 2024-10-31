using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    public InventoryBehavior myBag;
    public QuestListBehavior myQuest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            myBag.testAddItem();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            myBag.setActive(!myBag.getActive());
        }
    }
}

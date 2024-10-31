using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        flying = true;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Vector3 targetPosition = navAgent.transform.position;
        targetPosition.y = 2;
        transform.position = targetPosition;
    }
}

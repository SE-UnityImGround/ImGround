using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    public InGameViewBehavior inGameViewBehavior;
    [SerializeField]
    private InGameViewMode LinkedView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        inGameViewBehavior.displayView(LinkedView);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : UIBehavior
{
    [SerializeField]
    private DateDisplayer dateDisplayer;
    [SerializeField]
    private ExpDisplayer expDisplayer;
    [SerializeField]
    private LifeDisplayer lifeDisplayer;

    private float lifeRatio = 1.0f;
    private float expRatio = 0.0f;
    private int level = 0;

    private void checkValue(object v, string name)
    {
        if (v == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, name);
            return;
        }
    }

    public override void initialize()
    {
        checkValue(dateDisplayer, nameof(dateDisplayer));
        checkValue(expDisplayer, nameof(expDisplayer));
        checkValue(lifeDisplayer, nameof(lifeDisplayer));

        dateDisplayer.initialize();
        expDisplayer.initialize();
        lifeDisplayer.initialize();
    }

    // Update is called once per frame
    void Update()
    {
        dateDisplayer.update();
        expDisplayer.update(expRatio, level);
        lifeDisplayer.update(lifeRatio);
    }

    public void setPlayerInfo(float lifeRatio, float expRatio, int level)
    {
        this.lifeRatio = lifeRatio;
        this.expRatio = expRatio;
        this.level = level;
    }
}

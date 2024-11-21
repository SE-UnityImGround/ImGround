using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UIBehavior : MonoBehaviour
{
    /// <summary>
    /// UI의 사용 전 초기화입니다.
    /// </summary>
    public abstract void initialize();

    /// <summary>
    /// UI 창의 표시/숨김을 관리합니다.
    /// </summary>
    /// <param name="isActive"></param>
    public virtual void setActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 현재 UI 창이 표시되는지를 반환합니다.
    /// </summary>
    /// <returns>UI 창이 표시중이면 true, 그렇지 않으면 false를 반환합니다.</returns>
    public bool getActive()
    {
        return gameObject.activeSelf;
    }
}

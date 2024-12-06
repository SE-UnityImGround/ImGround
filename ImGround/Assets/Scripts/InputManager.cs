using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InputManager
{
    public static bool onUI = false;

    /// <summary>
    /// UI가 켜진 상태에서는 입력을 무시한 입력 결과를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static bool GetKey(KeyCode key)
    {
        if (!onUI)
        {
            return Input.GetKey(key);
        }
        return false;
    }

    /// <summary>
    /// UI가 켜진 상태에서는 입력을 무시한 입력 결과를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static bool GetKeyDown(KeyCode key)
    {
        if (!onUI)
        {
            return Input.GetKeyDown(key);
        }
        return false;
    }

    /// <summary>
    /// UI가 켜진 상태에서는 입력을 무시한 입력 결과를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static bool GetMouseButton(int button)
    {
        if (!onUI)
        {
            return Input.GetMouseButton(button);
        }
        return false;
    }

    /// <summary>
    /// UI가 켜진 상태에서는 입력을 무시한 입력 결과를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static bool GetMouseButtonUp(int button)
    {
        if (!onUI)
        {
            return Input.GetMouseButtonUp(button);
        }
        return false;
    }

    /// <summary>
    /// UI가 켜진 상태에서는 입력을 무시한 입력 결과를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static float GetAxis(string axisName)
    {
        if (!onUI)
        {
            return Input.GetAxis(axisName);
        }
        return 0.0f;
    }

    /// <summary>
    /// UI가 켜진 상태에서는 입력을 무시한 입력 결과를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static float GetAxisRaw(string axisName)
    {
        if (!onUI)
        {
            return Input.GetAxisRaw(axisName);
        }
        return 0.0f;
    }

    /// <summary>
    /// UI가 켜진 상태에서는 입력을 무시한 입력 결과를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static bool GetButton(string buttonName)
    {
        if (!onUI)
        {
            return Input.GetButton(buttonName);
        }
        return false;
    }

    public static bool GetButtonDown(string buttonName)
    {
        if (!onUI)
        {
            return Input.GetButtonDown(buttonName);
        }
        return false;
    }
}

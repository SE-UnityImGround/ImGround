using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WarningManager
{
    /// <summary>
    /// 경고 처리를 시작해야할 때 발생하는 이벤트입니다.
    /// </summary>
    private static Action onWarningStartHandler;

    /// <summary>
    /// 경고 처리를 시작해야할 때 발생하는 이벤트를 구독합니다.
    /// </summary>
    /// <param name="onWarningStart"></param>
    public static void assignWarningStartHandler(Action onWarningStart)
    {
        onWarningStartHandler += onWarningStart;
    }

    /// <summary>
    /// 경고 처리를 시작합니다.
    /// </summary>
    public static void startWarning()
    {
        onWarningStartHandler?.Invoke();
    }
}

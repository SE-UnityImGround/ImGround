using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UISoundManager
{
    public static Action<int> onPlayUiSound;

    public static void playUiSound(int type)
    {
        onPlayUiSound?.Invoke(type);
    }
}

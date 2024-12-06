using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SaveManager
{
    private static Action onSave;
    public static bool isLoadedGame { get; private set; } = false;

    public static void setOnSave(Action onSave)
    {
        SaveManager.onSave += onSave;
    }

    public static void invokeOnSave()
    {
        onSave?.Invoke();
    }

    public static void setLoadState()
    {
        isLoadedGame = true;
    }
}

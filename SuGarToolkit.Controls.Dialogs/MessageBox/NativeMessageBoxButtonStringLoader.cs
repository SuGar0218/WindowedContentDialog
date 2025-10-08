using System.Text;

using Windows.System;
using Windows.Win32;

namespace SuGarToolkit.Controls.Dialogs;

internal class NativeMessageBoxButtonStringLoader
{
    public static string OK => field ??= LoadString(800u);
    public static string Cancel => field ??= LoadString(801u);
    public static string Abort => field ??= LoadString(802u);
    public static string Retry => field ??= LoadString(803u);
    public static string Ignore => field ??= LoadString(804u);
    public static string Yes => field ??= LoadString(805u);
    public static string No => field ??= LoadString(806u);
    public static string Close => field ??= LoadString(807u);
    public static string Help => field ??= LoadString(808u);
    public static string TryAgain => field ??= LoadString(809u);
    public static string Continue => field ??= LoadString(810u);

    private static string LoadString(uint id, int maxLength = 64)
    {
        char[] buffer = new char[maxLength];
        int length = PInvoke.LoadString(_moduleHandle, id, buffer, maxLength);
        StringBuilder result = new();
        for (int i = 0; i < length; i++)
        {
            result.Append(buffer[i]);
        }
        return result.ToString();
    }

    private static readonly FreeLibrarySafeHandle _moduleHandle = PInvoke.GetModuleHandle("user32.dll");
}

using System.Text;

using Windows.Win32;

namespace SuGarToolkit.Controls.Dialogs;

internal class NativeMessageBoxButtonTextLoader
{
    public static string OK => field ??= LoadText(800u);
    public static string Cancel => field ??= LoadText(801u);
    public static string Abort => field ??= LoadText(802u);
    public static string Retry => field ??= LoadText(803u);
    public static string Ignore => field ??= LoadText(804u);
    public static string Yes => field ??= LoadText(805u);
    public static string No => field ??= LoadText(806u);
    public static string Close => field ??= LoadText(807u);
    public static string Help => field ??= LoadText(808u);
    public static string TryAgain => field ??= LoadText(809u);
    public static string Continue => field ??= LoadText(810u);

    private static string LoadText(uint id, int maxLength = 64)
    {
        char[] buffer = new char[maxLength];
        int length = PInvoke.LoadString(_moduleHandle, id, buffer, maxLength);
        StringBuilder result = new();
        for (int i = 0; i < length; i++)
        {
            if (buffer[i] == '(')
                break;
            result.Append(buffer[i]);
        }
        return result.ToString();
    }

    private static readonly FreeLibrarySafeHandle _moduleHandle = PInvoke.GetModuleHandle("user32.dll");
}

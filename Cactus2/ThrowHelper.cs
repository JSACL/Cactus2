#nullable enable

namespace Cactus2;
public static class ThrowHelper
{
    public static void ThrowIfNot(bool @this, Func<Exception>? constructor = null)
    {
        if (!@this) throw constructor?.Invoke() ?? new Exception();
    } 

    public static void ThrowIf(bool @this, Func<Exception>? constructor = null)
    {
        if (@this) throw constructor?.Invoke() ?? new Exception();
    }
}
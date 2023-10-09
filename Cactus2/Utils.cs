using System.Collections.Generic;
using System.Numerics;

namespace Cactus2;
public static class Utils
{
    public static DynamicMask.Provider LayerMaskProvider { get; }

    static Utils()
    {
        LayerMaskProvider = new();
        LayerMaskProvider.Preserve(0b0000_0000_0001_1111);
    }

    public static Quaternion LookRotation(Vector3 at, Vector3 identity)
    {
        var c = Vector3.Cross(at, identity);
        var a = MathF.Asin(c.Length());
        return Quaternion.CreateFromAxisAngle(c, a);
    }

    // 300系みたいな形の関数。
    public static float Shinkansen300(float x) => x switch
    {
        <= 0 => 1,
        <= 0.75f => 1 - (2 / 3f) * x,
        <= 1f => 2 - 2 * x,
        _ => 0
    };

    public static float PullUp(float x) => x switch
    {
        <= -1 => -1,
        >= 1 => 1,
        _ => x
    };

    public static IEnumerable<T> Foreach<T>(this IEnumerable<T> @this, Action<T> callback)
    {
        foreach (var item in @this)
        {
            callback(item);
            yield return item;
        }
    }

    public static void Add<T>(this IScene @this, T obj) => @this?.Add(Typed.Get(obj));
    public static void Remove<T>(this IScene @this, T obj) => @this?.Remove(Typed.Get(obj));
}

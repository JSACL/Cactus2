#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using UED =  UnityEngine.Debug;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public static class Utils
{
    [Conditional("DEBUG")]
    public static void Want(bool condition, object? message = null, [CallerFilePath] string path = "", [CallerLineNumber] int line = -1)
    {
        if (!condition) UED.LogWarning($"{message ?? "<Utilsに由る警告>"} (path: {path}, line: {line})"); 
    }

    [Conditional("DEBUG")]
    public static void Log(bool condition = false, object? message = null, [CallerFilePath] string path = "", [CallerLineNumber] int line = -1)
    {
        if (!condition) UED.Log($"{message ?? "<Utilsに由る記録>"} (path: {path}, line: {line})");
    }

    [Conditional("DEBUG")]
    public static void Assert([DoesNotReturnIf(false)] bool condition, object? message = null, [CallerFilePath] string path = "", [CallerLineNumber] int line = -1)
    {
        UED.Assert(condition, $"{message ?? "<Utilsに由る怎辦>"} (path: {path}, line: {line})");
    }

    public static Quaternion Rotated(this Quaternion @this, Vector3 by) => Quaternion.Euler(by) * @this;

    public static void Adjust(this Transform @this, IEntity to, float rate = 1)
    {
        Want(rate is >= 0);

        if (rate is > 1) throw new ArgumentOutOfRangeException();
        if (rate is < 0) return;

        // 位置合わせ
        {
            var delta = rate * (to.Position - @this.position);

            @this.position += delta;
        }

        // 回転合わせ
        {
            var delta = rate * (Quaternion.Inverse(@this.rotation) * to.Rotation).eulerAngles;

            @this.Rotate(delta);
        }
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

    public static float Confine(float x, float min, float max) => x < min ? min : x > max ? max : x;

    public static IEnumerable<T> Foreach<T>(this IEnumerable<T> @this, Action<T> callback)
    {
        foreach (var item in @this)
        {
            callback(item);
            yield return item;
        }
    }

    public static Quaternion Multiply(this Quaternion @this, float by)
    {
        var e = @this.eulerAngles;
        if (e.x > 180) e.x -= 360;
        if (e.y > 180) e.y -= 360;
        if (e.z > 180) e.z -= 360;
        return Quaternion.Euler(by * e);
    }
}
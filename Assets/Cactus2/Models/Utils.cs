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
    public static void AssertIsNumber(Vector3 vector)
    {
        Assert(Single.IsFinite(vector.x) && !Single.IsNaN(vector.x), vector);
        Assert(Single.IsFinite(vector.y) && !Single.IsNaN(vector.y), vector);
        Assert(Single.IsFinite(vector.z) && !Single.IsNaN(vector.z), vector);
    }

    public static Quaternion Rotated(this Quaternion @this, Vector3 by) => Quaternion.Euler(by) * @this;

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
        @this.ToAngleAxis(out var angle, out var vec);
        if (!Single.IsNormal(vec.x) || !Single.IsNormal(vec.y) || !Single.IsNormal(vec.z)) return @this;
        if (angle > 180) angle -= 360;
        return Quaternion.AngleAxis(angle * by, vec);
    }
}
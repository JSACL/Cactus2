#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using UED =  UnityEngine.Debug;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static UnityEngine.ParticleSystem;
using System.Linq;

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

    public static int OneHot(int shift) => 1 << shift;
    public static int ShiftOf(int oneHot) => oneHot switch
    {
        1 => 0,
        2 => 1,
        4 => 2,
        8 => 3,
        16 => 4,
        32 => 5,
        64 => 6,
        128 => 7,
        256 => 8,
        512 => 9,
        1024 => 10,
        2048 => 11,
        4096 => 12,
        8192 => 13,
        16384 => 14,
        32768 => 15,
        65536 => 16,
        131072 => 17,
        262144 => 18,
        524288 => 19,
        1048576 => 20,
        2097152 => 21,
        4194304 => 22,
        8388608 => 23,
        16777216 => 24,
        33554432 => 25,
        67108864 => 26,
        134217728 => 27,
        268435456 => 28,
        536870912 => 29,
        1073741824 => 30,
        -2147483648 => 31,
        _ => throw new ArgumentException()
    };

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

    public static IEnumerable<TResult> Successed<TSource, TResult>(this IEnumerable<TSource> @this, TryFunc<TSource, TResult> tryFunc)
    {
        foreach (var item in @this)
        {
            if (tryFunc(item, out var r)) yield return r;
        }
    }

    public static T? GetComponentIC<T>(this Component @this, string name, bool includeInactive = false) where T : Component => GetComponentIC<T>(@this.transform, name, includeInactive);
    public static T? GetComponentIC<T>(this GameObject @this, string name, bool includeInactive = false) where T : Component => GetComponentIC<T>(@this.transform, name, includeInactive);
    public static T? GetComponentIC<T>(this Transform @this, string name, bool includeInactive = false) where T : Component
    {
        if (@this.name == name && @this.TryGetComponent<T>(out var r)) return r;
        var l = @this.childCount;
        for (int i = 0; i < l; i++)
        {
            if (@this.GetChild(i).GetComponentIC<T>(name, includeInactive) is { } r_) return r_;
        }
        return null;
    }

    public static IEnumerable<TComponent> WithinSight<TComponent>(this IEnumerable<TComponent> @this, Vector3 p, Quaternion d, float x, float y) where TComponent : Component
    {
        return @this.Where(P);

        bool P(Component c)
        {
            var v = c.transform.position - p;
            var v_f = Quaternion.Inverse(d) * v;
            return v_f.z <= x * v_f.x || v_f.y <= y * v_f.x;
        }
    }
    public static IEnumerable<TComponent> WithinSight<TComponent>(this IEnumerable<TComponent> @this, Vector3 p, Quaternion d, float r) where TComponent : Component
    {
        return @this.Where(P);

        bool P(Component c)
        {
            var v = c.transform.position - p;
            var v_f = Quaternion.Inverse(d) * v;
            var x = v_f.x;
            v_f.x = 0;
            return v_f.sqrMagnitude <= r * x * r * x;
        }
    }
}
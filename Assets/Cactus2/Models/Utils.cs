#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using UED =  UnityEngine.Debug;
using UnityEngine;

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
    public static void Assert(bool condition, object? message = null, [CallerFilePath] string path = "", [CallerLineNumber] int line = -1)
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
}
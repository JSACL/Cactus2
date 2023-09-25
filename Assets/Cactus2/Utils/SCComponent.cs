#nullable enable
using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SCComponent : MonoBehaviour
{
    protected virtual void Start()
    {
        var info = SCComponentInfo.GetInfo(GetType());
        if (info.ShortCircuitIsAvailable)
        {
            gameObject.layer |= info.LayerMask!;
        }
    }
}

public class SCComponentInfo
{
    public bool ShortCircuitIsAvailable { get; private set; }
    public DynamicMask LayerMask { get; private set; } = default;

    public SCComponentInfo(Type type)
    {
        ShortCircuitIsAvailable = true;
        LayerMask = DynamicMask.GetNew(SceneConfiguration.Current.LayerMaskProvider, $"To short-circuit component: {type.Name}.");
    }

    public static SCComponentInfo GetInfo(Type type)
    {
        if (_dictionary.TryGetValue(type, out var r)) return r;

        r = new(type);
        _dictionary.Add(type, r);
        return r;
    }

    readonly static Dictionary<Type, SCComponentInfo> _dictionary = new();

    public static SCComponentInfo GetInfo<T>()
    {
        if (StaticDictionary<T>.value is { } value) return value;

        return StaticDictionary<T>.value = GetInfo(typeof(T));
    }

    static class StaticDictionary<T>
    {
        public static SCComponentInfo? value;
    }
}

public static class SCComponentExtensions
{
    public static TComponent? GetComponentSC<TComponent>(this Component @this) where TComponent : SCComponent
    {
        var info = SCComponentInfo.GetInfo<TComponent>();
        if (info.ShortCircuitIsAvailable)
        {
            var layer = @this.gameObject.layer;

            if ((info.LayerMask & layer) != 0)
            {
                return @this.GetComponent<TComponent>();
            }
            else
            {
                return null;
            }
        }
        else
        {
            return @this.GetComponent<TComponent>();
        }
    }
    public static TComponent? GetComponentSC<TComponent>(this GameObject @this) where TComponent : SCComponent
    {
        var info = SCComponentInfo.GetInfo<TComponent>();
        if (info.ShortCircuitIsAvailable)
        {
            var layer = @this.layer;

            if ((info.LayerMask & layer) != 0)
            {
                return @this.GetComponent<TComponent>();
            }
            else
            {
                return null;
            }
        }
        else
        {
            return @this.GetComponent<TComponent>();
        }
    }

}
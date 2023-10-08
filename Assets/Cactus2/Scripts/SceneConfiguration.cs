#nullable enable
using System;
using Nonno.Assets;
using UnityEditor;
using UnityEngine;

public class SceneConfiguration : ScriptableObject
{
    public DynamicMask.Provider LayerMaskProvider { get; } = new();

    static SceneConfiguration _current = new();

    public SceneConfiguration()
    {
        LayerMaskProvider = new();
        LayerMaskProvider.Preserve(0b0000_0000_0001_1111);
    }

    public static event EventHandler? Changed;
    public static SceneConfiguration Current
    {
        get => _current;
        set
        {
            _current = value;
            Changed?.Invoke(null, EventArgs.Empty);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(SceneConfiguration))]
public class SceneConfigurationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var c = (SceneConfiguration)target;

    }
}

#endif
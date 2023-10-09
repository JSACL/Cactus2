using System;
using System.ComponentModel;
using System.Numerics;
using Nonno.Assets;

public readonly struct InputEventArgs
{
    public Action Action { get; }

    public InputEventArgs(Action action)
    {
        Action = action;
    }
}

public delegate void InputEventHandler(object? sender, InputEventArgs e);

public readonly struct CollectionChangeEventArgs<T>
{
    public CollectionChangeAction Action { get; }
    public T? Element { get; }

    public CollectionChangeEventArgs(CollectionChangeAction action, T? element)
    {
        Action = action;
        Element = element;
    }
}

public delegate void CollectionChangeEventHandler<T>(object? sender, CollectionChangeEventArgs<T> e);

public readonly struct ChangeEventArgs<T>
{
    public T? Value { get; }

    public ChangeEventArgs(T? element)
    {
        Value = element;
    }
}

public delegate void ChangeEventHandler<T>(object? sender, ChangeEventArgs<T> e);

public readonly struct UpdateEventArgs
{
    public float DeltaTime { get; }
    public int FrameCount { get; }

    public UpdateEventArgs(float deltaTime, int frameCount)
    {
        DeltaTime = deltaTime;
        FrameCount = frameCount;
    }
}

public delegate void UpdateEventHandler(object? sender, UpdateEventArgs e);

public readonly struct EffectEventArgs
{
    public Effect Effect { get; }

    public EffectEventArgs(Effect effect)
    {
        Effect = effect;
    }
}

public delegate void EffectEventHandler(object? sender, EffectEventArgs e);

public readonly struct FamilyChangeEventArgs
{
    public FamilyChangeEventArgs(SceneChangeAction action, Typed @object)
    {
        Action = action;
        Object = @object;
    }

    public SceneChangeAction Action { get; }
    public Typed Object { get; }
}

public delegate void FamilyChangeEventHandler(object? sender, FamilyChangeEventArgs e);
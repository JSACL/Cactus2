#nullable enable

using System;
using System.ComponentModel;
using UnityEngine;

public readonly struct ElapsedEventArgs
{
    public float DeltaTime { get; }

    public ElapsedEventArgs(float deltaTime)
    {
        DeltaTime = deltaTime;
    }
}

public delegate void ElapsedEventHandler(object? sender, ElapsedEventArgs e);

public readonly struct InputEventArgs
{
    public Action Action { get; }

    public InputEventArgs(Action action)
    {
        Action = action;
    }
}

public delegate void InputEventHandler(object? sender,  InputEventArgs e);

public readonly struct ColliderEventArgs
{
    public ColliderFlug Flug { get; }
    public Collider Other { get; }
    public Collision? Collision { get; }
    public int CollidingCount { get; }
    public Vector3 Impulse { get; }

    public ColliderEventArgs(ColliderFlug flug, Collision collision, int collidingCount)
    {
        Flug = flug;
        Collision = collision;
        Other = collision.collider;
        CollidingCount = collidingCount;
        Impulse = collision.impulse;
    }
    public ColliderEventArgs(ColliderFlug flug, Collider other, int collidingCount)
    {
        Flug = flug;
        Collision = null;
        Other = other;
        CollidingCount = collidingCount;
        Impulse = default;
    }
}

public delegate void ColliderEventHandler(object? sender, ColliderEventArgs e);

public readonly struct AnimationTransitionEventArgs
{
    readonly int _id;
    readonly string? _name;

    public bool AutoReset { get; }
    public int? Identifier => _name is not null ? null : _id;
    public string? Name => _name;

    public AnimationTransitionEventArgs(int identifier, bool autoReset)
    {
        _id = identifier;
        _name = null;

        AutoReset = autoReset;
    }
    public AnimationTransitionEventArgs(string name, bool autoReset)
    {
        _id = -1;
        _name = name;

        AutoReset = autoReset;
    }

    public int GetIdentifier(Animator animator) => _name is null ? _id : animator.GetInteger(_name);

    public void Apply(Animator to, bool value = true)
    {
        var i = GetIdentifier(to);

        switch (AutoReset, value)
        {
        case (true, false):
            to.SetTrigger(i);
            return;
        case (true, true):
            to.ResetTrigger(i);
            return;
        case (false, _):
            to.SetBool(i, value);
            return;
        }
    }
}

public delegate void AnimationTransitionEventHandler(object? sender, AnimationTransitionEventArgs e);

public readonly struct CollectionChangeEventArgs
{
    public CollectionChangeAction Action { get; }
    public object? Element { get; }

    public CollectionChangeEventArgs(CollectionChangeAction action, object? element)
    {
        Action = action;
        Element = element;
    }
}

public delegate void CollectionChangeEventHandler(object? sender, CollectionChangeEventArgs e);

public readonly struct CommandEventArgs
{
    public ICommand Command { get; }
    public CommandEventArgs(ICommand command) => Command = command;
}

public delegate void CommandEventHandler(object? sender, CommandEventArgs e);

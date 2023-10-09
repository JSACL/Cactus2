#nullable enable

using Cactus2;
using UnityEngine;

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

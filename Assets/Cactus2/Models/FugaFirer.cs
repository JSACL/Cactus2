#nullable enable
using System;
using UnityEngine;

public class FugaFirer : Weapon, IFirer
{
    public FugaFirer(IScene scene) : base(scene)
    {

    }

    protected override void Fire(Authority tag)
    {
        Scene.Add(new Bullet(Scene)
        {
            Authority = tag,
            Rotation = Rotation,
            Position = Position,
            Velocity = Rotation * (20 * Vector3.forward),
        });
    }
}
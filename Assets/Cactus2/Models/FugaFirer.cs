#nullable enable
using System;
using UnityEngine;

public class FugaFirer : Weapon, IFirer
{
    public FugaFirer(DateTime time) : base(time)
    {

    }

    protected override void Fire(ParticipantIndex tag)
    {
        var b = new Bullet(Time)
        {
            ParticipantIndex = tag,
            Rotation = Rotation,
            Position = Position,
            Velocity = Rotation * (20 * Vector3.forward),
            Visitor = Visitor
        };
    }
}
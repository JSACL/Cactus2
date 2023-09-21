#nullable enable
using System;
using UnityEngine;

public class FugaFirer : Weapon, IFirer
{
    public FugaFirer(DateTime time, TeamGameReferee.Team team) : base(time, team)
    {

    }

    protected override void Fire(Tag tag)
    {
        var b = new Bullet(Time)
        {
            Tag = tag,
            Rotation = Rotation,
            Position = Position,
            Velocity = Rotation * (20 * Vector3.forward),
            Visitor = Visitor
        };
    }
}
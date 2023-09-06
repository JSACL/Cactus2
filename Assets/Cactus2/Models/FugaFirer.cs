#nullable enable
using System;
using UnityEngine;

public class FugaFirer : Weapon, IFirer
{
    public FugaFirer(TeamGameReferee.Team team) : base(team)
    {

    }

    protected override void Fire(Tag tag)
    {
        var b = new Bullet(tag)
        {
            Rotation = Rotation,
            Position = Position,
            Velocity = Rotation * (InitialSpeed * Vector3.forward),
            Visitor = Visitor
        };
    }
}
#nullable enable
using System;
using UnityEngine;

public class FugaFirer : Weapon, IFirer
{
    protected override void Fire(Team? team)
    {
        var b = new Bullet()
        {
            Rotation = Rotation, 
            Position = Position,
            Velocity = Rotation * (InitialSpeed * Vector3.forward),
            Visitor = Visitor 
        };
        
        team?.Referee.Join(b, team.Name);
    }
    protected override void Fire(ParticipantInfo participantInfo)
    {
        var b = new Bullet()
        {
            Rotation = Rotation,
            Position = Position,
            Velocity = Rotation * (InitialSpeed * Vector3.forward),
            Visitor = Visitor
        };

        Referee.SetInfo(b, participantInfo);
    }
}
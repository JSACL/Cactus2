#nullable enable

using System;
using UnityEngine;

public class LaserGun : Weapon
{
    Laser? _laserHavingFired;
    DateTime _timeToStop;

    public LaserGun(DateTime time) : base(time)
    {
    }

    public Vector3 TargetPosition { get; set; }
    public TimeSpan Span => new(0, 0, 2);
    public override float CooldownTime => 5;

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Time > _timeToStop)
        {
            // レーザーを照射し終わる。
            _laserHavingFired = null;
        }

        if (_laserHavingFired is not null)
        {
            // レーザーを照射し続ける。
            _laserHavingFired.Length = (_laserHavingFired.Position - Position).magnitude;
        }
    }

    protected override void Fire(ParticipantIndex tag)
    {
        var v = TargetPosition - Position;
        var v_n = v.normalized;
        _laserHavingFired = new Laser(Time)
        {
            Visitor = Visitor,
            Position = Position,
            Velocity = 100 * v_n,
            ParticipantIndex = ParticipantIndex,
            Rotation = Quaternion.LookRotation(v),
        };
        _timeToStop = Time + Span;
    }
}
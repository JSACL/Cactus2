#nullable enable

using System;
using UnityEngine;

public class LaserGun : Weapon
{
    Laser? _laserHavingFired;
    DateTime _timeToStop;

    public Vector3 TargetPosition { get; set; }
    public TimeSpan Span => new(0, 0, 2);
    public override float CooldownTime => 5;

    public LaserGun(IScene scene) : base(scene)
    {
    }

    public override void Visit(IVisitor visitor) => visitor.Add(this);
    public override void Forgo(IVisitor visitor) => visitor.Remove(this);

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

    protected override void Fire(Authority tag)
    {
        var v = TargetPosition - Position;
        var v_n = v.normalized;
        Scene.Add(_laserHavingFired = new Laser(Scene)
        {
            Position = Position,
            Velocity = 100 * v_n,
            Authority = tag,
            Rotation = Quaternion.LookRotation(v),
        });
        _timeToStop = Time + Span;
    }
}
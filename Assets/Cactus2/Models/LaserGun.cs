#nullable enable

using System;
using Nonno.Assets;
using System.Numerics;

public class LaserGun : Weapon
{
    Laser? _laserHavingFired;
    DateTime _timeToStop;

    public Vector3 TargetPosition { get; set; }
    public TimeSpan Span => new(0, 0, 2);
    public override float CooldownTime => 5;
    public override string Name => "Laser Gun";

    public LaserGun(IScene scene) : base(scene)
    {
    }

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
            _laserHavingFired.Length = Vector3.Distance(_laserHavingFired.Transform.Position, Transform.Position);
        }
    }

    protected override void Fire(Authority authority)
    {
        var v = TargetPosition - Transform.Position;
        var v_n = Vector3.Normalize(v);
        var r = Utils.LookRotation(v, Vector3.UnitZ);
        Transform = new(Transform.Position, r);
        Scene.Add(_laserHavingFired = new Laser(Scene)
        {
            Transform = Transform,
            Velocity = new(100 * v_n, Vector3.Zero),
            Authority = authority,
        });
        _timeToStop = Time + Span;
    }
}
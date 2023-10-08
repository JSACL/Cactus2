#nullable enable
using System;
using System.Threading.Tasks;
using Nonno.Assets;
using Nonno.Assets.Presentation;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : Entity, IBullet
{
    readonly DateTime _due;
    readonly HitEffect _hitEffect;
    public HitEffect HitEffect => _hitEffect;
    public bool VanishOnHit { get; }
    public bool VanishAutonomously { get; }

    public Bullet(IScene scene) : base(scene)
    {
        _due = Time + new TimeSpan(0, 0, 20);
        _hitEffect = new() { DamageForResilience = 0.1f, DamageForVitality = 0.1f };
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Time > _due) Scene.Remove(this);
    }
}

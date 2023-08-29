#nullable enable
using System;
using UnityEngine;

public class Firer1 : Entity, IFirer
{
    float _cooldownTimeRemaining;

    public override IVisitor? Visitor
    {
        get => base.Visitor;
        set
        {
            _visitor?.Remove(this);
            _visitor = value;
            _visitor?.Add(this);
        }
    }
    public float CooldownTimeRemaining => _cooldownTimeRemaining;
    public float CooldownTime => 1f;

    protected override void Update(float deltaTime)
    {
        _cooldownTimeRemaining -= deltaTime;
        if (_cooldownTimeRemaining < 0) _cooldownTimeRemaining = 0;

        base.Update(deltaTime);
    }

    public void Fire(IEntity issuer, IEntity? target)
    {
        if (CooldownTimeRemaining > 0) return;

        _cooldownTimeRemaining = CooldownTime;

        var v = target is null ? Rotation * Vector3.forward : target.Position - Position;
        new Bullet(issuer) 
        { 
            Target = target, 
            Rotation = Quaternion.LookRotation(v), 
            Position = Position,
            Velocity = v.normalized,
            Visitor = Visitor 
        };
    }
}
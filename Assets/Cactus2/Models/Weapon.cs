#nullable enable
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;
using UnityEngine.UIElements;
using System;

public abstract class Weapon : Entity, IWeapon
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
    public bool IsReadyToFire => CooldownTimeRemaining <= 0;
    public virtual float CooldownTime => 1f;
    public ParticipantIndex BulletIndex { get; set; } = ParticipantIndex.Unknown;

    public Weapon(DateTime time) : base(time)
    {
    }

    protected override void Update(float deltaTime)
    {
        _cooldownTimeRemaining -= deltaTime;
        if (_cooldownTimeRemaining < 0) _cooldownTimeRemaining = 0;

        base.Update(deltaTime);
    }

    public void Trigger()
    {
        if (!IsReadyToFire) return;

        _cooldownTimeRemaining = CooldownTime;

        Fire(BulletIndex);
    }

    protected abstract void Fire(ParticipantIndex tag);
}
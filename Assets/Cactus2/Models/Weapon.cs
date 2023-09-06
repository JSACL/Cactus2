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
    public virtual float CooldownTime => 1f;
    public virtual float InitialSpeed => 5f;
    public TeamGameReferee.Team Team { get; set; }

    public Weapon(TeamGameReferee.Team team)
    {
        Team = team;
    }

    protected override void Update(float deltaTime)
    {
        _cooldownTimeRemaining -= deltaTime;
        if (_cooldownTimeRemaining < 0) _cooldownTimeRemaining = 0;

        base.Update(deltaTime);
    }

    public void Trigger()
    {
        if (CooldownTimeRemaining > 0) return;

        _cooldownTimeRemaining = CooldownTime;

        if (Team.Weapon is not { } weaponPI) throw new InvalidOperationException("Š‘®ƒ`[ƒ€‚É‚Í•Ší‚ª‘¶İ‚Å‚«‚Ü‚¹‚ñB");
        Fire(weaponPI);
    }

    protected abstract void Fire(Tag tag);
}
#nullable enable
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;
using UnityEngine.UIElements;

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

    protected override void Update(float deltaTime)
    {
        _cooldownTimeRemaining -= deltaTime;
        if (_cooldownTimeRemaining < 0) _cooldownTimeRemaining = 0;

        base.Update(deltaTime);
    }

    public void Trigger(Team? team)
    {
        if (CooldownTimeRemaining > 0) return;

        _cooldownTimeRemaining = CooldownTime;

        Fire(team);
    }
    public void Trigger(ParticipantInfo participantInfo)
    {
        if (CooldownTimeRemaining > 0) return;

        _cooldownTimeRemaining = CooldownTime;

        Fire(participantInfo);
    }

    protected abstract void Fire(Team? team);
    protected abstract void Fire(ParticipantInfo participantInfo);
}
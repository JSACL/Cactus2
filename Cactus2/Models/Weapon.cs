#nullable enable
using System;
using Nonno.Assets;
using Nonno.Assets.Presentation;

public abstract class Weapon : Entity, IWeapon
{
    float _cooldownTimeRemaining;

    public abstract string Name { get; }
    public float CooldownTimeRemaining => _cooldownTimeRemaining;
    public bool IsReadyToFire => CooldownTimeRemaining <= 0;
    public virtual float CooldownTime => 1f;
    public Authority BulletIndex { get; set; } = Authority.Unknown;
    public override IScene Scene
    {
        get => _scene;
        set
        {
            _scene.Remove(this);
            _scene = value;
            _scene.Add(this);
        }
    }

    public Weapon(IScene scene) : base(scene)
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

    protected abstract void Fire(Authority tag);
}
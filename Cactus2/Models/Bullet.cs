#nullable enable

namespace Cactus2;

public class Bullet : Entity, IBullet
{
    readonly DateTime _due;
    readonly HitEffect _hitEffect;
    public HitEffect HitEffect => _hitEffect;
    public bool VanishOnHit { get; }
    public bool VanishAutonomously { get; }

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

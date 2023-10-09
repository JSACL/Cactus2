#nullable enable
using System;
using System.Threading.Tasks;

public class Laser : Entity, ILaser
{
    readonly DateTime _due;
    readonly HitEffect _hitEffect;
    public HitEffect HitEffect => _hitEffect;
    public float Length { set; get; }
    public float Strength { private set; get; } = 200;
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

    public Laser(IScene scene) : base(scene)
    {
        _due = Time + new TimeSpan(0, 0, 20);
        _hitEffect = new() { DamageForResilience = 0.1f, DamageForVitality = 0.1f };
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        Strength -= 10 * deltaTime;
        if (Time > _due) Scene.Remove(this);
    }
}
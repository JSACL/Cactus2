#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : Entity, IBullet
{
    readonly DateTime _due;

    public float DamageForVitality { get; }
    public float DamageForResilience { get; }
    public bool VanishOnHit { get; }
    public bool VanishAutonomously { get; }
    public bool BreakOnlyOnDefaultLayer { get; }
    public Vector3? TargetCoordinate { get; set; }
    public event EventHandler? ShowEffect;
    //public Tag Tag { get; }

    public Bullet(IScene scene) : base(scene)
    {
        _due = Time + new TimeSpan(0, 0, 20);

        Authority = Authority.Unknown;
    }

    public override void Visit(IVisitor visitor) => visitor.Add(this);
    public override void Forgo(IVisitor visitor) => visitor.Remove(this);

    public async void Hit()
    {
        ShowEffect?.Invoke(this, EventArgs.Empty);
        //Visitor = null;

        await Task.Delay(100);
        Scene.Remove(this);
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        Track(deltaTime);

        if (Time > _due) Scene.Remove(this);
    }

    protected void Track(float deltaTime)
    {
        if (TargetCoordinate is Vector3 tC)
        {
            Rotation = Quaternion.Lerp(Rotation, Quaternion.LookRotation(tC - Position), deltaTime);
            Velocity += Rotation * Vector3.forward;
        }
    }
}
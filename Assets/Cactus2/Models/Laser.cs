#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : Entity, ILaser
{
    public float DamageForVitality { get; } = 0.2f;
    public float DamageForResilience { get; } = 0.1f;
    public float Length { set;  get; }
    public float Strength { private set; get; } = 200;
    public event EventHandler? ShowEffect;

    public Laser(IScene scene) : base(scene)
    {
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

        Strength -= 10 * deltaTime;
        if (Strength <= 0) Scene.Remove(this);
    }
}
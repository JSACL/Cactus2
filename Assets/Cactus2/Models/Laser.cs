#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : Entity, IBullet
{
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
    public float DamageForVitality { get; }
    public float DamageForResilience { get; }
    public bool VanishOnHit { get; }
    public bool VanishAutonomously { get; }
    public bool BreakOnlyOnDefaultLayer { get; }
    public Vector3? TargetCoordinate { get; set; }
    public event EventHandler? ShowEffect;

    public Laser()
    {
    }

    public async void Hit()
    {
        ShowEffect?.Invoke(this, EventArgs.Empty);
        //Visitor = null;

        await Task.Delay(100);
        Visitor = null;
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }
}
#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : Entity, ILaser
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
    public float Length { set;  get; }
    public float Strength { private set; get; } = 200;
    public event EventHandler? ShowEffect;

    public Laser(DateTime time) : base(time)
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

        Strength -= 10 * deltaTime;
        if (Strength <= 0) Visitor = null;
    }
}
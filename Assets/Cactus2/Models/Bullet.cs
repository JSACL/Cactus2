#nullable enable
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : Entity, IBullet
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
    public float DamageForVagor { get; }
    float IBullet.DamageForHitPoint => DamageForVagor;
    public float DamageForRepairPoint { get; }
    public bool VanishOnHit { get; }
    public bool VanishAutonomously { get; }
    public bool BreakOnlyOnDefaultLayer { get; }
    public IEntity? Target { get; set; }
    public IEntity Issuer { get; }
    public event EventHandler? ShowEffect;

    public Bullet(IEntity issuer)
    {
        Issuer = issuer;
    }

    public void Hit()
    {
        ShowEffect?.Invoke(this, EventArgs.Empty);
        //Visitor = null;
    }

    protected override void Update(float deltaTime)
    {
        Debug.Log($"{Velocity} {Position}");

        base.Update(deltaTime);
    }

    protected void Track(float deltaTime)
    {
        if (Target != null)
        {
            Rotation = Quaternion.Lerp(Rotation, Quaternion.LookRotation(Target.Position - Position), deltaTime);
        }
        //velocity += acceleration * Time.deltaTime;
        //if (acceleration != 0f)
        //{
        //    this.gameObject.transform.position += this.gameObject.transform.forward * (((velocity * velocity) - (velocity_ini * velocity_ini)) / (2f * acceleration));
        //}
        //else
        //{
        //    this.gameObject.transform.position += this.gameObject.transform.forward * (velocity * Time.deltaTime);
        //}
    }
}
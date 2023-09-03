#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using static ConstantValues;
using static UnityEngine.Input;
using static Utils;
using vec = UnityEngine.Vector3;

public class FugaEnemy : Animal, ISpecies1
{
    float _timer;

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

    public const float FORCE_REDUCTION_RATE_PER_SEC = 0.001f;

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        _timer += deltaTime;
        if (_timer > 5)
        {
            new Laser() 
            { 
                Visitor = Visitor, 
                Position = Position, 
                Rotation = Rotation, 
                Time = Time,
                Velocity = 50f * (Rotation * Vector3.forward),
            };
            _timer -= 5;
        }
    }
}

#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static ConstantValues;
using static UnityEngine.Input;
using static Utils;
using Random = System.Random;
using vec = UnityEngine.Vector3;

public class FugaEnemy : Animal, ISpecies1
{
    public const float FORCE_REDUCTION_RATE_PER_SEC = 0.001f;
    LaserGun? _gun;
    readonly Random _rand;
    readonly CachedCollection<Vector3> _targetPositions;

    public LaserGun? Gun { get => _gun; set => _gun = value; }

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
    public IEnumerable<Vector3> TargetPositions
    {
        get => _targetPositions;
        set => _targetPositions.InnerEnumerable = value;
    }

    public FugaEnemy(DateTime time) : base(time)
    {
        _targetPositions = new();
        _rand = new Random();
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (_gun is { })
        {
            _gun.Position = Position;
            _gun.Rotation = Rotation;
            if (_gun.IsReadyToFire)
            {
                _targetPositions.Update();
                if (_targetPositions.Any())
                {
                    _gun.TargetPosition = _targetPositions[_rand.Next(_targetPositions.Count - 1)];
                    _gun.Trigger();
                }
            }
        }
    }
}

#nullable enable
using System;
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

    readonly Random _rand;
    float _cooldownTime_rest;
    float _charge_rest = 0;
    Laser? _laserHavingFired;
    Vector3[] _targetCoordinates;

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
    public ReadOnlySpan<Vector3> TargetCoordinates
    {
        get => _targetCoordinates;
        set
        {
            Array.Resize(ref _targetCoordinates, value.Length);
            value.CopyTo(_targetCoordinates);
        }
    }

    public FugaEnemy(DateTime time) : base(time)
    {
        _targetCoordinates = Array.Empty<Vector3>();
        _rand = new Random();
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        _cooldownTime_rest -= deltaTime;
        if (_cooldownTime_rest < 0)
        {
            _cooldownTime_rest += 5;
            _charge_rest += 0.1f;

            var v = _targetCoordinates[_rand.Next(_targetCoordinates.Length - 1)] - Position;
            var v_n = v.normalized;
            _laserHavingFired = new Laser(Time)
            {
                Visitor = Visitor,
                Position = Position,
                Velocity = 100 * v_n,
                Tag = Tag,
                Rotation = Quaternion.LookRotation(v),
            };
        }

        if (_charge_rest < 0)
        {
            _laserHavingFired = null;
        }

        if (_laserHavingFired is not null)
        {
            _laserHavingFired.Length = (_laserHavingFired.Position - Position).magnitude;
            _charge_rest -= 0.1f * deltaTime;
        }
    }
}

#nullable enable
using System;
using UnityEngine;
using vec = UnityEngine.Vector3;
using static System.MathF;
using static Utils;
using TMPro;

public class Entity : IEntity
{
    DateTime _time;
    float _mass;
    float _mass_rec;
    Quaternion _rotation;
    vec _angularVelocity;
    vec _velocity;
    vec _position;

    protected internal bool SkipVelocityApplication { get; set; }
    public vec Position
    {
        get => _position;
        set
        {
            AssertIsNumber(value);
            _position = value;
        }
    }
    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
        }
    }
    public vec Velocity
    {
        get => _velocity;
        set
        {
            AssertIsNumber(value);
            _velocity = value;
        }
    }
    public vec AngularVelocity
    {
        get => _angularVelocity;
        set
        {
            const float A_V_MAX = 50;
            if (value.x is < -A_V_MAX or > A_V_MAX ||
                value.y is < -A_V_MAX or > A_V_MAX ||
                value.z is < -A_V_MAX or > A_V_MAX) 
                throw new ArgumentOutOfRangeException();
            _angularVelocity = value;
        }
    }
    public float Mass
    {
        get => _mass;
        protected set
        {
            if (!Single.IsNormal(value)) throw new ArgumentException();
            _mass = value;
            _mass_rec = 1 / value;
        }
    }
    public float Mass_rec
    {
        get => _mass_rec;
    }
    public DateTime Time
    {
        get => _time;
        set
        {
            var dif = value - _time;
            if (dif == default) return;

            _time = value;
            Update((float)dif.TotalSeconds);
        }
    }

    public Entity()
    {
        Mass = 10;
        Rotation = Quaternion.identity;
    }

    public void AddTime(float deltaTime)
    {
        Update(deltaTime);
        _time += TimeSpan.FromSeconds(deltaTime);
    }

    protected virtual void Update(float deltaTime)
    {
        if (!SkipVelocityApplication)
        {
            Position += deltaTime * Velocity;
            Rotation = Quaternion.Euler(deltaTime * AngularVelocity) * Rotation;
        }
    }

    public virtual void Impulse(vec at, vec impulse)
    {
        Velocity += impulse * Mass_rec;
    }

    protected Vector3 GetLocalVelocity() => Quaternion.Inverse(Rotation) * Velocity;
}

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

    protected internal bool SkipVelocityApplication { get; set; }
    public vec Position { get; set; }
    public virtual vec Velocity { get; set; }
    public Quaternion Rotation { get; set; }
    public virtual vec AngularVelocity { get; set; }
    public DateTime Time
    {
        get => _time;
        set
        {
            var dif = value - _time;
            if (dif == default) return;

            _time = value;
            Elapsed(this, new((float)dif.TotalSeconds));
        }
    }

    public Entity()
    {
        Rotation = Quaternion.identity;
    }

    public void Rotate(vec euler) => Rotation = Quaternion.Euler(euler) * Rotation;
    public void Rotate(Quaternion rotation) => Rotation = rotation * Rotation;

    public void AddTime(float deltaTime)
    {
        Elapsed(null, new(deltaTime));
        _time += TimeSpan.FromSeconds(deltaTime);
    }

    protected virtual void Elapsed(object? sender, ElapsedEventArgs e)
    {

    }
}

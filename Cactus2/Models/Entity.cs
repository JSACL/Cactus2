#nullable enable
using System;
using static System.MathF;
using static Cactus2Utils;
using Nonno.Assets;
using Nonno.Assets.Presentation;
using System.Numerics;

public class Entity : IEntity
{
    protected IScene _scene;
    DateTime _time;
    float _mass;
    float _mass_rec;
    Transform _transform;
    Displacement _velocity;
    Authority _authority;

    protected internal bool SkipVelocityApplication { get; set; }
    public virtual IScene Scene
    {
        get => _scene;
        set
        {
            _scene.Remove(this);
            _scene = value;
            _scene.Add(this);
        }
    }
    public Transform Transform
    {
        get => _transform;
        set => _transform = value;
    }
    public Displacement Velocity
    {
        get => _velocity;
        set
        {
            const float MAX = 50;
            if (value.Angular.X is < -MAX or > MAX ||
                value.Angular.Y is < -MAX or > MAX ||
                value.Angular.Z is < -MAX or > MAX)
                throw new ArgumentOutOfRangeException();
            _velocity = value;
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
    public Authority Authority 
    { 
        get => _authority;
        set
        {
            if (_authority != value)
            {
                _authority = value;
                Authorize(value);
            }
        }
    }

    public Entity(IScene scene)
    {
        _time = scene.Time;
        _authority = Authority.Unknown;

        Scene = scene;
        Mass = 10;
        Transform = Transform.Identity;
    }

    public void AddTime(float deltaTime)
    {
        Update(deltaTime);
        _time += TimeSpan.FromSeconds(deltaTime);
    }

    protected virtual void Authorize(Authority authority)
    {

    }

    protected virtual void Update(float deltaTime)
    {
        if (!SkipVelocityApplication)
        {
            Transform += deltaTime * Velocity;
        }
    }

    public virtual void Impulse(Vector3 at, Vector3 impulse)
    {
        Velocity = new(Velocity.Linear + impulse * _mass_rec, Velocity.Angular);
    }
}

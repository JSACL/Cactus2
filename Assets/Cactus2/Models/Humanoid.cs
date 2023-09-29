#nullable enable
using UnityEngine;
using static Utils;
using vec = UnityEngine.Vector3;
using static ConstantValues;
using static System.MathF;
using static System.Math;
using System.Threading.Tasks;
using System;

public class Humanoid : Animal, IHumanoid
{
    public const float FORCE_REDUCTION_RATE_PER_SEC = 0.001f;
    public const float FORCE_LEG_FORWARD_MAX = 50f;
    public const float FORCE_LEG_HORIZONTAL_MAX = 30f;
    public const float FORCE_LEG_HORIZONTAL_MIN = -FORCE_LEG_HORIZONTAL_MAX;
    //public const float DEF_FORCE_LEG_UPWARD = 2.5f;
    //public const float DEF_FORCE_LEG_DOWNWARD = 0.1f;
    public const float SPEED_FORWARD_MAX = 2f;
    public const float SPEED_HORIZONTAL_MAX = 1.8f;
    public const float SPEED_HORIZONTAL_MIN = -SPEED_HORIZONTAL_MAX;
    const int JUMP_DELAY_MS = 100;
    const float STOP_PROMPTNESS = 600f;
    const float MOVEMENT_PROMPTNESS = 180.0f;
    const float STOP_PULL_UP_COEF = 0.2f;

    bool _footIsOn;
    vec _force_leg;

    public bool IsRunning { get; set; }
    public vec Force_leg
    {
        get => _force_leg;
        set
        {
            var v = value;

            var speed_z = vec.Dot(Velocity, Rotation * vec.forward);
            var speed_x = vec.Dot(Velocity, Rotation * vec.right);
            var max_z = FORCE_LEG_FORWARD_MAX * Shinkansen300(speed_z / SPEED_FORWARD_MAX);
            var min_z = FORCE_LEG_HORIZONTAL_MIN * Shinkansen300(speed_z / SPEED_HORIZONTAL_MIN);
            var max_x = FORCE_LEG_HORIZONTAL_MAX * Shinkansen300(speed_x / SPEED_HORIZONTAL_MAX);
            var min_x = FORCE_LEG_HORIZONTAL_MIN * Shinkansen300(speed_x / SPEED_HORIZONTAL_MIN);

            v.z = Clamp(v.z, min_z, max_z);
            v.x = Clamp(v.x, min_x, max_x);
            //Log(message: $"in:{value} sp:{speed_z} max:{max_z} min:{min_z} v:{v}");
            _force_leg = v;
        }
    }
    public bool FootIsOn
    {
        get => _footIsOn;
        set
        {
            if (_footIsOn == value) return;

            _footIsOn = value;
        }
    }
    public Quaternion HeadRotation { get; set; } = Quaternion.identity;
    public IEntity? Focus { get; set; }

    public Humanoid(IScene scene) : base(scene)
    {

    }

    public override void Visit(IVisitor visitor) => visitor.Add(this);
    public override void Forgo(IVisitor visitor) => visitor.Remove(this);

    public void Seek(vec direction_local)
    {
        var f = Force_leg;
        switch (direction_local.z)
        {
        case < 0.001f and > -0.001f:
            var v = vec.Dot(Velocity, Rotation * vec.forward);
            f.z = -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
            break;
        default:
            f += direction_local.z * MOVEMENT_PROMPTNESS * vec.forward;
            break;
        }
        switch (direction_local.x)
        {
        case < 0.001f and > -0.001f:
            var v = vec.Dot(Velocity, Rotation * vec.right);
            f.x = -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
            break;
        default:
            f += direction_local.x * MOVEMENT_PROMPTNESS * vec.right;
            break;
        }
        Force_leg = f;
    }

    public async void Jump(float strength)
    {
        if (!FootIsOn) return;

        OnTransitBodyAnimation(new(A_N_JUMP, true));

        await Task.Delay(JUMP_DELAY_MS);
        Impulse(Position, new(0, 40.0f, 0));
    }

    protected override void Update(float deltaTime)
    {
        if (FootIsOn)
        {
            Rotation.ToAngleAxis(out var angle, out var axis);
            Rotation = Quaternion.AngleAxis(angle, vec.Dot(axis, vec.up) * vec.up);

            Impulse(Position, deltaTime * (Rotation * Force_leg));
        }
        else
        {
            Force_leg = vec.zero;
        }

        base.Update(deltaTime);
    }
}
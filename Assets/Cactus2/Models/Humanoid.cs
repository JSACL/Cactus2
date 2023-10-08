#nullable enable
using static Utils;
using vec = System.Numerics.Vector3;
using qtn = System.Numerics.Quaternion;
using static ConstantValues;
using static System.MathF;
using static System.Math;
using System.Threading.Tasks;
using System;
using System.Numerics;
using Assets.Cactus2;

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

            var speed_z = vec.Dot(Velocity.Linear, vec.Transform(vec.UnitZ, Transform.Rotation));
            var speed_x = vec.Dot(Velocity.Linear, vec.Transform(vec.UnitX, Transform.Rotation));
            var max_z = FORCE_LEG_FORWARD_MAX * Shinkansen300(speed_z / SPEED_FORWARD_MAX);
            var min_z = FORCE_LEG_HORIZONTAL_MIN * Shinkansen300(speed_z / SPEED_HORIZONTAL_MIN);
            var max_x = FORCE_LEG_HORIZONTAL_MAX * Shinkansen300(speed_x / SPEED_HORIZONTAL_MAX);
            var min_x = FORCE_LEG_HORIZONTAL_MIN * Shinkansen300(speed_x / SPEED_HORIZONTAL_MIN);

            v.Z = Clamp(v.Z, min_z, max_z);
            v.X = Clamp(v.X, min_x, max_x);
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
    public qtn HeadRotation { get; set; } = qtn.Identity;
    public ISet<vec> View { get; } = UniversalSet<vec>.Shared;

    public Humanoid(IScene scene) : base(scene)
    {

    }

    public void Seek(vec direction_local)
    {
        var f = Force_leg;
        switch (direction_local.Z)
        {
        case < 0.001f and > -0.001f:
            var v = vec.Dot(Velocity.Linear, vec.Transform(vec.UnitZ, Transform.Rotation));
            f.Z = -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
            break;
        default:
            f += direction_local.Z * MOVEMENT_PROMPTNESS * vec.UnitZ;
            break;
        }
        switch (direction_local.X)
        {
        case < 0.001f and > -0.001f:
            var v = vec.Dot(Velocity.Linear, vec.Transform(vec.UnitX, Transform.Rotation));
            f.X = -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
            break;
        default:
            f += direction_local.X * MOVEMENT_PROMPTNESS * vec.UnitX;
            break;
        }
        Force_leg = f;
    }

    public async void Jump(float strength)
    {
        if (!FootIsOn) return;

        OnTransitAnimation();

        await Task.Delay(JUMP_DELAY_MS);
        Impulse(Transform.Position, new(0, 40.0f, 0));
    }

    protected override void Update(float deltaTime)
    {
        if (FootIsOn)
        {
            float ADJUSTMENT_PROMPTNESS = 1.0f;

            Matrix4x4 matrix = Matrix4x4.CreateFromQuaternion(Transform.Rotation);
            Matrix4x4.Decompose(matrix, out var scale, out var rotation, out var translation);
            vec axis = new vec(rotation.X, rotation.Y, rotation.Z);
            vec gap = axis - vec.UnitY;

            Velocity = new(Velocity.Linear, ADJUSTMENT_PROMPTNESS * gap);
            
            Impulse(Transform.Position, deltaTime * vec.Transform(Force_leg, Transform.Rotation));
        }
        else
        {
            Force_leg = vec.Zero;
        }

        base.Update(deltaTime);
    }

    public void Recognize(Appearance t) => throw new NotImplementedException();
}
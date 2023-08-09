#nullable enable
using UnityEngine;
using static Utils;
using vec = UnityEngine.Vector3;
using static System.MathF;

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

    bool _footIsOn;
    vec _force_leg;

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

            v.z = Confine(v.z, min_z, max_z);
            v.x = Confine(v.x, min_x, max_x);
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

            if (_footIsOn)
            {
                Constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                Constraints = RigidbodyConstraints.None;
            }
        }
    }
    public Quaternion HeadRotation { get; set; } = Quaternion.identity;

    protected override void Elapsed(object? sender, ElapsedEventArgs e)
    {
        base.Elapsed(sender, e);

        //OnImpulse(new(Force_leg));

        //OnTransitBodyAnimation(new("", false));
    }
}
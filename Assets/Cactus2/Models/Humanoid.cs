#nullable enable
using UnityEngine;
using static Utils;
using vec = UnityEngine.Vector3;

public class Humanoid : Animal, IHumanoid
{
    public const float FORCE_REDUCTION_RATE_PER_SEC = 0.001f;
    public const float DEF_FORCE_LEG_FORWARD = 0.5f;
    public const float DEF_FORCE_LEG_HORIZONTAL = 0.3f;
    public const float DEF_FORCE_LEG_UPWARD = 2.5f;
    public const float DEF_FORCE_LEG_DOWNWARD = 0.1f;

    bool _footIsOn;
    vec _force_leg;

    public vec Force_leg
    {
        get => _force_leg;
        set
        {
            var v = value;
            if (v.z >= DEF_FORCE_LEG_FORWARD) v.z = DEF_FORCE_LEG_FORWARD;
            else if (v.z <= -DEF_FORCE_LEG_HORIZONTAL) v.z = -DEF_FORCE_LEG_HORIZONTAL;
            if (v.x >= DEF_FORCE_LEG_HORIZONTAL) v.x = DEF_FORCE_LEG_HORIZONTAL;
            else if (v.x <= -DEF_FORCE_LEG_HORIZONTAL) v.x = -DEF_FORCE_LEG_HORIZONTAL;
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

    protected override void Elapsed(object? sender, ElapsedEventArgs e)
    {
        base.Elapsed(sender, e);

        //OnImpulse(new(Force_leg));

        OnTransitBodyAnimation(new("", false));
    }
}
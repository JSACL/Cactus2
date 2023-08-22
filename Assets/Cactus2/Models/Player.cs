#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using static ConstantValues;
using static UnityEngine.Input;
using static Utils;
using vec = UnityEngine.Vector3;

public class Player : Humanoid, IPlayer
{
    const int JUMPING_DELAY_MS = 100;
    const float STOP_PROMPTNESS = 600f;
    const float MOVEMENT_PROMPTNESS = 180.0f;
    const float STOP_PULL_UP_COEF = 0.2f;

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
        if (JumpingComponent > 0.1f) return;

        JumpingComponent = strength;
        await Task.Delay(JUMPING_DELAY_MS);
        Impulse(Position, new(0, 40.0f, 0));
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }
}

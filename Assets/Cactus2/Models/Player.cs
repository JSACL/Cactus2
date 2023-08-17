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
    const int JUMP_DELAY_MS = 100;
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

        OnTransitBodyAnimation(new(A_N_JUMP, true));

        await Task.Delay(JUMP_DELAY_MS);
        Impulse(Position, new(0, 40.0f, 0));
    }

    protected override void Update(float deltaTime)
    {
        Want(FootIsOn, "FootIsOff");

        // 真偽的動作...押してる間ずっとのやつ
        //if (FootIsOn)
        //{
        //    switch (
        //        GetKey(KeyCode.W),
        //        GetKey(KeyCode.S))
        //    {
        //    case (true, false):
        //        break;
        //    case (false, true):
        //        Force_leg += deltaTime * MOVEMENT_PROMPTNESS * vec.back;
        //        break;
        //    case (false, false):
        //        var f = Force_leg;
        //        var v = vec.Dot(Velocity, Rotation * vec.forward);
        //        f.z = deltaTime * -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
        //        Force_leg = f;
        //        break;
        //    }
        //    switch (
        //        GetKey(KeyCode.A),//GetButton(A_N_MOV_LEFT), 
        //        GetKey(KeyCode.D))//GetButton(A_N_MOV_RIGHT))
        //    {
        //    case (true, false):
        //        Force_leg += deltaTime * MOVEMENT_PROMPTNESS * vec.left;
        //        break;
        //    case (false, true):
        //        Force_leg += deltaTime * MOVEMENT_PROMPTNESS * vec.right;
        //        break;
        //    case (false, false):
        //        var f = Force_leg;
        //        var v = vec.Dot(Velocity, Rotation * vec.right);
        //        f.x = deltaTime * -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
        //        Force_leg = f;
        //        break;
        //    }
        //}

        base.Update(deltaTime);
    }

    //protected override void Elapsed(object? sender, ElapsedEventArgs e)
    //{
    //    //Want(sender is PlayerBehaviour, "Elapsedが想定のゲームオブジェクト以外から送られてきてます。まぁ、大丈夫でしょうが、スレッド系のエラーに悩んだら確認してみてください。");
    //    Want(FootIsOn, "FootIsOff");

    //    // 真偽的動作...押してる間ずっとのやつ
    //    if (FootIsOn)
    //    {
    //        switch (
    //            GetKey(KeyCode.W),
    //            GetKey(KeyCode.S))
    //        {
    //        case (true, false):
    //            Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.forward;
    //            break;
    //        case (false, true):
    //            Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.back;
    //            break;
    //        case (false, false):
    //            var f = Force_leg;
    //            var v = vec.Dot(Velocity, Rotation * vec.forward);
    //            f.z = e.DeltaTime * -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
    //            Force_leg = f;
    //            break;
    //        }
    //        switch (
    //            GetKey(KeyCode.A),//GetButton(A_N_MOV_LEFT), 
    //            GetKey(KeyCode.D))//GetButton(A_N_MOV_RIGHT))
    //        {
    //        case (true, false):
    //            Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.left;
    //            break;
    //        case (false, true):
    //            Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.right;
    //            break;
    //        case (false, false):
    //            var f = Force_leg;
    //            var v = vec.Dot(Velocity, Rotation * vec.right);
    //            f.x = e.DeltaTime * -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
    //            Force_leg = f;
    //            break;
    //        }
    //    }

    //    base.Elapsed(sender, e);
    //}
}

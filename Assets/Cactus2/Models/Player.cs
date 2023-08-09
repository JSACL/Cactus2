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
    const float STOP_PROMPTNESS = 100f;
    const float MOVEMENT_PROMPTNESS = 20.0f;
    const float STOP_PULL_UP_COEF = 0.2f;
    const float MOUSE_SENSITIVILITY = 0.02f;

    public void Move(vec direction)
    {

    }

    protected override void Elapsed(object? sender, ElapsedEventArgs e)
    {
        //Want(sender is PlayerBehaviour, "Elapsedが想定のゲームオブジェクト以外から送られてきてます。まぁ、大丈夫でしょうが、スレッド系のエラーに悩んだら確認してみてください。");
        Want(FootIsOn, "FootIsOff");

        // 真偽的動作...押してる間ずっとのやつ
        if (FootIsOn)
        {
            switch (
                GetKey(KeyCode.W),
                GetKey(KeyCode.S))
            {
            case (true, false):
                Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.forward;
                break;
            case (false, true):
                Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.back;
                break;
            case (false, false):
                var f = Force_leg;
                var v = vec.Dot(Velocity, Rotation * vec.forward);
                f.z = e.DeltaTime * -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
                Force_leg = f;
                break;
            }
            switch (
                GetKey(KeyCode.A),//GetButton(A_N_MOV_LEFT), 
                GetKey(KeyCode.D))//GetButton(A_N_MOV_RIGHT))
            {
            case (true, false):
                Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.left;
                break;
            case (false, true):
                Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.right;
                break;
            case (false, false):
                var f = Force_leg;
                var v = vec.Dot(Velocity, Rotation * vec.right);
                f.x = e.DeltaTime * -STOP_PROMPTNESS * PullUp(STOP_PULL_UP_COEF * v);
                Force_leg = f;
                break;
            }
        }

        base.Elapsed(sender, e);
    }

    public async void Input(Action action, float value)
    {
        switch (action)
        {
        case Action.Jump:
            {
                if (!FootIsOn) break;

                OnTransitBodyAnimation(new(A_N_JUMP, true));

                await Task.Delay(JUMP_DELAY_MS);

                Force_leg += new vec(0, 5, 0);

                await Task.Delay(100);

                Force_leg = new vec(0, 0, 0);

                return;
            }
        case Action.Sit:
            {
                break;
            }
        case Action.Func0:
            {
                // アイテムを使用したり...

                break;
            }
        case Action.Func1:
            {
                break;
            }
        case Action.Func2:
            {
                break;
            }
        case Action.Func3:
            {
                break;
            }
        case Action.Escape:
            {
                break;
            }
        case Action.VerticalRotation:
            {
                HeadRotation = Quaternion.Euler(MOUSE_SENSITIVILITY * value, 0, 0) * HeadRotation;

                break;    
            }
        case Action.HorizontalRotation:
            {
                Rotate(euler: new(0, MOUSE_SENSITIVILITY * value, 0));

                break;
            }
        }
    }
}

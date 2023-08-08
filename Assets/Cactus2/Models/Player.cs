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
    const float STOP_PROMPTNESS = 0.001f;
    const float MOVEMENT_PROMPTNESS = 1f;

    public void Move(vec direction)
    {

    }

    protected override void Elapsed(object? sender, ElapsedEventArgs e)
    {
        //Want(sender is PlayerBehaviour, "Elapsed���z��̃Q�[���I�u�W�F�N�g�ȊO���瑗���Ă��Ă܂��B�܂��A���v�ł��傤���A�X���b�h�n�̃G���[�ɔY�񂾂�m�F���Ă݂Ă��������B");
        Want(FootIsOn, "FootIsOff");

        // �^�U�I����...�����Ă�Ԃ����Ƃ̂��
        if (FootIsOn)
        {
            var f_stop = true;
            if (GetKey(KeyCode.W))//GetButton(A_N_MOV_FORWARD))
            {
                Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.forward;
                f_stop = false;
            }
            if (GetKey(KeyCode.S))//GetButton(A_N_MOV_BACKWARD))
            {
                Force_leg -= e.DeltaTime * MOVEMENT_PROMPTNESS * vec.back;
                f_stop = false;
            }
            switch (
                GetKey(KeyCode.A),//GetButton(A_N_MOV_LEFT), 
                GetKey(KeyCode.D))//GetButton(A_N_MOV_RIGHT))
            {
            case (true, false):
                Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.left;
                f_stop = false;
                break;
            case (false, true):
                Force_leg += e.DeltaTime * MOVEMENT_PROMPTNESS * vec.right;
                f_stop = false;
                break;
            }
            if (f_stop)
            {
                Force_leg *= e.DeltaTime * STOP_PROMPTNESS;
            }
        }

        base.Elapsed(sender, e);
    }

    public void Input(Action action)
    {
        switch (action)
        {
        case Action.Jump:
            {
                if (!FootIsOn) break;

                OnTransitBodyAnimation(new(A_N_JUMP, true));

                Task.Delay(JUMP_DELAY_MS).ContinueWith(_ =>
                {
                    Force_leg += new vec(0, 1, 0);
                });

                break;
            }
        case Action.Sit:
            {
                break;
            }
        case Action.Func0:
            {
                // �A�C�e�����g�p������...

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
        }
    }
}

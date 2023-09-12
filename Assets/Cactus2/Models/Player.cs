#nullable enable
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using static ConstantValues;
using static UnityEngine.Input;
using static Utils;
using vec = UnityEngine.Vector3;

public class Player : Humanoid, IPlayer
{

    int _itemNumber;
    readonly List<IItem> _items;

    public override IVisitor? Visitor
    {
        get => base.Visitor;
        set
        {
            _visitor?.Remove(this);
            _visitor = value;
            _visitor?.Add(this);
        }
    }
    public int SelectedItemIndex
    {
        get => _itemNumber;
        set
        {
            _itemNumber = value % _items.Count;
        }
    }
    public IList<IItem> Items => _items;
    IReadOnlyList<IItem> IPlayer.Items => _items;

    public Player(DateTime time) : base(time)
    {
        _items = new();
    }

    public void Fire(float timeSpan)
    {
        if (_items is IWeapon weapon)
        {
            weapon.Trigger();
        }
    }

    protected override void Update(float deltaTime)
    {
        Want(FootIsOn, "FootIsOff");

        // �^�U�I����...�����Ă�Ԃ����Ƃ̂��
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

        foreach (var item in _items) 
        {
            // TODO: ���̏������͑ӑāB
            if (item is not IEntity entity) continue;
            entity.Position = Position + vec.up;
            entity.Rotation = Rotation * HeadRotation;
        }
        //Items[SelectedItemIndex].Rotation = Rotation;
    }

    //protected override void Elapsed(object? sender, ElapsedEventArgs e)
    //{
    //    //Want(sender is PlayerBehaviour, "Elapsed���z��̃Q�[���I�u�W�F�N�g�ȊO���瑗���Ă��Ă܂��B�܂��A���v�ł��傤���A�X���b�h�n�̃G���[�ɔY�񂾂�m�F���Ă݂Ă��������B");
    //    Want(FootIsOn, "FootIsOff");

    //    // �^�U�I����...�����Ă�Ԃ����Ƃ̂��
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

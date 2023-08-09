#nullable enable
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.MathF;
using static ConstantValues;
using static UnityEngine.Input;
using static Utils;
using vec = UnityEngine.Vector3;

// View-Model
public class PlayerBehaviour : MonoBehaviour
{
    const float ADJUSTMENT_PROMPTNESS = 0.001f;

    IPlayer? _model;
    //vec _position = vec.zero;
    //Quaternion _rotation = Quaternion.identity;
    //vec _velocity = vec.zero;
    //vec _anugularVelocity = vec.zero;
    Rigidbody _rigidbody = null!;
    TriggerSensor _footSensor = null!;
    Animator _animator = null!;
    Camera _camera = null!;

    public IPlayer? Model
    {
        get => _model;
        set
        {
            if (_model is not null)
            {
                _model.TransitBodyAnimation -= TransitBodyAnimation;
                _model = null;
            }
            if (value is not null)
            {
                _model = value;
                _model.TransitBodyAnimation += TransitBodyAnimation;
            }
        }
    }

    void Start()
    {
        var tSs = GetComponentsInChildren<TriggerSensor>();
        var sls = GetComponentsInChildren<Slider>();

        _footSensor = (from tS in tSs where tS.name is PART_N_FOOT select tS).Single();
        //_slider = (from sl in sls where sl.name is "sample" select sl).Single();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Assert(Model is not null);

        // VM -> M のターン
        //var pos_neo = Model!.Position + (transform.position - _position);
        //var rot_neo = (Quaternion.Inverse(_rotation) * transform.rotation) * Model.Rotation;
        Model!.AddTime(Time.deltaTime);
        //Model.Position = pos_neo;//(transform.position - _position) - (pos - Model.Position);
        //Model.Rotation = rot_neo;
        Model.Velocity = _rigidbody.velocity;
        Model.AngularVelocity = _rigidbody.angularVelocity;
        Model.FootIsOn = _footSensor.IsOn;
        if (GetKeyDown(KeyCode.Space)) Model.Input(Action.Jump);
        if (GetMouseButtonDown(0)) Model.Input(Action.Func0);
        if (GetMouseButtonDown(1)) Model.Input(Action.Func1);
        if (GetMouseButtonDown(2)) Model.Input(Action.Func2);

        // M -> VM のターン
        var rate = 1 - Exp(-ADJUSTMENT_PROMPTNESS * Time.deltaTime);
        //_rigidbody.position += rate * (Model.Position - _rigidbody.position);
        _rigidbody.rotation = Quaternion.Euler(rate * (Quaternion.Inverse(_rigidbody.rotation) * Model.Rotation).eulerAngles) * _rigidbody.rotation;
        //_rigidbody.velocity = Model.Velocity;
        _rigidbody.angularVelocity = Model.AngularVelocity;

        // VM -> V を発動
        Bind();
        _rigidbody.constraints = Model.Constraints;
        _rigidbody.AddForce(Model!.Force_leg);
    }

    void FixedUpdate()
    {
        Assert(Model is not null);
    }

    void Adjust(float rate = 1)
    {
        Assert(Model is not null);
        Want(rate is >= 0);

        if (rate is > 1) throw new ArgumentOutOfRangeException();
        if (rate is < 0) return;

        // 位置合わせ
        {
            var delta = rate * (Model!.Position - _rigidbody.position);

            _rigidbody.position += delta;
        }

        // 回転合わせ
        {
            var delta = rate * (Quaternion.Inverse(_rigidbody.rotation) * Model.Rotation).eulerAngles;

            _rigidbody.rotation = Quaternion.Euler(delta) * _rigidbody.rotation;
        }
    }

    void Bind()
    {
        //_rigidbody.position = _position;
        //_rigidbody.rotation = _rotation;
        //_rigidbody.velocity = _velocity;
        //_rigidbody.angularVelocity = _anugularVelocity;
    }

    void TransitBodyAnimation(object? sender, AnimationTransitionEventArgs e)
    {
#warning 応急処置、原因究明を
        if (_animator is null) return;

        Assert(_animator is not null);

        e.Apply(to: _animator!);
    }
}
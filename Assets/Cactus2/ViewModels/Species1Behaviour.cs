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

public sealed class Species1Behaviour : MonoBehaviour
{
    const float ADJUSTMENT_PROMPTNESS = 0.001f;

    ISpecies1? _model;
    vec _position = vec.zero;
    Quaternion _rotation = Quaternion.identity;
    vec _velocity = vec.zero;
    vec _anugularVelocity = vec.zero;
    Rigidbody _rigidbody = null!;
    Animator _animator = null!;

    public ISpecies1? Model
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
        //var tSs = GetComponentsInChildren<TriggerSensor>();
        //var sls = GetComponentsInChildren<Slider>();

        //_footSensor = (from tS in tSs where tS.Name is PART_N_FOOT select tS).Single();
        //_slider = (from sl in sls where sl.name is "sample" select sl).Single();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Assert(Model is not null);

        var pos_neo = Model!.Position + (transform.position - _position);
        var rot_neo = (Quaternion.Inverse(_rotation) * transform.rotation) * Model.Rotation;
        Model.Time += TimeSpan.FromSeconds(Time.deltaTime);
        Model.Position = pos_neo;//(transform.position - _position) - (pos - Model.Position);
        Model.Rotation = rot_neo;

        Adjust(rate: 1 - Exp(-ADJUSTMENT_PROMPTNESS * Time.deltaTime));

        Bind();
    }

    void Adjust(float rate = 1)
    {
        Assert(Model is not null);
        Want(rate is >= 0);

        if (rate is > 1) throw new ArgumentOutOfRangeException();
        if (rate is < 0) return;

        // ˆÊ’u‡‚í‚¹
        {
            var delta = rate * (Model!.Position - _position);

            _position += delta;
        }

        // ‰ñ“]‡‚í‚¹
        {
            var delta = rate * (Quaternion.Inverse(_rotation) * Model.Rotation).eulerAngles;

            _rotation = Quaternion.Euler(delta) * _rotation;
        }
    }

    void Bind()
    {
        _rigidbody.position = _position;
        _rigidbody.rotation = _rotation;
        _rigidbody.velocity = _velocity;
        _rigidbody.angularVelocity = _anugularVelocity;
    }

    void TransitBodyAnimation(object? sender, AnimationTransitionEventArgs e)
    {
        e.Apply(to: _animator);
    }
}
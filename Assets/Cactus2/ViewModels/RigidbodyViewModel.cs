#nullable enable
using System;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;
using qtn = UnityEngine.Quaternion;

public class RigidbodyViewModel<TModel> : ViewModel<TModel> where TModel : class, IEntity
{
    const float POS_ADJUSTMENT_PROMPTNESS = 3f;
    const float ROT_ADJUSTMENT_PROMPTNESS = 100.0f;

    public bool lerp = true;

    [SerializeField]
    Rigidbody _rigidbody;

    public bool UseGravity { get; protected set; }

    protected void OnEnable()
    {
        _rigidbody.position = Model.Position;
        _rigidbody.rotation = Model.Rotation;
    }

    protected void FixedUpdate()
    {
        _rigidbody.position = lerp ? vec.Lerp(Model.Position, _rigidbody.position, Exp(-POS_ADJUSTMENT_PROMPTNESS * Time.deltaTime)) : Model.Position;
        _rigidbody.rotation = lerp ? qtn.Lerp(Model.Rotation, _rigidbody.rotation, Exp(-ROT_ADJUSTMENT_PROMPTNESS * Time.deltaTime)) : Model.Rotation;
        _rigidbody.velocity = Model.Velocity;
        _rigidbody.angularVelocity = Model.AngularVelocity;
        _rigidbody.mass = Model.Mass;
    }

    protected void Update()
    {
        Assert(Model is { });

        Model.AddTime(Time.deltaTime);
    }
}
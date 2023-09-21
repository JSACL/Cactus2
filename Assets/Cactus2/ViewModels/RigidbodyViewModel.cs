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

    [SerializeField]
    Rigidbody _rigidbody;

    protected void FixedUpdate()
    {
        var ratio = 1 - Exp(-POS_ADJUSTMENT_PROMPTNESS * Time.deltaTime);
        _rigidbody.position = vec.Lerp(_rigidbody.position, Model.Position, ratio);
        ratio = 1 - Exp(-ROT_ADJUSTMENT_PROMPTNESS * Time.deltaTime);
        _rigidbody.rotation = qtn.Lerp(_rigidbody.rotation, Model.Rotation, ratio);

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
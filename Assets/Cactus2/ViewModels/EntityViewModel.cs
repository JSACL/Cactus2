#nullable enable
using System;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;
using UE = UnityEngine;

public class EntityViewModel : EntityViewModel<IEntityPresenter>
{

}

public class EntityViewModel<TModel> : ViewModel<TModel> where TModel : class, IEntityPresenter
{
    public new UE::Transform transform;

    protected override void Connect()
    {
        base.Connect();
        Model.PropertyChanged += Reflect;
    }
    protected override void Disconnect()
    {
        Model.PropertyChanged -= Reflect;
        base.Disconnect();
    }

    protected virtual void Reflect()
    {
        transform.SetPositionAndRotation(Model.Transform.Position.ToUnityVector3(), Model.Transform.Rotation.ToUnityQuaternion());
    }

    protected void FixedUpdate() => Model.Elapsed(1);

    protected void Update() => Model.AddTime(Time.deltaTime);

}
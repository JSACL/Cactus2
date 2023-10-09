#nullable enable
using UnityEngine;
using Cactus2.Presenter;

public class RigidbodyViewModel<TModel> : ViewModel<TModel> where TModel : class, IRigidbodyPresenter
{
    public new Rigidbody rigidbody;

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
        rigidbody.position = Model.Transform.Position.ToUnityVector3();
        rigidbody.rotation = Model.Transform.Rotation.ToUnityQuaternion();
        rigidbody.angularVelocity = Model.Velocity.Angular.ToUnityVector3();
        rigidbody.velocity = Model.Velocity.Linear.ToUnityVector3();
        rigidbody.mass = Model.Mass;
    }

    protected void FixedUpdate() => Model.Elapsed(1);

    protected void Update() => Model.AddTime(Time.deltaTime);
}
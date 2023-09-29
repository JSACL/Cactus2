#nullable enable
using System.Collections.Generic;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;
using qtn = UnityEngine.Quaternion;

public class HumanoidViewModel<TModel> : RigidbodyViewModel<TModel> where TModel : class, IHumanoid
{
    public Animator animator_body;
    public Transform transform_eye;
    public ColliderComponent collider_body;
    public TargetComponent target_body;
    public TargetComponent target_foot;

    protected override void Connect()
    {
        base.Connect();
        Model.TransitBodyAnimation += TransitBodyAnimation;
    }
    protected override void Disconnect()
    {
        Model.TransitBodyAnimation -= TransitBodyAnimation;
        base.Disconnect();
    }

    protected void Start()
    {
        target_foot.Executed += (_, e) =>
        {
            e.Command.Execute(Model);
        };
        target_body.Executed += (_, e) =>
        {
            e.Command.Execute(Model);
        };
        collider_body.Stay += (_, e) =>
        {
            Model.Impulse(Model.Position, e.Impulse);
        };
    }

    protected new void Update()
    {
        base.Update();

        Model.Impulse(Model.Position, Time.deltaTime * Model.Mass * Physics.gravity);

        transform_eye.localRotation = Model.HeadRotation;
    }

    void TransitBodyAnimation(object? sender, AnimationTransitionEventArgs e)
    {
        Assert(animator_body is not null);

        e.Apply(to: animator_body);
    }
}
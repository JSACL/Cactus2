#nullable enable
using System.Collections.Generic;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;
using qtn = UnityEngine.Quaternion;

public class HumanoidViewModel<TModel> : RigidbodyViewModel<TModel> where TModel : class, IHumanoid
{
    [SerializeField]
    List<GroundComponent> _groundComponents = new();
    [SerializeField]
    Animator _animator = null!;
    [SerializeField]
    Transform _eyeT = null!;
    [SerializeField]
    ColliderComponent _bodyCES = null!;
    [SerializeField]
    TriggerComponent _footTES = null!;
    [SerializeField]
    TargetComponent _bodyTC = null!;

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
        _bodyTC.Tag = Model.Tag;

        _footTES.Enter += (_, e) =>
        {
            if (e.Other.GetComponent<GroundComponent>() is { } gC)
                _groundComponents.Add(gC);
        };
        _footTES.Exit += (_, e) =>
        {
            if (e.Other.GetComponent<GroundComponent>() is { } gC)
                _groundComponents.Remove(gC);
        };
        _bodyCES.Stay += (_, e) =>
        {
            Model.Impulse(Model.Position, e.Impulse);
        };
        _bodyTC.HitPointInflicted += (_, e) =>
        {
            Model.InflictOnVitality(e);
        };
        _bodyTC.RepairPointInflicted += (_, e) =>
        {
            Model.InflictOnResilience(e);
        };
    }

    protected new void Update()
    {
        base.Update();

        Model.FootIsOn = _groundComponents.Count > 0;
        Model.Impulse(Model.Position, Time.deltaTime * Model.Mass * Physics.gravity);

        _eyeT.localRotation = Model.HeadRotation;
    }

    void TransitBodyAnimation(object? sender, AnimationTransitionEventArgs e)
    {
        Assert(_animator is not null);

        e.Apply(to: _animator);
    }
}
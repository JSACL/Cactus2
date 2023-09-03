#nullable enable
using System.Collections.Generic;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;
using qtn = UnityEngine.Quaternion;

public class HumanoidViewModel : ViewModel<IHumanoid>
{
    const float ADJUSTMENT_PROMPTNESS = 3f;

    [SerializeField]
    List<GroundComponent> _groundComponents = new();
    [SerializeField]
    Rigidbody _rigidbody = null!;
    [SerializeField]
    Animator _animator = null!;
    [SerializeField]
    Camera _camera = null!;
    [SerializeField]
    CollisionEventSource _bodyCES = null!;
    [SerializeField]
    TriggerEventSource _footTES = null!;
    [SerializeField]
    TargetComponent _bodyIES = null!;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _bodyIES.ParticipantInfo = Referee.GetInfo(Model);

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
            Assert(Model is not null);

            Model.Impulse(Model.Position, e.Impulse);
        };
        _bodyIES.HitPointInflicted += async (_, e) =>
        {
            Assert(Model is not null);

            var j = await Referee.Judge(e.issuer, Referee.GetInfo(Model));
            if (j is Judgement.Valid) Model.InflictOnVitality(e.point);
        };
    }

    private void Update()
    {
        Assert(Model is not null);

        Model.FootIsOn = _groundComponents.Count > 0;
        Model.AddTime(Time.deltaTime);
    }

    void FixedUpdate()
    {
        Assert(Model is not null);

        var ratio = 1 - Exp(-ADJUSTMENT_PROMPTNESS * Time.deltaTime);
        _rigidbody.position = vec.Lerp(_rigidbody.position, Model.Position, ratio);
        _rigidbody.rotation = qtn.Lerp(_rigidbody.rotation, Model.Rotation, ratio);

        _rigidbody.velocity = Model.Velocity;
        _rigidbody.angularVelocity = Model.AngularVelocity;
        _camera.transform.localRotation = Model.HeadRotation;
        _rigidbody.mass = Model.Mass;
    }

    void TransitBodyAnimation(object? sender, AnimationTransitionEventArgs e)
    {
        Assert(_animator is not null);

        e.Apply(to: _animator);
    }
}
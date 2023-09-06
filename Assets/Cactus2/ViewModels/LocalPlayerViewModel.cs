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
using qtn = UnityEngine.Quaternion;
using System.Collections.Generic;

public class LocalPlayerViewModel : ViewModel<IPlayer>
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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //_rigidbody = this.GetComponentIC<Rigidbody>("body")!;
        //_animator = this.GetComponentIC<Animator>("body")!;
        //_camera = this.GetComponentIC<Camera>("eye")!;
        //_bodyCES = this.GetComponentIC<CollisionEventSource>("body")!;
        //_footTES = this.GetComponentIC<TriggerEventSource>("foot")!;
        //_bodyTC = this.GetComponentIC<TargetComponent>("body")!;
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

    private void Update()
    {
        Model.IsRunning = GetKey(KeyCode.LeftShift);
        Model.Turn(GetAxis("Mouse X"), GetAxis("Mouse Y"));
        Model.Seek(GetKey(KeyCode.W), GetKey(KeyCode.S), GetKey(KeyCode.D), GetKey(KeyCode.A), Time.deltaTime);
        Model.FootIsOn = _groundComponents.Count > 0;
        if (GetKeyDown(KeyCode.Space)) Model.Jump(1);
        Model.AddTime(Time.deltaTime);
        Model.Impulse(Model.Position, Time.deltaTime * Model.Mass * Physics.gravity);
        if (GetMouseButtonDown(0)) Model.Fire(Time.deltaTime);
    }

    void FixedUpdate()
    {
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
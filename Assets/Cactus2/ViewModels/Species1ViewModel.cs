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
using System.Reflection;

public class Species1ViewModel : ViewModel<ISpecies1>
{
    const float ADJUSTMENT_PROMPTNESS = 3f;

    [Header("Internal")]
    [SerializeField]
    int _groundCount_onFoot;
    [Header("External")]
    [SerializeField]
    Rigidbody _rigidbody = null!;
    [SerializeField]
    CollisionEventSource _bodyCES = null!;
    [SerializeField]
    TriggerEventSource _footTES = null!;

    protected Rigidbody Rigidbody => _rigidbody;
    private protected vec Position
    //{ get; set; }
    { get => _rigidbody.position; set => _rigidbody.position = value; }
    private protected qtn Rotation
    //{ get; set; }
    { get => _rigidbody.rotation; set => _rigidbody.rotation = value; }

    void Start()
    {
        _footTES.Enter += (_, e) => { if (e.Other.gameObject.layer is LAYER_GROUND) _groundCount_onFoot++; };
        _footTES.Exit += (_, e) => { if (e.Other.gameObject.layer is LAYER_GROUND) _groundCount_onFoot--; };
        _bodyCES.Stay += (_, e) => { Model?.Impulse(Model.Position, e.Impulse); };

        Cursor.lockState = CursorLockMode.Locked;

        Model.TargetCoordinates = new Vector3[] { new vec(10, 0, 0) };
    }

    private void Update()
    {
        Assert(Model is not null);

        // V -> VM
        // VM -> M ...?
        Model.AddTime(Time.deltaTime);
    }

    void FixedUpdate()
    {
        Assert(Model is not null);

        // M -> VM
        //Rigidbody.AddForce(10f * (Model.Position - Position));
        //Rigidbody.AddTorque(10f * (qtn.Inverse(Rotation) * Model.Rotation).eulerAngles);
        var ratio = 1 - Exp(-ADJUSTMENT_PROMPTNESS * Time.deltaTime);
        // transform‚ðmodel‚Ö“K‚©‚·B
        //Position += ratio * (Model.Position - Position);
        Position = vec.Lerp(Position, Model.Position, ratio);
        //Debug.Log($"{Model.Position - Position} {Model.Position} {Position}");
        //Rotation = (qtn.Inverse(Rotation) * Model.Rotation).Multiply(by: 1) * Rotation;
        Rotation = qtn.Lerp(Rotation, Model.Rotation, ratio);

        Rigidbody.velocity = Model.Velocity;
        Rigidbody.angularVelocity = Model.AngularVelocity;
        ////Debug.Log($"{(qtn.Inverse(Rotation) * Model.Rotation).eulerAngles} {Rotation}");
        Rigidbody.mass = Model.Mass;

        //// VM -> V
        //Rigidbody.position = Position;
        //Rigidbody.rotation = Rotation;
        //Rigidbody.velocity = Velocity;
        //Rigidbody.angularVelocity = AngularVelocity;
    }
}
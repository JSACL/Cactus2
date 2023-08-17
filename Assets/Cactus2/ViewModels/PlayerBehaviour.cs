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

// View-Model
public class PlayerBehaviour : MonoBehaviour
{
    const float ADJUSTMENT_PROMPTNESS = 3f;

    IPlayer? _model;
    [Header("Internal")]
    [SerializeField]
    int _frameCount;
    [SerializeField]
    int _groundCount_onFoot;
    [Header("External")]
    [SerializeField]
    Rigidbody _rigidbody = null!;
    [SerializeField]
    CollisionEventSource _bodyCES = null!;
    [SerializeField]
    Animator _animator = null!;
    [SerializeField]
    Camera _camera = null!;
    [SerializeField]
    TriggerEventSource _footTES = null!;

    // rigidbodyはView用の状態を持つ(ViewModel)
    // ただしこれはPlayerの状態というより3dモデルの状態。
    // なので別にPlayerとしてのView用の状態=position, rotationを持ち、惰してrigidbodyのそれと一致させる(←これは必然ではない！)

    public IPlayer? Model
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
    protected Rigidbody Rigidbody => _rigidbody;
    protected Camera Camera => _camera;
    private protected qtn CameraRotation_local { get => _camera.transform.localRotation; set => _camera.transform.localRotation = value; }
    private protected vec Position 
        //{ get; set; }
        { get => _rigidbody.position; set => _rigidbody.position = value; }
    private protected qtn Rotation 
        //{ get; set; }
        { get => _rigidbody.rotation; set => _rigidbody.rotation = value; }

    void Start()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _camera = GetComponentInChildren<Camera>();
        _footTES = GetComponentsInChildren<TriggerEventSource>().First();
        _bodyCES = GetComponentsInChildren<CollisionEventSource>().First();

        _footTES.Enter += (_, e) => { if (e.Other.gameObject.layer is LAYER_GROUND) _groundCount_onFoot++; };
        _footTES.Exit += (_, e) => { if (e.Other.gameObject.layer is LAYER_GROUND) _groundCount_onFoot--; };
        _bodyCES.Stay += (_, e) => { Model?.Impulse(Model.Position, e.Impulse); };

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Assert(Model is not null);

        // V -> VM
        // VM -> M ...?
        Model.IsRunning = GetKey(KeyCode.LeftShift);
        Model.Turn(GetAxis("Mouse X"), GetAxis("Mouse Y"));
        //Model.Turn(mousePosition.x - _mousePosition.x, mousePosition.y - _mousePosition.y);
        Model.Seek(GetKey(KeyCode.W), GetKey(KeyCode.S), GetKey(KeyCode.D), GetKey(KeyCode.A), Time.deltaTime);
        Model.FootIsOn = _groundCount_onFoot > 0;
        if (GetKeyDown(KeyCode.Space)) Model.Jump(1);
        Model.AddTime(Time.deltaTime);
        Model.Impulse(Model.Position, Time.deltaTime * Model.Mass * Physics.gravity);
        //var pos_neo = Model.Position + (Position - pos_l); // 1: 先に計算
        //var rot_neo = (qtn.Inverse(rot_l) * Rotation) * Model.Rotation; // 1:
        //Model.Position += Position - pos_l; // 2: 相手を考慮して計算
        //Model.Rotation = (Quaternion.Inverse(rot_l) * Rotation) * Model.Rotation; // 2:
        //Model.Velocity = Rigidbody.velocity;
        //Model.AngularVelocity = Rigidbody.angularVelocity;
        //Model.Position = pos_neo;// 1:打ち消して無効化
        //Model.Rotation = rot_neo;// 1:
        //Model.Input(Action.HorizontalRotation, MousePosition.x - mPos_l.x);
        //Model.Input(Action.VerticalRotation, MousePosition.y - mPos_l.y);

    }

    void FixedUpdate()
    {
        Assert(Model is not null);

        // M -> VM
        //Rigidbody.AddForce(10f * (Model.Position - Position));
        //Rigidbody.AddTorque(10f * (qtn.Inverse(Rotation) * Model.Rotation).eulerAngles);
        var ratio = 1 - Exp(-ADJUSTMENT_PROMPTNESS * Time.deltaTime);
        // transformをmodelへ適かす。
        //Position += ratio * (Model.Position - Position);
        Position = vec.Lerp(Position, Model.Position, ratio);
        //Debug.Log($"{Model.Position - Position} {Model.Position} {Position}");
        //Rotation = (qtn.Inverse(Rotation) * Model.Rotation).Multiply(by: 1) * Rotation;
        Rotation = qtn.Lerp(Rotation, Model.Rotation, ratio);

        Rigidbody.velocity = Model.Velocity;
        Rigidbody.angularVelocity = Model.AngularVelocity;
        ////Debug.Log($"{(qtn.Inverse(Rotation) * Model.Rotation).eulerAngles} {Rotation}");
        CameraRotation_local = Model.HeadRotation;
        Rigidbody.mass = Model.Mass;

        //// VM -> V
        //Rigidbody.position = Position;
        //Rigidbody.rotation = Rotation;
        //Rigidbody.velocity = Velocity;
        //Rigidbody.angularVelocity = AngularVelocity;
    }

    void TransitBodyAnimation(object? sender, AnimationTransitionEventArgs e)
    {
        Assert(_animator is not null);

        e.Apply(to: _animator);
    }
}
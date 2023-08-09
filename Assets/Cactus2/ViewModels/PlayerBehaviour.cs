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
    const float ADJUSTMENT_PROMPTNESS = 120f;

    IPlayer? _model;
    [SerializeField]
    vec _position = vec.zero;
    [SerializeField]
    qtn _rotation = qtn.identity;
    [SerializeField]
    Rigidbody _rigidbody = null!;
    [SerializeField]
    TriggerSensor _footSensor = null!;
    [SerializeField]
    Animator _animator = null!;
    [SerializeField]
    Camera _camera = null!;

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
    protected vec Position
    {
        get => _position;
        set => _position = value;
    }
    protected qtn Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
    protected vec MousePosition { get; private set; }
    protected vec Velocity { get; set; }
    protected vec AngularVelocity { get; set; }
    protected Rigidbody Rigidbody => _rigidbody;

    void Start()
    {
        var tSs = GetComponentsInChildren<TriggerSensor>();
        var sls = GetComponentsInChildren<Slider>();

        _footSensor = (from tS in tSs where tS.name is PART_N_FOOT select tS).Single();
        //_slider = (from sl in sls where sl.name is "sample" select sl).Single();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Assert(Model is not null);

        // V -> VM
        var pos_l = Position;
        var rot_l = Rotation;
        var mPos_l = MousePosition;
        Position = Rigidbody.position;
        Rotation = Rigidbody.rotation;
        Velocity = Rigidbody.velocity;
        AngularVelocity = Rigidbody.angularVelocity;
        MousePosition = mousePosition;
        //Log(false, MousePosition);

        // VM -> M
        var pos_neo = Model.Position + (Position - pos_l); // 1: 先に計算
        var rot_neo = (qtn.Inverse(rot_l) * Rotation) * Model.Rotation; // 1:
        //Model.Position += Position - pos_l; // 2: 相手を考慮して計算
        //Model.Rotation = (Quaternion.Inverse(rot_l) * Rotation) * Model.Rotation; // 2:
        Model.Velocity = Velocity;
        Model.AngularVelocity = AngularVelocity;
        Model.AddTime(Time.deltaTime);
        Model.Position = pos_neo;// 1:打ち消して無効化
        Model.Rotation = rot_neo;// 1:
        Model.FootIsOn = _footSensor.Count >= 2; // TODO: tagで
        //Model.Input(Action.HorizontalRotation, MousePosition.x - mPos_l.x);
        //Model.Input(Action.VerticalRotation, MousePosition.y - mPos_l.y);
        Model.Input(Action.HorizontalRotation, GetKey(KeyCode.RightArrow) ? 100 : GetKey(KeyCode.LeftArrow) ? -100 : 0);
        Model.Input(Action.VerticalRotation, GetKey(KeyCode.UpArrow) ? -100 : GetKey(KeyCode.DownArrow) ? 100 : 0);
        if (GetKeyDown(KeyCode.Space)) Model.Input(Action.Jump, Time.deltaTime);
        if (GetMouseButtonDown(0)) Model.Input(Action.Func0, Time.deltaTime);
        if (GetMouseButtonDown(1)) Model.Input(Action.Func1, Time.deltaTime);
        if (GetMouseButtonDown(2)) Model.Input(Action.Func2, Time.deltaTime);

        // TODO: 場違いなの何とかしてほしい
        ResetMousePosition();

        // M -> VM
        var rate = 1 - Exp(-ADJUSTMENT_PROMPTNESS * Time.deltaTime);
        // transformをmodelへ適かす。
        Position += rate * (Model.Position - Position);
        Rotation = (qtn.Inverse(Rotation) * Model.Rotation).Multiply(by: rate) * Rotation;
        Debug.Log($"{(qtn.Inverse(Rotation) * Model.Rotation).eulerAngles} {Rotation}");
        _camera.transform.localRotation = Model.HeadRotation;
        // TODO: 変
        Rigidbody.constraints = Model.Constraints;

        // VM -> V
        Rigidbody.position = Position;
        Rigidbody.rotation = Rotation;
        Rigidbody.velocity = Velocity;
        Rigidbody.angularVelocity = AngularVelocity;
    }

    void ResetMousePosition()
    {
        vec s = new vec(Screen.width / 2, Screen.height / 2, 0);
        Want((MousePosition - s).sqrMagnitude < 5000, MousePosition - s);
        if ((MousePosition - s).sqrMagnitude > 5000)
        {
            Cursor.lockState = CursorLockMode.Locked;
            MousePosition = s;
        }
        else Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Assert(Model is not null);

        Rigidbody.AddForce(Rotation * Model.Force_leg);
    }

    void TransitBodyAnimation(object? sender, AnimationTransitionEventArgs e)
    {
        Assert(_animator is not null);

        e.Apply(to: _animator);
    }
}
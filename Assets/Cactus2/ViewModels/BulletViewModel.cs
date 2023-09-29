#nullable enable
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using static Utils;

public class BulletViewModel : RigidbodyViewModel<IBullet>
{
    readonly HitCommand _hitCommand = new();

    public Transform tf_collider;
    public AudioClip clip_breakingSE;
    public GameObject obj_hitEffect;
    public AudioSource audioSource;
    public EnterCommandComponent enC_body;
    public TargetComponent tC_body;
    public ColliderComponent col_body;

    [SerializeField]
    bool _adsorbOnApproaching;
    [SerializeField]
    float _thresholdDistance_absorption;
    bool _isAdsorbed;
    //[SerializeField]
    //GameObject _speaker;

    [Header("一定距離に近づいたら目標にくっつく。高速の攻撃を安定させる。")]
    public bool AttachTarget;
    [Header("AttachTargetがOnの時、目標にくっつく距離")]
    public float AttachDistance;
    [HideInInspector]//AttachTargetがOnの時、目標にくっついているかどうかを示す。
    public bool Attached;

    protected override void Connect()
    {
        base.Connect();
        Model.ShowEffect += ShowEffect;
    }
    protected override void Disconnect()
    {
        Model.ShowEffect -= ShowEffect;
        base.Disconnect();
    }

    void Start()
    {
        enC_body.Command = _hitCommand;
        tC_body.Executed += (_, e) =>
        {
            e.Command.Execute(Model);
        };
    }

    new protected void OnEnable()
    {
        base.OnEnable();

        _hitCommand.Authority = Model.Authority;
        _hitCommand.DamageForResilience = 0.002f;
        _hitCommand.DamageForVitality = 0.001f;
        _hitCommand.IsValid = true;

        _isAdsorbed = false;
        obj_hitEffect.SetActive(false);

        if (Model is null) return;

        tf_collider.SetPositionAndRotation(Model.Position, Model.Rotation);
        //if (_speaker != null)
        //{
        //    _speaker.transform.position = transform.position;
        //}
    }

    void ShowEffect(object? sender, EventArgs e)
    {
        obj_hitEffect.transform.position = transform.position;
        obj_hitEffect.SetActive(true);
        audioSource.transform.position = transform.position;
        audioSource.PlayOneShot(clip_breakingSE);
    }
}
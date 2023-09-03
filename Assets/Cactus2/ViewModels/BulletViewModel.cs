#nullable enable
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using static Utils;

public class BulletViewModel : ViewModel<IBullet>
{
    [SerializeField]
    bool _adsorbOnApproaching;
    [SerializeField]
    float _thresholdDistance_absorption;
    bool _isAdsorbed;
    //[SerializeField]
    //GameObject _speaker;
    [SerializeField]
    AudioClip _breakingSE;
    [SerializeField]
    GameObject _hitEffect;
    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    TriggerEventSource _bodyTES;

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
        _bodyTES.Enter += (_, e) =>
        {
            if (e.Other.GetComponentSC<TargetComponent>() is { } tC)
            {
                var pI = Referee.GetInfo(Model);
                if (pI.IsTargeting(tC.ParticipantInfo))
                {
                    tC.InflictToHitPoint(pI, Model.DamageForVitality);
                    tC.InflictToRepairPoint(pI, Model.DamageForResilience);
                    Model.Hit();
                }
            }
        };
    }

    void OnEnable()
    {
        _isAdsorbed = false;
        _hitEffect.SetActive(false);

        transform.position = Model.Position;
        transform.rotation = Model.Rotation;

        //if (_speaker != null)
        //{
        //    _speaker.transform.position = transform.position;
        //}
    }

    void Update()
    {
        Assert(Model is not null);

        Model.AddTime(Time.deltaTime);

        transform.position = Model.Position;
        transform.rotation = Model.Rotation;
    }

    void ShowEffect(object? sender, EventArgs e)
    {
        _hitEffect.transform.position = this.transform.position;
        _hitEffect.SetActive(true);
        _audioSource.transform.position = this.transform.position;
        _audioSource.PlayOneShot(_breakingSE);
    }
}
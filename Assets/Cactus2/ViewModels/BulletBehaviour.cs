#nullable enable
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using static Utils;

public class BulletBehaviour : MonoBehaviour, IViewModel<IBullet>
{
    IBullet? _model;

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

    public IBullet? Model
    {
        get => _model;
        set
        {
            if (_model is { })
            {
                _model.ShowEffect -= ShowEffect;
                _model = null;
            }
            if (value is { })
            {
                _model = value;
                _model.ShowEffect += ShowEffect;
            }
            enabled = _model is { };
        }
    }

    void Start()
    {
        _bodyTES.Enter += (_, e) =>
        {
            Assert(Model is not null);

            if (e.Other.GetComponent<InflictionEventSource>() is { } iES)
            {
                iES.Inflict(Model.Issuer, Model.DamageForHitPoint, Model.DamageForRepairPoint);
                Model.Hit();
            }
        };
    }

    void OnEnable()
    {
        _isAdsorbed = false;
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
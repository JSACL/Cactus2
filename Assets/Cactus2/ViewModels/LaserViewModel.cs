#nullable enable
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static Utils;

public class LaserViewModel : ViewModel<ILaser>
{
    [SerializeField]
    Transform _headT;
    [SerializeField]
    Transform _colliderT;
    [SerializeField]
    AudioClip _breakingSE;
    [SerializeField]
    GameObject _hitEffect;
    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    TriggerComponent _bodyTES;
    [SerializeField]
    HarmfulObjectComponent _bodyHOC;

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
            Referee.JudgeCollisionEnter(_bodyHOC.gameObject, e.Other.gameObject);
        };
        _bodyHOC.Hit += (sender, e) =>
        {
            ShowEffect(sender, e);
        };
    }

    void OnEnable()
    {
        _hitEffect.SetActive(false);
        _bodyHOC.Participant = Model.ParticipantIndex;
        _bodyHOC.DamageForVitality = 0.2f;
        _bodyHOC.DamageForResilience = 0.1f;

        if (Model is null) return;
        _headT.SetPositionAndRotation(Model.Position, Model.Rotation);
        _colliderT.SetPositionAndRotation(Model.Position, Model.Rotation);
        //if (_speaker != null)
        //{
        //    _speaker.transform.position = transform.position;
        //}
    }

    void Update()
    {
        Assert(Model is not null);

        Model.AddTime(Time.deltaTime);

        if (Model is null) return;

        _headT.SetPositionAndRotation(Model.Position, Model.Rotation);
        var mid = Model.Position - 0.5f * Model.Length * (Model.Rotation * Vector3.forward);
        _colliderT.SetPositionAndRotation(mid, Model.Rotation);
        _colliderT.localScale = new(1, 1, Model.Length);
    }

    void ShowEffect(object? sender, EventArgs e)
    {
        _hitEffect.transform.position = _headT.transform.position;
        _hitEffect.SetActive(true);
        _audioSource.transform.position = _headT.transform.position;
        _audioSource.PlayOneShot(_breakingSE);
    }
}
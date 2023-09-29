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
    readonly HitCommand _hitCommand = new();

    public Transform tf_head;
    public Transform tf_collider;
    public AudioClip clip_breakingSE;
    public GameObject obj_hitEffect;
    public AudioSource audioSource;
    public StayCommandComponent stC_body;
    public TargetComponent tC_body;

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
        stC_body.Command = _hitCommand;
        tC_body.Executed += (_, e) =>
        {
            e.Command.Execute(Model);
        };
    }

    protected void OnEnable()
    {
        _hitCommand.Authority = Model.Authority;
        _hitCommand.DamageForResilience = 0.002f;
        _hitCommand.DamageForVitality = 0.001f;
        _hitCommand.IsValid = true;

        obj_hitEffect.SetActive(false);

        if (Model is null) return;
        
        tf_head.SetPositionAndRotation(Model.Position, Model.Rotation);
        tf_collider.SetPositionAndRotation(Model.Position, Model.Rotation);
        //if (_speaker != null)
        //{
        //    _speaker.transform.position = transform.position;
        //}
    }

    protected void Update()
    {
        if (Model is null) return;

        tf_head.SetPositionAndRotation(Model.Position, Model.Rotation);
        var mid = Model.Position - 0.5f * Model.Length * (Model.Rotation * Vector3.forward);
        tf_collider.SetPositionAndRotation(mid, Model.Rotation);
        tf_collider.localScale = new(1, 1, Model.Length);
    }

    void ShowEffect(object? sender, EventArgs e)
    {
        obj_hitEffect.transform.position = tf_head.transform.position;
        obj_hitEffect.SetActive(true);
        audioSource.transform.position = tf_head.transform.position;
        audioSource.PlayOneShot(clip_breakingSE);
    }
}
#nullable enable
using System;
using Cactus2.Presenters;
using Nonno.Assets.Collections;
using UnityEngine;
using Time = UnityEngine.Time;
using UE = UnityEngine;

public class LaserViewModel : ViewModel<ILaserPresenter>
{
    public UE::Transform tf_head;
    public UE::Transform tf_collider;
    public AudioClip clip_breakingSE;
    public GameObject obj_hitEffect;
    public AudioSource audioSource;
    public StayEffectComponent stC_body;
    public TargetComponent tC_body;

    protected override void Connect()
    {
        base.Connect();
        Model.ShowEffect += ShowEffect;
        Model.PropertyChanged += Model_PropertyChanged;
        stC_body.Effect = Model.HitEffect;
    }
    protected override void Disconnect()
    {
        Model.ShowEffect -= ShowEffect;
        Model.PropertyChanged -= Model_PropertyChanged;
        stC_body.Effect = null;
        base.Disconnect();
    }

    void Start()
    {
        tC_body.Command = new DelegatedCommand(Model.Hit);
        obj_hitEffect.SetActive(false);
    }

    protected void Update() => Model.AddTime(Time.deltaTime);
    private void FixedUpdate() => Model.Elapsed();

    void ShowEffect(object? sender, EventArgs e)
    {
        obj_hitEffect.transform.position = tf_head.transform.position;
        obj_hitEffect.SetActive(true);
        audioSource.transform.position = tf_head.transform.position;
        audioSource.PlayOneShot(clip_breakingSE);
    }

    private void Model_PropertyChanged()
    {
        tf_head.Set(Model.Transform);
        tf_collider.Set(Model.Transform);
        tf_collider.localScale = new(1, 1, Model.Length);
    }
}
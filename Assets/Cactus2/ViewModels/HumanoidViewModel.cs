#nullable enable
using System.Collections.Generic;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;
using qtn = UnityEngine.Quaternion;
using Nonno.Assets;
using System;

public class HumanoidViewModel : EntityViewModel<IHumanoidPresenter>
{
    public Animator animator_body;
    public HeadViewModel headViewModel;
    public FootViewModel footViewModel;

    protected override void Connect()
    {
        base.Connect();
        Model.Transit += Model_Transit;
        headViewModel.Model = Model.HeadPresenter;
        footViewModel.Model = Model.FootPresenter;
    }
    protected override void Disconnect()
    {
        Model.Transit -= Model_Transit;
        headViewModel.Model = null;
        footViewModel.Model = null;
        base.Disconnect();
    }

    private void Model_Transit(float obj)
    {
        animator_body.CrossFade(Model.StateIndex.Value, obj, Model.AnimationContext.Layer, Model.AnimationOffset);
    }
}

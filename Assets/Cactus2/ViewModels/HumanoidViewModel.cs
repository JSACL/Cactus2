#nullable enable
using UnityEngine;
using Cactus2.Presenters;
using Nonno.Assets.Collections;
using System;

public class HumanoidViewModel : EntityViewModel<IHumanoidPresenter>
{
    public Animator animator_body;
    public HeadViewModel headViewModel;
    public FootViewModel footViewModel;
    public ControllerViewModel controllerViewModel;

    protected override void Connect()
    {
        base.Connect();
        Model.Transit += Model_Transit;
        Model.FamilyChanged += Model_FamilyChanged;
        headViewModel.Model = Model.HeadPresenter;
        footViewModel.Model = Model.FootPresenter;
    }
    protected override void Disconnect()
    {
        Model.Transit -= Model_Transit;
        Model.FamilyChanged -= Model_FamilyChanged;
        headViewModel.Model = null;
        footViewModel.Model = null;
        base.Disconnect();
    }

    private void Model_Transit(float obj)
    {
        animator_body.CrossFade(Model.StateIndex.Value, obj, Model.AnimationContext.Layer, Model.AnimationOffset);
    }

    private void Model_FamilyChanged(object? sender, Cactus2.FamilyChangeEventArgs e)
    {
        switch (e.Action)
        {
        case Cactus2.SceneChangeAction.Add:
            if (e.Object.Value is IControllerPresenter controllerPresenter) controllerViewModel.Model = controllerPresenter;
            break;
        case Cactus2.SceneChangeAction.Remove:
            if (Equals(controllerViewModel.Model, e.Object.Value)) controllerViewModel.Model = null;
            break;
        }
    }
}

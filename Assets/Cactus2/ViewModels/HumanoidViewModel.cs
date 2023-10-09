#nullable enable
using UnityEngine;
using Cactus2.Presenter;

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

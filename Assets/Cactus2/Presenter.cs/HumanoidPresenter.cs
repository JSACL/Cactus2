#nullable enable
using System;
using System.Numerics;
using Nonno.Assets;

public class HumanoidPresenter<TModel> : EntityPresenter<TModel>, IHumanoidPresenter where TModel : IHumanoid
{
    public HumanoidPresenter()
    {
        HeadPresenter = new HeadPresenter<TModel>() { Model = Model };
        FootPresenter = new FootPresenter<TModel>() { Model = Model };
    }

    public IHeadPresenter HeadPresenter { get; }
    public IFootPresenter FootPresenter { get; }
    public AnimationContext AnimationContext => throw new NotImplementedException();
    public AnimationStateIndex StateIndex => throw new NotImplementedException();
    public float AnimationOffset => throw new NotImplementedException();
    public event Action<float>? Transit;

    public override void AddTime(float deltaTime)
    {
        base.AddTime(deltaTime);

    }
}

public class HeadPresenter<TModel> : Presenter<TModel>, IHeadPresenter where TModel : IHumanoid
{
    public Quaternion HeadLocalRotation => Model.HeadRotation;

    public event FamilyChangeEventHandler? FamilyChanged;
}

public class FootPresenter<TModel> : Presenter<TModel>, IFootPresenter where TModel : IHumanoid
{
    int _stabCount;

    public int StabilizedCount 
    { 
        get => _stabCount;
        private set
        {
            _stabCount = value;
            Model.FootIsOn = _stabCount > 0;
        }
    }

    public void Affect(Typed info)
    {
        if (info.Index == TypeIndex.Of<StabilizationEffect>()) StabilizedCount++;
        if (info.Index == TypeIndex.Of<DestabilizationEffect>()) StabilizedCount--;
    }
}

#nullable enable
using System.Diagnostics;
using System.Numerics;

namespace Cactus2.Presenters;
public class HumanoidPresenter<TModel> : EntityPresenter<TModel>, IHumanoidPresenter where TModel : IHumanoid
{
    readonly HeadPresenter<TModel> _headPresenter;
    readonly FootPresenter<TModel> _footPresenter;
    ControllerPresenter? _controllerPresenter;

    public HumanoidPresenter()
    {
        _headPresenter = new HeadPresenter<TModel>();
        _footPresenter = new FootPresenter<TModel>();
    }

    protected override void Enable()
    {
        _headPresenter.Model = Model; 
        _footPresenter.Model = Model;
        if (Model is IPlayer p) FamilyChanged?.Invoke(this, new(SceneChangeAction.Add, Typed.Get(_controllerPresenter = new ControllerPresenter() { Model = p })));
        base.Enable();
    }
    protected override void Disable() 
    { 
        base.Disable();
        _headPresenter.Model = default;
        _footPresenter.Model = default;
        if (_controllerPresenter is { }) FamilyChanged?.Invoke(this, new(SceneChangeAction.Remove, Typed.Get(_controllerPresenter)));
    }

    public IHeadPresenter HeadPresenter => _headPresenter;
    public IFootPresenter FootPresenter => _footPresenter;
    public AnimationContext AnimationContext => throw new NotImplementedException();
    public AnimationStateIndex StateIndex => throw new NotImplementedException();
    public float AnimationOffset => throw new NotImplementedException();
    public event Action<float>? Transit;
    public event FamilyChangeEventHandler? FamilyChanged;

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
    int _stabCount = 1;// TODO: ‚«‚¿‚ñ‚Æˆ—‚·‚é‚±‚ÆB‚Ù‚ñ‚Æ‚Í0

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
        Debug.WriteLine($"Affect {info.Index.Type}");
        if (info.Index == TypeIndex.Of<StabilizationEffect>()) StabilizedCount++;
        if (info.Index == TypeIndex.Of<DestabilizationEffect>()) StabilizedCount--;
    }
}

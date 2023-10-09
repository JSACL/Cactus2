#nullable enable
using System.Numerics;
using Nonno.Assets.Presentation;
using static System.MathF;

public class EntityPresenter<TModel> : Presenter<TModel>, IEntityPresenter where TModel : IEntity
{
    public bool Lerp { get; set; } = true;
    public float PositionAdjustmentPromptness { get; set; } = 3f;
    public float RotationAdjustmentPromptness { get; set; } = 20f;
    public Transform Transform { get; protected set; }

    public EntityPresenter()
    {
    }

    protected override void Enable()
    {
        Transform = Model.Transform;
        OnPropertyChanged();
    }

    protected override void Disable()
    {
        Transform = Transform.Identity;
        OnPropertyChanged();
    }

    public virtual void AddTime(float deltaTime)
    {
        Model.AddTime(deltaTime);
        if (Model is null) return;
        OnPropertyChanged();
    }

    public void Elapsed(int tickCount)
    {
        var (p, r) = Model.Transform;
        var (pv, rv) = Transform;
        var pn = Lerp ? Vector3.Lerp(p, pv, Exp(-PositionAdjustmentPromptness * tickCount)) : p;
        var rn = Lerp ? Quaternion.Lerp(r, rv, Exp(-RotationAdjustmentPromptness * tickCount)) : r;
        Transform = new(pn, rn);
    }
}
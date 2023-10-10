#nullable enable

namespace Cactus2.Presenters;
public class LaserPresenter<TModel> : RigidbodyPresenter<TModel>, ILaserPresenter where TModel : ILaser
{
    public float Length => Model.Length;
    public Effect HitEffect { get; } = new HitEffect() { DamageForResilience = 0.1f, DamageForVitality = 0.1f};
    public event EventHandler? ShowEffect;

    protected override void Enable() 
    {
        HitEffect.Authority = Model.Authority;
        base.Enable();
    }

    public void Hit(Typed info)
    {
        ShowEffect?.Invoke(this, EventArgs.Empty);
    }
}

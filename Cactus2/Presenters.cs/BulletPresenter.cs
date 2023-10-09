#nullable enable

namespace Cactus2.Presenter;
public class BulletPresenter<TModel> : RigidbodyPresenter<TModel>, IBulletPresenter where TModel : IBullet
{
    public bool IsSticky { get; set; }
    public bool IsSticking { get; private set; }
    public bool StickRadius { get; set; }

    public Effect HitEffect { get; } = new HitEffect() { DamageForResilience = 0.1f, DamageForVitality = 0.1f };
    public event EventHandler? ShowEffect;

    public void Hit(Typed info)
    {
        ShowEffect?.Invoke(this, EventArgs.Empty);
    }
}
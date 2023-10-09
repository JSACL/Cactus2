namespace Cactus2.Presenter;
public class WeaponPresenter<TModel> : ItemPresenter<TModel>, IWeaponPresenter where TModel : IWeapon
{
    public float Value1 => Model.CooldownTimeRemaining / Model.CooldownTime;

    public Transform Transform => Model.Transform;

    public void AddTime(float deltaTime)
    {
        Model.AddTime(deltaTime);
    }
}
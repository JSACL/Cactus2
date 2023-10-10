#nullable enable


namespace Cactus2.Presenters;
public class CombatUIPresenter<TModel> : Presenter<TModel>, ICombatUIPresenter where TModel : IPlayer
{
    public CombatUIPresenter()
    {

    }

    public Transform Transform => Transform.Identity;

    public event FamilyChangeEventHandler? FamilyChanged;

    public void AddTime(float deltaTime)
    {
        Model.AddTime(deltaTime);
    }

    public void Select()
    {
        Model.SelectedItemIndex++;
    }
}
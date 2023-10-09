#nullable enable

using Nonno.Assets;
using Nonno.Assets.Presentation;

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
namespace Cactus2.Presenter;
public class ItemPresenter<TModel> : Presenter<TModel>, IItemPresenter where TModel : IItem
{
    public string Name => Model.Name;

    public string Description => Model.ToString();

    public string Icon => throw new System.NotImplementedException();
}
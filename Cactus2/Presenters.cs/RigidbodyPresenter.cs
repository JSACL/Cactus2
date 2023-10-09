#nullable enable

using Nonno.Assets.Presentation;

public class RigidbodyPresenter<TModel> : EntityPresenter<TModel>, IRigidbodyPresenter where TModel : IEntity
{
    public RigidbodyPresenter()
    {
    }

    public Displacement Velocity => Model.Velocity;
    public float Mass => Model.Mass;
}
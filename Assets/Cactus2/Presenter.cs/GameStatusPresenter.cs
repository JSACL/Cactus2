using Nonno.Assets.Presentation;

public class GameStatusPresenter<TModel> : Presenter<TModel>, IStatusGaugePresenter where TModel : IStatus
{
    public float HP => Model.Vitality;
    public float RP => Model.Resilience;
    public Transform Transform => Transform.Identity;
    public void AddTime(float deltaTime) => Model.AddTime(deltaTime);
}
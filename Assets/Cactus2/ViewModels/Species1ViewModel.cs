#nullable enable
using Cactus2.Presenters;

public class Species1ViewModel : RigidbodyViewModel<IRigidbodyPresenter>
{
    public ColliderComponent _bodyCES = null!;
    public TargetComponent targetComponent;
}
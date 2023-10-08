using Nonno.Assets;

public class FootViewModel : ViewModel<ITargetPresenter>, ICommand
{
    public TargetComponent trigger;

    public bool CanExecute => true;

    public void Execute(Typed info) => Model.Affect(info);

    protected override void Connect()
    {
        trigger.Command = this;
        base.Connect();
    }
    protected override void Disconnect()
    {
        base.Disconnect();
        trigger.Command = this;
    }
}

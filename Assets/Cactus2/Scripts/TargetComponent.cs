#nullable enable
using Nonno.Assets.Collections;

public class TargetComponent : SCComponent
{
    public ICommand? Command { get; set; }
    public void Affect(Typed info)
    {
        if (Command is { } && Command.CanExecute)
            Command.Execute(info);
    }
}
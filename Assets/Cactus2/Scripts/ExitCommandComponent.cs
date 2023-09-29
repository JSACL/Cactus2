using UnityEngine;

public class ExitCommandComponent : SCComponent
{
    public ICommand Command { get; set; }

    private void OnCollisionExit(Collision collision)
    {
        if (Command.IsValid)
        {
            var tC = collision.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Execute(Command);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Command.IsValid)
        {
            var tC = other.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Execute(Command);
        }
    }
}
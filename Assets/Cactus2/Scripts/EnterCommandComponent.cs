using UnityEngine;

public class EnterCommandComponent : SCComponent
{
    public ICommand Command { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (Command.IsValid)
        {
            var tC = collision.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Execute(Command);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Command.IsValid)
        {
            var tC = other.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Execute(Command);
        }
    }
}
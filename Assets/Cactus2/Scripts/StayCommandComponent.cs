using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StayCommandComponent : SCComponent
{
    readonly List<TargetComponent> _targets = new();

    public ICommand Command { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        var tC = collision.gameObject.GetComponentSC<TargetComponent>();
        if (tC != null) _targets.Add(tC);
    }

    private void OnCollisionExit(Collision collision)
    {
        var tC = collision.gameObject.GetComponentSC<TargetComponent>();
        if (tC != null) _targets.Remove(tC);
    }

    private void OnTriggerEnter(Collider other)
    {
        var tC = other.gameObject.GetComponentSC<TargetComponent>();
        if (tC != null) _targets.Add(tC);
    }

    private void OnTriggerExit(Collider other)
    {
        var tC = other.gameObject.GetComponentSC<TargetComponent>();
        if (tC != null) _targets.Remove(tC);
    }

    private void Update()
    {

        if (!Command.IsValid) return;

        foreach (var target in _targets)
        {
            target.Execute(Command);
            if (!Command.IsValid) return;
        }
    }
}
using System.Collections.Generic;
using Cactus2;
using UnityEngine;

public class StayEffectComponent : SCComponent
{
    readonly List<TargetComponent> _targets = new();

    public Effect Effect { get; set; }

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
        if (!Effect.IsValid) return;

        foreach (var target in _targets)
        {
            target.Affect(Effect);
            if (!Effect.IsValid) return;
        }
    }
}
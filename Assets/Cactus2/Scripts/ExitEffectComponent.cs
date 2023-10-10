using Cactus2;
using UnityEngine;

public class ExitEffectComponent : SCComponent
{
    public Effect Effect { get; set; }

    private void OnCollisionExit(Collision collision)
    {
        if (Effect is { } && Effect.IsValid)
        {
            var tC = collision.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Affect(Effect);
        }
    }

    private void OnTriggerExitr(Collider other)
    {
        if (Effect is { } && Effect.IsValid)
        {
            var tC = other.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Affect(Effect);
        }
    }
}
using Cactus2;
using UnityEngine;

public class EnterEffectComponent : SCComponent
{
    public Effect Effect { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (Effect.IsValid)
        {
            var tC = collision.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Affect(Effect);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Effect.IsValid)
        {
            var tC = other.gameObject.GetComponentSC<TargetComponent>();
            if (tC != null) tC.Affect(Effect);
        }
    }
}
#nullable enable

using UnityEngine;
using static Utils;

public class Animal : Entity, IAnimal
{
    public RigidbodyConstraints Constraints { get; set; }

    public event AnimationTransitionEventHandler? TransitBodyAnimation;
    //public event ImpulseEventHandler? Impulse;

    protected void OnTransitBodyAnimation(AnimationTransitionEventArgs e) => TransitBodyAnimation?.Invoke(this, e);

    //protected void OnImpulse(ImpulseEventArgs e) => Impulse?.Invoke(this, e);
}
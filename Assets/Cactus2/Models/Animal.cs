#nullable enable

using UnityEngine;
using static Utils;

public class Animal : Entity, IAnimal
{
    public event AnimationTransitionEventHandler? TransitBodyAnimation;

    protected void OnTransitBodyAnimation(AnimationTransitionEventArgs e) => TransitBodyAnimation?.Invoke(this, e);
}

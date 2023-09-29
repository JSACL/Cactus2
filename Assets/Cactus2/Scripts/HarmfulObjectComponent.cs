using System;
using UnityEngine;

public class HarmfulObjectComponent : SCComponent
{
    public EventHandler Hit;

    public ParticipantIndex Participant { get; set; } = ParticipantIndex.Unknown;
    public float DamageForVitality { get; set; }
    public float DamageForResilience { get; set; }

    public void OnHit() => Hit?.Invoke(this, EventArgs.Empty);
}
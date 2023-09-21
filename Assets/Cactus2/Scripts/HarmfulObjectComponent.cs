using System;
using UnityEngine;

public class HarmfulObjectComponent : SCComponent
{
    public EventHandler Hit;

    public Tag Tag { get; set; } = Tag.Unknown;
    public float DamageForVitality { get; set; }
    public float DamageForResilience { get; set; }

    public void OnHit() => Hit?.Invoke(this, EventArgs.Empty);
}
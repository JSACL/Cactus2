#nullable enable
using System;
using UnityEngine;

public class TargetComponent : SCComponent
{
    public ParticipantInfo ParticipantInfo { get; set; }
    public float HitPoint { get; set; }
    public float RepairPoint { get; set; }
    public event EventHandler<(ParticipantInfo issuer, float point)>? HitPointInflicted;
    public event EventHandler<(ParticipantInfo issuer, float point)>? RepairPointInflicted;

    public void InflictToHitPoint(ParticipantInfo issuer, float point)
    {
        HitPointInflicted?.Invoke(this, (issuer, point));
    }

    public void InflictToRepairPoint(ParticipantInfo issuer, float point)
    {
        RepairPointInflicted?.Invoke(this, (issuer, point));
    }
}
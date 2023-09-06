#nullable enable
using System;
using UnityEngine;

public class TargetComponent : SCComponent
{
    public Tag Tag { get; set; } = Tag.Unknown;
    public float HitPoint { get; set; }
    public float RepairPoint { get; set; }
    public event EventHandler<float>? HitPointInflicted;
    public event EventHandler<float>? RepairPointInflicted;

    public void InflictToHitPoint(float point)
    {
        HitPointInflicted?.Invoke(this, point);
    }

    public void InflictToRepairPoint(float point)
    {
        RepairPointInflicted?.Invoke(this, point);
    }

    [Obsolete("判定は攻撃側が行ったほうがいいと思うのダ。受ける方は為手何人に関わらず甘んじて受けなさい。")]
    public void InflictToHitPoint(Tag issuer, float point)
    {
        HitPointInflicted?.Invoke(this, point);
    }
    [Obsolete("判定は攻撃側が行ったほうがいいと思うのダ。受ける方は為手何人に関わらず甘んじて受けなさい。")]
    public void InflictToRepairPoint(Tag issuer, float point)
    {
        RepairPointInflicted?.Invoke(this, point);
    }
}
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

    [Obsolete("����͍U�������s�����ق��������Ǝv���̃_�B�󂯂���͈׎艽�l�Ɋւ�炸�Â񂶂Ď󂯂Ȃ����B")]
    public void InflictToHitPoint(Tag issuer, float point)
    {
        HitPointInflicted?.Invoke(this, point);
    }
    [Obsolete("����͍U�������s�����ق��������Ǝv���̃_�B�󂯂���͈׎艽�l�Ɋւ�炸�Â񂶂Ď󂯂Ȃ����B")]
    public void InflictToRepairPoint(Tag issuer, float point)
    {
        RepairPointInflicted?.Invoke(this, point);
    }
}
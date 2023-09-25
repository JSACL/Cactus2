#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TargetComponent : SCComponent, IParticipant
{
    LinkedListNode<TargetComponent>? _node;
    ParticipantIndex _tag = ParticipantIndex.Unknown;

    public ParticipantIndex ParticipantIndex
    {
        get => _tag;
        set
        {
            if (_node is null)
            {
                _tag = value;
            }
            else
            {
                _targetComponents[_tag]?.Remove(_node!);
                _tag = value;
                _targetComponents[_tag] ??= new();
                _node = _targetComponents[_tag]!.AddLast(this);
            }
        }
    }
    public event EventHandler<float>? HitPointInflicted;
    public event EventHandler<float>? RepairPointInflicted;
    //public event EventHandler? Aimed;

    void OnEnable()
    {
        if (ParticipantIndex is null) return;
        _targetComponents[ParticipantIndex] ??= new();
        _node = _targetComponents[ParticipantIndex]!.AddLast(this);
    }
    void OnDisable()
    {
        _targetComponents[ParticipantIndex]?.Remove(_node!);
    }

    public void InflictToHitPoint(float point)
    {
        HitPointInflicted?.Invoke(this, point);
    }

    public void InflictToRepairPoint(float point)
    {
        RepairPointInflicted?.Invoke(this, point);
    }

    readonly static CorrespondenceTable<ParticipantIndex, LinkedList<TargetComponent>?> _targetComponents = new(ParticipantIndex.Context);

    public static IEnumerable<TargetComponent> Enableds(ParticipantIndex with) => _targetComponents[with] ?? (IEnumerable<TargetComponent>)Array.Empty<TargetComponent>();
}
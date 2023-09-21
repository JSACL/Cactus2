#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TargetComponent : SCComponent, ITagged
{
    LinkedListNode<TargetComponent>? _node;
    Tag _tag = Tag.Unknown;

    public Tag Tag
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
    public event EventHandler? Aimed;

    void OnEnable()
    {
        if (Tag is null) return;
        _targetComponents[Tag] ??= new();
        _node = _targetComponents[Tag]!.AddLast(this);
    }
    void OnDisable()
    {
        _targetComponents[Tag]?.Remove(_node!);
    }

    public void InflictToHitPoint(float point)
    {
        HitPointInflicted?.Invoke(this, point);
    }

    public void InflictToRepairPoint(float point)
    {
        RepairPointInflicted?.Invoke(this, point);
    }

    public void OnAimed()
    {
        Aimed?.Invoke(this, EventArgs.Empty);
    }

    readonly static CorrespondenceTable<Tag, LinkedList<TargetComponent>?> _targetComponents = new(Tag.Context);

    public static IEnumerable<TargetComponent> Enableds(Tag with) => _targetComponents[with] ?? (IEnumerable<TargetComponent>)Array.Empty<TargetComponent>();
}
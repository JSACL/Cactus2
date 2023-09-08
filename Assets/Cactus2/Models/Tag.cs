#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;

/// <summary>
/// 軄は各実体の役割を一意に表します。
/// </summary>
public class Tag : Context<Tag>.Index
{
    readonly string _name;
    readonly CorrespondenceDictionary<Tag, Relationship> _relationships;

    /// <summary>
    /// 軄を名に拠って作します。
    /// </summary>
    /// <param name="name"></param>
    public Tag(string name) : base(Context)
    {
        _name = name;
        _relationships = new(Context);
    }

    public Relationship GetRelationship(Tag to)
    {
        return _relationships[to];
    }

    public static Tag GetOrCreate(string name)
    {
        foreach (var tag in Context.Indexes) if (tag._name == name) return tag;
        return new(name);
    }

    public new static Context<Tag> Context { get; } = new();

    public static Tag Unknown { get; } = new("Unknown");
    public static Tag NaturalStructure => new("Natural");
}

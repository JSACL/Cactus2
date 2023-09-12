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
public class Tag : Context.Index
{
    readonly string _name;
    readonly CorrespondenceDictionary<Tag, Relationship> _relationships;

    public string Name => _name;

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

    public static new Context<Tag> Context { get; } = new();

    public static Tag GetOrCreate(string name)
    {
        foreach (var tag in Context.Indexes) if (tag.Name == name) return tag;
        return new Tag(name);
    }

    public static Tag Unknown { get; } = new("unknown");
    public static Tag NaturalStructure => new("natural");
}

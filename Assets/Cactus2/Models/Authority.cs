#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;

public class Authority : Context.Index
{
    readonly string _name;
    readonly CorrespondenceTable<Authority, Relationship> _relationships;

    public string Name => _name;

    public Authority(string name) : base(Context)
    {
        _name = name;
        _relationships = new(Context);
    }

    public Relationship GetRelationship(Authority to)
    {
        return _relationships[to];
    }

    public static new Context<Authority> Context { get; } = new();

    public static Authority GetOrCreate(string name)
    {
        foreach (var tag in Context.Indexes) if (tag.Name == name) return tag;
        return new Authority(name);
    }

    public static Authority Unknown { get; } = new("unknown");
    public static Authority Natural { get; } = new("natural");
}

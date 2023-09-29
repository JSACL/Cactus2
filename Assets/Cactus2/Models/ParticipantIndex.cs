#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;

public class ParticipantIndex : Context.Index
{
    readonly string _name;
    readonly CorrespondenceTable<ParticipantIndex, Relationship> _relationships;

    public string Name => _name;

    public ParticipantIndex(string name) : base(Context)
    {
        _name = name;
        _relationships = new(Context);
    }

    public Relationship GetRelationship(ParticipantIndex to)
    {
        return _relationships[to];
    }

    public static new Context<ParticipantIndex> Context { get; } = new();

    public static ParticipantIndex GetOrCreate(string name)
    {
        foreach (var tag in Context.Indexes) if (tag.Name == name) return tag;
        return new ParticipantIndex(name);
    }

    public static ParticipantIndex Unknown { get; } = new("unknown");
    public static ParticipantIndex NaturalStructure => new("natural");
}

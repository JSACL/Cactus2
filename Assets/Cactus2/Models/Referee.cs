#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro.EditorUtilities;

public class Referee
{
    public Referee()
    {
    }

    public Judgement JudgeC(ParticipantInfo offensiveSide, ParticipantInfo defensiveSide)
    {
        var t_o = offensiveSide.Team;
        if (t_o is null) return Judgement.Valid;

        return t_o.IsEnemy(with: defensiveSide.Team) ? Judgement.Valid : Judgement.Invalid;
    }

    public void Join(object obj, object @in)
    {
        SetInfo(obj, GetInfo(of: @in));
    }
    public void Join(object obj, string teamName)
    {
        var team = _participants.Values.FirstOrDefault(x => x.Team is { } team && team.Name == teamName).Team;
        team ??= new Team(this, teamName);
        Join(obj, team);
    }

    static readonly List<Referee> _referees = new() { new() };
    static readonly Dictionary<object?, ParticipantInfo> _participants = new();

    public static Referee Current { get; } = new Referee();

    public static Task<Judgement> Judge(ParticipantInfo offensiveSide, ParticipantInfo defensiveSide)
    {
        var j = Judgement.None;
        foreach (var referee in _referees)
        {
            var j_ = referee.JudgeC(offensiveSide, defensiveSide);
            j = (j, j_) switch
            {
                (Judgement.Error, _) => Judgement.Error,
                (not Judgement.Invalid, Judgement.Valid) => Judgement.Valid,
                (not Judgement.Valid, Judgement.Invalid) => Judgement.Invalid,
                (_, Judgement.Error) => Judgement.Error,
                _ => Judgement.None,
            };
        }
        return Task.FromResult(j);
    }

    public static void SetInfo(object of, ParticipantInfo participantInfo)
    {
        if (participantInfo == ParticipantInfo.Unknown)
        {
            _ = _participants.Remove(of);
        }
        else
        {
            if (!_participants.TryAdd(of, participantInfo))
            {
                _participants[of] = participantInfo;
            }
        }
    }

    public static ParticipantInfo GetInfo(object? of)
    {
        return of is not null && _participants.TryGetValue(of, out var r) ? r : ParticipantInfo.Unknown;
    }
}

public readonly struct ParticipantInfo : IEquatable<ParticipantInfo>
{
    readonly int _identifier;

    public Team? Team => _teams.Length > _identifier ? _teams[_identifier] : null;

    public ParticipantInfo(int identifier)
    {
        _identifier = identifier;
    }

    public bool IsTargeting(ParticipantInfo other)
    {
        return other._identifier != _identifier;
    }

    public override bool Equals(object? obj) => obj is ParticipantInfo info && Equals(info);
    public bool Equals(ParticipantInfo other) => _identifier == other._identifier;
    public override int GetHashCode() => HashCode.Combine(_identifier);

    static readonly Team?[] _teams;

    static ParticipantInfo()
    {
        _teams = Array.Empty<Team>();
    }

    public static ParticipantInfo Unknown => default;
    public static ParticipantInfo NaturalStructure => new(-1);

    public static bool operator ==(ParticipantInfo left, ParticipantInfo right) => left.Equals(right);
    public static bool operator !=(ParticipantInfo left, ParticipantInfo right) => !(left == right);
}

public class Team
{
    readonly Referee _referee;
    readonly List<Team?> _enemies;
    readonly List<Team?> _friends;
    string _name;

    public Referee Referee
    {
        get => _referee;
    }
    public string Name
    {
        get => _name;
        set => _name = value ?? String.Empty;
    }

    public Team(Referee referee, string name = "")
    {
        _referee = referee;
        _name = name;
        _enemies = new();
        _friends = new() { this };
    }

    public bool IsEnemy(Team? with) => _enemies.Contains(with);

    public bool IsFriend(Team? with) => _friends.Contains(with);
}
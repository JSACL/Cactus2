#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro.EditorUtilities;

public class SuperiourReferee : IReferee
{
    readonly List<IReferee> _inferiors;

    public IEnumerable<IReferee> Inferiors => _inferiors;

    public SuperiourReferee()
    {
        _inferiors = new();
    }

    public Judgement Judge(Tag offensiveSide, Tag defensiveSide)
    {
        var j = Judgement.None;
        foreach (var referee in _inferiors)
        {
            var j_ = referee.Judge(offensiveSide, defensiveSide);
            j = (j, j_) switch
            {
                (Judgement.Error, _) => Judgement.Error,
                (not Judgement.Invalid, Judgement.Valid) => Judgement.Valid,
                (not Judgement.Valid, Judgement.Invalid) => Judgement.Invalid,
                (_, Judgement.Error) => Judgement.Error,
                _ => Judgement.None,
            };
        }
        return j;
    }
    public async Task<Judgement> JudgeAsync(Tag offensiveSide, Tag defensiveSide)
    {
        var j = Judgement.None;
        var js = await Task.WhenAll(_inferiors.Select(x => x.JudgeAsync(offensiveSide, defensiveSide)));
        foreach (var j_ in js)
        {
            j = (j, j_) switch
            {
                (Judgement.Error, _) => Judgement.Error,
                (not Judgement.Invalid, Judgement.Valid) => Judgement.Valid,
                (not Judgement.Valid, Judgement.Invalid) => Judgement.Invalid,
                (_, Judgement.Error) => Judgement.Error,
                _ => Judgement.None,
            };
        }
        return j;
    }

    public void Add(IReferee referee)
    {
        _inferiors.Add(referee);
    }

    public void Remove(IReferee referee)
    {
        _inferiors.Remove(referee);
    }

    public static SuperiourReferee Topmost { get; } = new();
}

public class TeamGameReferee : IReferee 
{
    readonly CorrespondenceDictionary<Tag, Team> _teams;

    public TeamGameReferee()
    {
        _teams = new(Tag.Context);
    }

    public Judgement Judge(Tag offensiveSide, Tag defensiveSide)
    {
        if (!_teams.TryGetValue(offensiveSide, out var t_o)) return Judgement.None;
        if (!_teams.TryGetValue(defensiveSide, out var t_d)) return Judgement.None;

        return (t_o.IsEnemy(with: t_d), t_o.IsFriend(with: t_d)) switch
        {
            (true, false) => Judgement.Valid,
            (false, true) => Judgement.Invalid,
            (false, false) => Judgement.None,
            (true, true) => Judgement.Error,
        };
    }

    public class Team
    {
        readonly TeamGameReferee _referee;
        readonly Dictionary<Team?, TeamRelationShip> _relationships;
        string _name;
        Tag? _member;
        Tag? _weapon;

        public Tag? Member
        {
            get => _member;
            set => SetPI(ref _member, value);
        }
        public Tag? Weapon
        {
            get => _weapon;
            set => SetPI(ref _weapon, value);
        }

        public TeamGameReferee Referee
        {
            get => _referee;
        }
        public string Name
        {
            get => _name;
            set => _name = value ?? String.Empty;
        }

        public Team(TeamGameReferee referee, string name = "")
        {
            _referee = referee;
            _relationships = new();
            _name = name;
        }

        public void Regard(Team other, TeamRelationShip @as)
        {
            if (!_referee.Equals(other._referee)) throw new ArgumentException("チームの監督が異なります。", nameof(other));

            _relationships.Add(other, @as);
        }
        public void Disregard(Team other)
        {
            if (!_referee.Equals(other._referee)) throw new ArgumentException("チームの監督が異なります。", nameof(other));

            _relationships.Remove(other);
        }

        public bool IsEnemy(Team? with) => _relationships.TryGetValue(with, out var r) && r == TeamRelationShip.Enemy;
        public bool IsFriend(Team? with) => _relationships.TryGetValue(with, out var r) && r == TeamRelationShip.Friend;

        void SetPI(ref Tag? field, Tag? value)
        {
            if (field is { })
            {
                _referee._teams.Remove(field);
                field = null;
            }
            if (value is { })
            {
                field = value;
                _referee._teams.Add(field, this);
            }
        }
    }
}

#nullable enable
using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;

public class TeamGameReferee : IReferee 
{
    readonly CorrespondenceTable<Authority, Team> _teams;
    readonly CorrespondenceTable<Authority, List<IAuthorized>> _members;

    public TeamGameReferee()
    {
        _teams = new(Authority.Context);
        _members = new(Authority.Context);
    }

    public Team GetTeam(Authority tag) => _teams[tag] ?? throw new KeyNotFoundException();

    public Judgement Judge(Authority offensiveSide, Authority defensiveSide)
    {
        if (_teams[offensiveSide] is not { } t_o) return Judgement.None;
        if (_teams[defensiveSide] is not { } t_d) return Judgement.None;

        return (t_o.IsEnemy(with: t_d), t_o.IsFriend(with: t_d)) switch
        {
            (true, false) => Judgement.Valid,
            (false, true) => Judgement.Invalid,
            (false, false) => Judgement.None,
            (true, true) => Judgement.Error,
        };
    }
    public Judgement Judge(IAuthorized offensiveSide, IAuthorized defensiveSide)
    {
        if (_teams[offensiveSide.Authority] is not { } t_o) return Judgement.None;
        if (_teams[defensiveSide.Authority] is not { } t_d) return Judgement.None;

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
        Authority? _player;
        Authority? _weapon;

        public Authority? Player
        {
            get => _player;
            set => SetPI(ref _player, value);
        }
        public Authority? Weapon
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

        void SetPI(ref Authority? field, Authority? value)
        {
            if (field is { })
            {
                _referee._teams[field] = null;
                field = null;
            }
            if (value is { })
            {
                field = value;
                _referee._teams[field] = this;
            }
        }
    }
}

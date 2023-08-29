#nullable enable
using System.Collections.Generic;

public class Referee
{
    readonly Dictionary<object?, CharacterInfo> _characters = new();

    public Referee()
    {
    }

    public bool IsValid(object? offensiveSide, object? defensiveSide)
    {
        _characters.TryGetValue(offensiveSide, out var cI_o);
        _characters.TryGetValue(defensiveSide, out var cI_d);

        return cI_o.Team.IsEnemy(with: cI_d.Team);
    }

    readonly struct CharacterInfo
    {
        public Team Team { get; }
    }

    public static Referee Current { get; } = new Referee();
}

public class Team
{
    readonly List<Team> _enemies = new();
    readonly List<Team> _friends = new();

    public bool IsEnemy(Team with) => _enemies.Contains(with);

    public bool IsFriend(Team with) => _friends.Contains(with);
}
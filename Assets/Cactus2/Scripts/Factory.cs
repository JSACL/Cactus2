using System;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public LocalVisitor visitor;

    TeamGameReferee _referee;
    TeamGameReferee.Team _friendTeam;
    TeamGameReferee.Team _enemyTeam;

    private void Start()
    {
        var dt = DateTime.Now;

        _referee = new TeamGameReferee();
        IReferee.Current = _referee;
        _friendTeam = new(_referee, "friend")
        {
            Member = new("friend"),
            Weapon = new("friend-weapon"),
        };
        _enemyTeam = new(_referee, "enemy")
        {
            Member = new("enemy"),
            Weapon = new("enemy-weapon"),
        };
        _friendTeam.Regard(_enemyTeam, @as: TeamRelationShip.Enemy);
        _enemyTeam.Regard(_friendTeam, @as: TeamRelationShip.Enemy);

        var p = new Player(dt) 
        { 
            Visitor = visitor, 
            Tag = _friendTeam.Member 
        };
        p.Items.Add(new FugaFirer(dt, _friendTeam) { Visitor = visitor });
        p.Items.Add(new FugaFirer(dt, _friendTeam) { Visitor = visitor});
        p.Items.Add(new FugaFirer(dt, _friendTeam) { Visitor = visitor});
        var s = new FugaEnemy(dt) 
        { 
            Visitor = visitor, 
            Velocity = Vector3.forward, 
            Position = new Vector3(0, 10, 0), 
            Tag = _enemyTeam.Member 
        };
    }
}
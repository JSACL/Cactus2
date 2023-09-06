using UnityEngine;

public class Factory : MonoBehaviour
{
    public LocalVisitor visitor;

    TeamGameReferee _referee;
    TeamGameReferee.Team _friendTeam;
    TeamGameReferee.Team _enemyTeam;

    private void Start()
    {
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

        var p = new Player() 
        { 
            Visitor = visitor, 
            Tag = _friendTeam.Member 
        };
        p.Items.Add(new FugaFirer(_friendTeam) { Visitor = visitor });
        p.Items.Add(new FugaFirer(_friendTeam) { Visitor = visitor});
        p.Items.Add(new FugaFirer(_friendTeam) { Visitor = visitor});
        var s = new FugaEnemy() 
        { 
            Visitor = visitor, 
            Velocity = Vector3.forward, 
            Position = new Vector3(0, 10, 0), 
            Tag = _enemyTeam.Member 
        };
    }
}
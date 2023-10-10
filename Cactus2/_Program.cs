using System;
using Cactus2.Views;

namespace Cactus2;

public class Program
{
    public IScene Scene { get; }

    public Program(DateTime? dateTime = null, IReferee? referee = null)
    {
        Scene = new Scene(dateTime ?? DateTime.Now, referee);
    }

    public void StartGame()
    {

        var t_f = Scene.Referee is not TeamGameReferee t1 ? null : new TeamGameReferee.Team(t1, "friend")
        {
            Player = new("friend"),
            Weapon = new("friend-weapon"),
        };
        var t_e = Scene.Referee is not TeamGameReferee t2 ? null : new TeamGameReferee.Team(t2, "enemy")
        {
            Player = new("enemy"),
            Weapon = new("enemy-weapon"),
        };
        t_f?.Regard(t_e!, @as: TeamRelationShip.Enemy);
        t_e?.Regard(t_f!, @as: TeamRelationShip.Enemy);

        var p = new Player(Scene)
        {
            Authority = t_f?.Player ?? Authority.Unknown,
        };
        p.Items.Add(new FugaFirer(Scene) { BulletIndex = t_f?.Weapon ?? Authority.Unknown });
        p.Items.Add(new FugaFirer(Scene) { BulletIndex = t_f?.Weapon ?? Authority.Unknown  });
        p.Items.Add(new FugaFirer(Scene) { BulletIndex = t_f?.Weapon ?? Authority.Unknown });
        var s = new FugaEnemy(Scene)
        {
            Transform = new(Vec.UnitZ, Qtn.Identity),
            Velocity = new(Vec.UnitZ, Vec.Zero),
            Authority = t_e?.Player ?? Authority.Unknown,
            Gun = new LaserGun(Scene)
            {
                BulletIndex = t_e?.Weapon ?? Authority.Unknown,
            }
        };
    }

    static void Main()
    {
        Console.WriteLine("Hello World!");

        var p = new Program();
        new ConsoleSceneView().Model = p.Scene;
        p.StartGame();
    }
}
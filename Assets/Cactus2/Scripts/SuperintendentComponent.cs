using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class SuperintendentComponent : MonoBehaviour
{
    public bool @break;

    IVisitor _visitor;

    private void Start()
    {
        TaskScheduler.UnobservedTaskException += (sender, e) => Debug.LogError(e);

        var s_d = SceneManager.GetSceneByName("GeneratedScene");

        //SceneManager.LoadSceneAsync(s_d.buildIndex, LoadSceneMode.Additive);
        var v1 = new LocalHostVisitor(s_d);

        //var s_a = SceneManager.GetSceneByName("AnotherScene");
        ////SceneManager.LoadSceneAsync(s_a.buildIndex, LoadSceneMode.Additive);
        //var v2 = new LocalHostVisitor(s_a);

        _visitor = v1;//new CompositeVisitor(v1);

        Init();
    }

    private void Update()
    {
        if (@break)
        {
            ;
            @break = false;
            ;
        }
    }

    void Init()
    {
        var dt = DateTime.Now;
        var r = default(TeamGameReferee);
        IReferee.Current = r = new TeamGameReferee();

        var t_f = new TeamGameReferee.Team(r, "friend")
        {
            Player = new("friend"),
            Weapon = new("friend-weapon"),
        };
        var t_e = new TeamGameReferee.Team(r, "enemy")
        {
            Player = new("enemy"),
            Weapon = new("enemy-weapon"),
        };
        t_f.Regard(t_e, @as: TeamRelationShip.Enemy);
        t_e.Regard(t_f, @as: TeamRelationShip.Enemy);

        var p = new Player(dt)
        {
            Visitor = _visitor,
            ParticipantIndex = t_f.Player,
            Vitality = ConstantValues.PLAYER_VIGOR_STANDARD,
            Resilience = 1.0f,
        };
        p.Died += (sender, e) => { };
        p.Items.Add(new FugaFirer(dt) { Visitor = _visitor, BulletIndex = t_f.Weapon });
        p.Items.Add(new FugaFirer(dt) { Visitor = _visitor, BulletIndex = t_f.Weapon });
        p.Items.Add(new FugaFirer(dt) { Visitor = _visitor, BulletIndex = t_f.Weapon });
        var s = new FugaEnemy(dt)
        {
            Visitor = _visitor,
            Velocity = Vector3.forward,
            Position = new Vector3(0, 10, 0),
            ParticipantIndex = t_e.Player,
            Gun = new LaserGun(dt)
            {
                Visitor = _visitor,
                BulletIndex = t_e.Weapon,
            }
        };
    }
}
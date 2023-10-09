using System;
using System.Threading.Tasks;
using UnityEngine;
using vec = System.Numerics.Vector3;
using qtn = System.Numerics.Quaternion;
using Cactus2;
using Scene = Cactus2.Scene;

public class SuperintendentComponent : MonoBehaviour
{
    public bool @break;
    public SceneViewModel scene;

    Scene _scene;
    TeamGameReferee _referee;
    //IVisitor _visitor;

    private void Start()
    {
        TaskScheduler.UnobservedTaskException += (sender, e) => Debug.LogError(e);

        _scene = new Scene(DateTime.Now, _referee = new TeamGameReferee());
        scene.Model = _scene;
        //var s_d = SceneManager.GetSceneByName("GeneratedScene");

        //SceneManager.LoadSceneAsync(s_d.buildIndex, LoadSceneMode.Additive);
        //var v1 = new LocalHostVisitor(s_d);

        //var s_a = SceneManager.GetSceneByName("AnotherScene");
        ////SceneManager.LoadSceneAsync(s_a.buildIndex, LoadSceneMode.Additive);
        //var v2 = new LocalHostVisitor(s_a);

        //_visitor = v1;//new CompositeVisitor(v1);
        //_scene.Visit(v1);

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
        var t_f = new TeamGameReferee.Team(_referee, "friend")
        {
            Player = new("friend"),
            Weapon = new("friend-weapon"),
        };
        var t_e = new TeamGameReferee.Team(_referee, "enemy")
        {
            Player = new("enemy"),
            Weapon = new("enemy-weapon"),
        };
        t_f.Regard(t_e, @as: TeamRelationShip.Enemy);
        t_e.Regard(t_f, @as: TeamRelationShip.Enemy);

        var p = new Player(_scene)
        {
            Authority = t_f.Player,
        };
        p.Items.Add(new FugaFirer(_scene) { BulletIndex = t_f.Weapon });
        p.Items.Add(new FugaFirer(_scene) { BulletIndex = t_f.Weapon });
        p.Items.Add(new FugaFirer(_scene) { BulletIndex = t_f.Weapon });
        var s = new FugaEnemy(_scene)
        {
            Transform = new(vec.UnitZ, qtn.Identity),
            Velocity = new(vec.UnitZ, vec.Zero),
            Authority = t_e.Player,
            Gun = new LaserGun(_scene)
            {
                BulletIndex = t_e.Weapon,
            }
        };
    }
}
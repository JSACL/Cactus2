using System;
using UnityEngine;
using vec = System.Numerics.Vector3;
using qtn = System.Numerics.Quaternion;
using Cactus2;

[RequireComponent(typeof(SceneViewModel))]
public class EntryPointComponent : MonoBehaviour
{
    Scene _scene;
    TeamGameReferee _referee;

    private void Awake()
    {
        _scene = new Scene(DateTime.Now, _referee = new TeamGameReferee());
        var svm = GetComponent<SceneViewModel>();
        svm.Model = _scene;

        Init();
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
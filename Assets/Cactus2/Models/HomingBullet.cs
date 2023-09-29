#nullable enable
using System;

public class HomingBullet : Bullet, IHoming
{
    public HomingBullet(IScene scene) : base(scene)
    {

    }
}
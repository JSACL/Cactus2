#nullable enable
using System;

public class HomingBullet : Bullet, IHoming
{
    public HomingBullet(DateTime time) : base(time)
    {

    }
}
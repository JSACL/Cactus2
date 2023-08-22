#nullable enable
using System;
using System.IO;
using UnityEngine;

public interface IEntity
{
    Vector3 Position { get; protected set; }
    Vector3 Velocity { get; }
    Quaternion Rotation { get; protected set; }
    Vector3 AngularVelocity { get; }
    DateTime Time { get; protected set; }
    float Mass { get; }

    void Rotate(float angle, Vector3 axis) => Rotate(Quaternion.AngleAxis(angle, axis));
    [Obsolete]
    void Rotate(Vector3 euler) => Rotate(Quaternion.Euler(euler));
    void Rotate(Quaternion rotation) => Rotation = rotation * Rotation;
    void AddTime(float deltaTime) => Time += TimeSpan.FromSeconds(deltaTime);
    void Impulse(Vector3 at, Vector3 impulse);
}

public interface IAnimal : IEntity
{
}

public interface IHumanoid : IAnimal
{
#warning 公の設定役なかるべし。
    bool IsRunning { get; set; }
    bool FootIsOn { get; set; }
    Quaternion HeadRotation { get; protected set; }
    float JumpingComponent { get; }

    void Turn(Vector3 to) => HeadRotation = Quaternion.Euler(to - Position);
}

public interface IPlayer : IHumanoid
{
    internal const float DEFAULT_MOUSE_SENSITIVILITY = 1f;

    void Seek(bool forward, bool backward, bool right, bool left, float strength)
    {
        var x = 0f;
        if (right) x++;
        if (left) x--;
        var z = 0f;
        if (forward) z++;
        if (backward) z--;
        Seek(strength * new Vector3(x, 0, z));
    }
    void Seek(Vector3 direction_local);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strength">
    /// 短打鍵一回を単位とする強さ。
    /// </param>
    void Jump(float strength);
    void Turn(float horizontal, float vertical)
    {
        //HeadRotation = Quaternion.AngleAxis(DEFAULT_MOUSE_SENSITIVILITY * horizontal, Vector3.up) * Quaternion.AngleAxis(DEFAULT_MOUSE_SENSITIVILITY * vertical, HeadRotation * Vector3.right) * HeadRotation;

        var a = HeadRotation.eulerAngles;
        if (a.x < 80 || a.x > 280 || (a.x <= 180 && vertical < 0) || (a.x >= 180 && vertical > 0))
            HeadRotation = Quaternion.AngleAxis(DEFAULT_MOUSE_SENSITIVILITY * vertical, Vector3.left) * HeadRotation;
        Rotate(DEFAULT_MOUSE_SENSITIVILITY * horizontal, Vector3.up);
    }
}

public interface ISpecies1 : IAnimal
{

}

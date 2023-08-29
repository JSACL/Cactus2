#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IReferee
{

}

public interface IVisible
{
    IVisitor? Visitor { get; set; }
}

public interface IEntity : IVisible
{
    Vector3 Position { get; set; }
    Vector3 Velocity { get; }
    Quaternion Rotation { get; set; }
    Vector3 AngularVelocity { get; }
    DateTime Time { get; set; }
    float Mass { get; }

    void Rotate(float angle, Vector3 axis) => Rotate(Quaternion.AngleAxis(angle, axis));
    [Obsolete]
    void Rotate(Vector3 euler) => Rotate(Quaternion.Euler(euler));
    void Rotate(Quaternion rotation) => Rotation = rotation * Rotation;
    void AddTime(float deltaTime) => Time += TimeSpan.FromSeconds(deltaTime);
    void Impulse(Vector3 at, Vector3 impulse);
}

public interface IFirer : IEntity
{
    float CooldownTimeRemaining { get; }
    float CooldownTime { get; }

    void Fire(IEntity issuer, IEntity? target);
}

public interface IBullet : IEntity
{
    IEntity? Issuer { get; }
    float DamageForHitPoint { get; }
    float DamageForRepairPoint { get; }
    event EventHandler? ShowEffect;

    void Hit();
}

public interface IEphemeral
{
    float Vigor { get; }
}

public interface ICharacter
{
    float? HitPoint { get; }
    float? RepairPoint { get; }

    void Inflict(float damageForHP, float damageForRP);
}

public interface IAnimal : IEntity, IEphemeral
{
    event AnimationTransitionEventHandler? TransitBodyAnimation;
}

public interface IHumanoid : IAnimal
{
    bool IsRunning { get; set; }
    bool FootIsOn { get; set; }
    Quaternion HeadRotation { get; set; }
    IEntity? Focus { get; set; }

    void Turn(Vector3 to) => HeadRotation = Quaternion.Euler(to - Position);
}

public interface IPlayer : IHumanoid
{
    internal const float DEFAULT_MOUSE_SENSITIVILITY = 1f;

    int SelectedItemIndex { get; set; }
    ReadOnlySpan<IFirer> Items { get; }

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
    void Jump(float strength);
    void Turn(float horizontal, float vertical)
    {
        //HeadRotation = Quaternion.AngleAxis(DEFAULT_MOUSE_SENSITIVILITY * horizontal, Vector3.up) * Quaternion.AngleAxis(DEFAULT_MOUSE_SENSITIVILITY * vertical, HeadRotation * Vector3.right) * HeadRotation;

        var a = HeadRotation.eulerAngles;
        if (a.x < 80 || a.x > 280 || (a.x <= 180 && vertical < 0) || (a.x >= 180 && vertical > 0))
            HeadRotation = Quaternion.AngleAxis(DEFAULT_MOUSE_SENSITIVILITY * vertical, Vector3.left) * HeadRotation;
        Rotate(DEFAULT_MOUSE_SENSITIVILITY * horizontal, Vector3.up);
    }
    void Fire(float timeSpan);
}

public interface ISpecies1 : IAnimal
{

}

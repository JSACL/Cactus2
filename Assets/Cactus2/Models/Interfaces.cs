#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public interface IReferee
{
    Judgement Judge(ParticipantIndex offensiveSide, ParticipantIndex defensiveSide);
    Task<Judgement> JudgeAsync(ParticipantIndex offensiveSide, ParticipantIndex defensiveSide) => Task.FromResult(Judge(offensiveSide, defensiveSide));
    [Obsolete]
    Judgement Judge(IParticipant offensiveSide, IParticipant defensiveSide);
    [Obsolete]
    Task<Judgement> JudgeAsync(IParticipant offensiveSide, IParticipant defensiveSide) => Task.FromResult(Judge(offensiveSide, defensiveSide));

    static IReferee Current { get; set; } = SuperiorReferee.Topmost;
}

public interface IVisible
{
    IVisitor? Visitor { get; set; }
}

public interface IParticipant
{
    ParticipantIndex ParticipantIndex => ParticipantIndex.Unknown;
}

public interface IEntity : IVisible, IParticipant
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
    // TODO; 甘えなきもする。違反ではないが。
    bool TrySetTag(ParticipantIndex tag) => false;
}

public interface IItem
{
    //void Use(object? user);
}

public interface IWeapon : IEntity, IItem
{
    float CooldownTimeRemaining { get; }
    float CooldownTime { get; }

    void Trigger();

    //void IItem.Use(object? user) => Trigger(Referee.GetInfo(of: user));
}

public interface IFirer : IWeapon
{
}

public interface IBullet : IEntity
{
    float DamageForVitality { get; }
    float DamageForResilience { get; }

    event EventHandler? ShowEffect;

    void Hit();
}

public interface ILaser : IEntity
{
    float DamageForVitality { get; }
    float DamageForResilience { get; }
    float Length { get; }

    event EventHandler? ShowEffect;

    void Hit();
}

public interface IHoming : IEntity
{
    Vector3? TargetCoordinate { get; set; }
}

public interface IEphemeral
{
    float Vitality { get; }
    float Resilience { get; }
    event EventHandler? Died;

    void InflictOnVitality(float damage);
    void InflictOnResilience(float damage);
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
        var a = HeadRotation.eulerAngles;
        if (a.x < 80 || a.x > 280 || (a.x <= 180 && vertical < 0) || (a.x >= 180 && vertical > 0))
            HeadRotation = Quaternion.AngleAxis(vertical, Vector3.left) * HeadRotation;
        Rotate(horizontal, Vector3.up);
    }
}

public interface IGround
{

}

public interface IPlayer : IHumanoid
{
    int SelectedItemIndex { get; set; }
    IReadOnlyList<IItem> Items { get; }

    void Fire(float timeSpan);
}

public interface ISpecies1 : IAnimal
{
    IEnumerable<Vector3> TargetPositions { get; set; }
}

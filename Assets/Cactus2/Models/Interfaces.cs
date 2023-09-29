#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public interface IReferee
{
    Judgement Judge(Authority offensiveSide, Authority defensiveSide);
    Task<Judgement> JudgeAsync(Authority offensiveSide, Authority defensiveSide) => Task.FromResult(Judge(offensiveSide, defensiveSide));
    [Obsolete]
    Judgement Judge(IAuthorized offensiveSide, IAuthorized defensiveSide);
    [Obsolete]
    Task<Judgement> JudgeAsync(IAuthorized offensiveSide, IAuthorized defensiveSide) => Task.FromResult(Judge(offensiveSide, defensiveSide));

    static IReferee Current { get; set; } = SuperiorReferee.Topmost;
}

public interface IVisible
{
    void Visit(IVisitor visitor);
    void Forgo(IVisitor visitor);
}

public interface ITransitory
{
    DateTime Time { get; set; }

    void AddTime(float seconds) => Time += TimeSpan.FromSeconds(seconds);
}

public interface IScene : IVisible, ITransitory
{
    IReferee Referee { get; }

    void Add(object obj) => Add(obj, assertIsNotVisible: false);
    void Add(object obj, bool assertIsNotVisible = false);
    void Add(IVisible obj);
    void Remove(object obj) => Remove(obj, assertIsNotVisible: false);
    void Remove(object obj, bool assertIsNotVisible = false);
    void Remove(IVisible obj);
}

public interface IAuthorized
{
    Authority Authority => Authority.Unknown;
}

public interface IEntity : IVisible, IAuthorized, ITransitory
{
    Vector3 Position { get; set; }
    Vector3 Velocity { get; }
    Quaternion Rotation { get; set; }
    Vector3 AngularVelocity { get; }
    IScene Scene { get; }
    float Mass { get; }

    void Rotate(float angle, Vector3 axis) => Rotate(Quaternion.AngleAxis(angle, axis));
    [Obsolete]
    void Rotate(Vector3 euler) => Rotate(Quaternion.Euler(euler));
    void Rotate(Quaternion rotation) => Rotation = rotation * Rotation;
    void Impulse(Vector3 at, Vector3 impulse);
}

public interface ICommand
{
    Authority Authority { get; }
    bool IsValid { get; }

    void Execute(object obj);
    Task ExecuteAsync(object obj)
    {
        Execute(obj);
        return Task.CompletedTask;
    }
}

public interface ICommand<T> : ICommand
{
    void Execute(T obj);
    Task ExecuteAsync(T obj)
    {
        Execute(obj);
        return Task.CompletedTask;
    }
    void ICommand.Execute(object obj)
    {
        if (IsValid && obj is T t) Execute(t);
    }
    Task ICommand.ExecuteAsync(object obj)
    {
        if (IsValid && obj is T t) Execute(t);
        return Task.CompletedTask;
    }
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

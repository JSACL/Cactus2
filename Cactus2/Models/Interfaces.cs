#nullable enable
using System.Numerics;
using System.Threading.Tasks;

namespace Cactus2;
public interface IReferee
{
    Judgement Judge(Authority offensiveSide, Authority defensiveSide);
    Task<Judgement> JudgeAsync(Authority offensiveSide, Authority defensiveSide) => Task.FromResult(Judge(offensiveSide, defensiveSide));
    [Obsolete]
    Judgement Judge(IAuthorized offensiveSide, IAuthorized defensiveSide);
    [Obsolete]
    Task<Judgement> JudgeAsync(IAuthorized offensiveSide, IAuthorized defensiveSide) => Task.FromResult(Judge(offensiveSide, defensiveSide));
}

public interface ITransitory
{
    DateTime Time { get; set; }

    void AddTime(float seconds) => Time += TimeSpan.FromSeconds(seconds);
}

public interface IStatus : ITransitory
{
    float Vitality { get; }
    float Resilience { get; }
    event EventHandler? Updated;
    void Affect(Typed effect);
}

public interface IFamily
{
    void Add(Typed obj);
    void Remove(Typed obj);
}

public interface IScene : ITransitory, IFamily
{
    IReferee Referee { get; }
}

public interface IEntity : IAuthorized, ITransitory
{
    Transform Transform { get; set; }
    Displacement Velocity { get; set; }
    IScene Scene { get; }
    float Mass { get; }

    void Rotate(float angle, Vector3 axis) => Rotate(Quaternion.CreateFromAxisAngle(axis, angle));
    void Rotate(Quaternion rotation) => Transform = new(Transform.Position, rotation * Transform.Rotation);
    void Impulse(Vector3 at, Vector3 impulse);
}

public interface ISet<T>
{
    bool Contains(T element);
}

public interface IItem
{
    string Name { get; }
}

public interface IWeapon : IEntity, IItem
{
    float CooldownTimeRemaining { get; }
    float CooldownTime { get; }

    void Trigger();
}

public interface IBullet : IEntity
{
    HitEffect HitEffect { get; }
}

public interface ILaser : IEntity
{
    HitEffect HitEffect { get; }
    float Length { get; }
}

public interface ICharacter
{
    IStatus Status { get; }
}

public interface IAnimal : IEntity
{
    event EventHandler? TransitAnimation;
}

public interface IHumanoid : IAnimal, IViewer
{
    bool IsRunning { get; set; }
    bool FootIsOn { get; set; }
    Quaternion HeadRotation { get; set; }

    void Turn(Vector3 to) => HeadRotation = Utils.LookRotation(to - Transform.Position, Vector3.UnitZ);
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
        var a = Vector3.Transform(Vector3.UnitZ, HeadRotation);
        if (a.X is < 1.5f and > -1.5f || (a.X > 0 && vertical < 0) || (a.X < 0 && vertical > 0))            HeadRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, vertical) * HeadRotation;
        Rotate(horizontal, Vector3.UnitY);
    }
}

public interface IGround
{ 
}

public interface IPlayer : IHumanoid, ICharacter
{
    int SelectedItemIndex { get; set; }
    SysGC::IReadOnlyList<IItem> Items { get; }

    void Fire(float timeSpan);
}

public interface ICognitive<T>
{
    void Recognize(T t);
}

public interface IViewer : ICognitive<Appearance>
{
    ISet<Vector3> View { get; }
}

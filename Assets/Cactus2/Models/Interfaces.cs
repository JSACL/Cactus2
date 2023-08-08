#nullable enable
using System;
using System.IO;
using UnityEngine;

public interface IEntity
{
    Vector3 Position { get; set; }
    Vector3 Velocity { get; set; }
    Quaternion Rotation { get; set; }
    Vector3 AngularVelocity { get; set; }
    DateTime Time { get; set; }

    void Rotate(Vector3 euler);
    void Rotate(Quaternion rotation);
    void AddTime(float deltaTime);
}

public interface IAnimal : IEntity
{
    event AnimationTransitionEventHandler? TransitBodyAnimation;
    //event ImpulseEventHandler? Impulse;

    RigidbodyConstraints Constraints { get; }
}

public interface IHumanoid : IAnimal
{
    bool FootIsOn { get; set; }
    Vector3 Force_leg { get; set; }
}

public interface IPlayer : IHumanoid
{
    void Input(Action action);
}

public interface ISpecies1 : IAnimal
{

}
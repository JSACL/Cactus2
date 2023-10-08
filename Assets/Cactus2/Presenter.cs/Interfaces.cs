#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nonno.Assets;
using Nonno.Assets.Presentation;
using UnityEngine;
using Transform = Nonno.Assets.Presentation.Transform;

public interface IVariablePresenter
{
    event Action? PropertyChanged;
}

public interface IEntityPresenter : IVariablePresenter
{
    Transform Transform { get; }

    void AddTime(float deltaTime);
    void Elapsed(int tickCount = 1) { }
}

public interface IRigidbodyPresenter : IEntityPresenter
{
    Displacement Velocity { get; }
    float Mass { get; }
}

public interface ICombatUIPresenter : IEntityPresenter, IFamilyPresenter
{
    void Select();
}

public interface IFamilyPresenter
{
    event FamilyChangeEventHandler? FamilyChanged;
}

public interface IItemPresenter : IVariablePresenter
{
    string Name { get; }
    string Description { get; }
    Texture Icon { get; }
}

public interface IWeaponPresenter : IItemPresenter, IEntityPresenter
{
    float Value1 { get; }
}

public interface IStatusGaugePresenter : IEntityPresenter
{
    float HP { get; }
    float RP { get; }
}

public interface IBulletPresenter : IRigidbodyPresenter
{
    Effect HitEffect { get; }
    event EventHandler? ShowEffect;
    void Hit(Typed info);
}

public interface ILaserPresenter : IRigidbodyPresenter
{
    Effect HitEffect { get; }
    event EventHandler? ShowEffect;
    float Length { get; }
    void Hit(Typed info);
}

public interface IHumanoidPresenter : IEntityPresenter, IAnimationPresenter
{
    IFootPresenter FootPresenter { get; }
    IHeadPresenter HeadPresenter { get; }
}

[Obsolete]
public interface ICollectionPresenter<T> : IReadOnlyCollection<T>
{
    event CollectionChangeEventHandler<T>? CollectionChanged;
}

public interface IAnimationPresenter
{
    event Action<float>? Transit;
    AnimationContext AnimationContext { get; }
    AnimationStateIndex StateIndex { get; }
    float AnimationOffset { get; }
}

public interface ITargetPresenter
{
    void Affect(Typed info);
}

public interface IFootPresenter : ITargetPresenter
{

}

public interface IHeadPresenter : IVariablePresenter
{
    System.Numerics.Quaternion HeadLocalRotation { get; }
    event FamilyChangeEventHandler? FamilyChanged;
}

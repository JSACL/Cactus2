#nullable enable

using System;
using UnityEditorInternal;
using UnityEngine;
using static Utils;

public class Animal : Entity, IAnimal
{
    public Animal()
    {
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }
}

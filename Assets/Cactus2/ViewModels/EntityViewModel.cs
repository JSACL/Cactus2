#nullable enable
using System;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;

public class EntityViewModel : ViewModel<IEntity>
{
    protected void Update()
    {
        Assert(Model is { });

        Model.AddTime(Time.deltaTime);
    }

    protected void LateUpdate()
    {
        if (Model is null) return;

        transform.SetPositionAndRotation(Model.Position, Model.Rotation);
    }
}
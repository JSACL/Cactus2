#nullable enable
using System;
using UnityEngine;
using static Utils;
using static System.MathF;
using vec = UnityEngine.Vector3;

public class EntityViewModel : ViewModel<IEntity>
{
    private void Update()
    {
        Assert(Model is { });

        Model.AddTime(Time.deltaTime);

        transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using static ConstantValues;
using static UnityEngine.Input;
using static Utils;
using vec = UnityEngine.Vector3;

public class FugaEnemy : Animal, ISpecies1
{
    public override IVisitor? Visitor
    {
        get => base.Visitor;
        set
        {
            _visitor?.Remove(this);
            _visitor = value;
            _visitor?.Add(this);
        }
    }

    public const float FORCE_REDUCTION_RATE_PER_SEC = 0.001f;
}

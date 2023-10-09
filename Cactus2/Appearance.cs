using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nonno.Assets;
using Nonno.Assets.Presentation;

public readonly struct Appearance
{
    public Appearance(int hashCode, Transform transform, Displacement velocity, Authority authority)
    {
        HashCode = hashCode;
        Transform = transform;
        Velocity = velocity;
        Authority = authority;
    }

    public int HashCode { get; }
    public Transform Transform { get; }
    public Displacement Velocity { get; }
    public Authority Authority { get; }
}

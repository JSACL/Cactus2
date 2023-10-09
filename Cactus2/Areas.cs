namespace Cactus2;
public class UniversalSet<T> : ISet<T>
{
    public bool Contains(T element) => true;

    public static UniversalSet<T> Shared { get; } = new();
}

public class TrapezoidalPrism : ISet<Vec>
{
    Transform _inverse;
    public Transform Transform
    {
        get => -_inverse;
        set => _inverse = -value;
    }
    public float SlopeX { get; set; }
    public float SlopeY { get; set; }
    public float MinZ { get; }
    public float MaxZ { get; }

    public bool Contains(Vec element)
    {
        var p1 = element + _inverse.Position;
        var p2 = Vec.Transform(p1, _inverse.Rotation);

        if (p2.Z < MinZ || MaxZ < p2.Z) return false;
        var x = p2.Z * SlopeX;
        if (p2.X < -x || x < p2.X) return false;
        var y = p2.Z * SlopeY;
        if (p2.Y < -y || y < p2.Y) return false;
        return true;
    }
}

public class Sphere : ISet<Vec>
{
    float _r;
    float _r_sq;

    public Vec Center { get; set; }
    public float Radius
    {
        get => _r;
        set
        {
            _r = value;
            _r_sq = value * value;
        }
    }

    public bool Contains(Vec element) => Vec.DistanceSquared(element, Center) <= _r_sq;
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nonno.Assets.Presentation;

namespace Assets.Cactus2
{
    public class UniversalSet<T> : ISet<T>
    {
        public bool Contains(T element) => true;

        public static UniversalSet<T> Shared { get; } = new();
    }

    public class TrapezoidalPrism : ISet<Vector3>
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

        public bool Contains(Vector3 element)
        {
            var p1 = element + _inverse.Position;
            var p2 = Vector3.Transform(p1, _inverse.Rotation);

            if (p2.Z < MinZ || MaxZ < p2.Z) return false;
            var x = p2.Z * SlopeX;
            if (p2.X < -x || x < p2.X) return false;
            var y = p2.Z * SlopeY;
            if (p2.Y < -y || y < p2.Y) return false;
            return true;
        }
    }

    public class Sphere : ISet<Vector3>
    {
        float _r;
        float _r_sq;

        public Vector3 Center { get; set; }
        public float Radius
        {
            get => _r;
            set
            {
                _r = value;
                _r_sq = value * value;
            }
        }

        public bool Contains(Vector3 element) => Vector3.DistanceSquared(element, Center) <= _r_sq;
    }
}

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetCoordinates : IEnumerable<Vector3>
{
    readonly Pool _pool;

    Quaternion _rot_rec;

    public Tag Tag { get; set; }
    public Vector3 EyePoint { get; set; }
    public Quaternion EyeRotation
    {
        get => Quaternion.Inverse(_rot_rec);
        set => _rot_rec= Quaternion.Inverse(value);
    }
    public float MaxDistance { get; set; }

    public TargetCoordinates(Tag tag, float maxDistance)
    {
        _pool = new(this);

        Tag = tag;
        MaxDistance = maxDistance;
    }

    public IEnumerator<Vector3> GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected abstract bool Filter(Vector3 p_local);

    public class Enumerator : IEnumerator<Vector3>
    {
        readonly TargetCoordinates _p;

        IEnumerator<TargetComponent> _etor;
        Vector3 _c;

        public Enumerator(TargetCoordinates p)
        {
            _p = p;

            Reset();
        }

        public Vector3 Current => _c;
        object IEnumerator.Current => _c;

        public void Dispose() => _p._pool.Release(this);
        public bool MoveNext()
        {
            retry:;
            if (!_etor.MoveNext()) return false;

            var pos = _etor.Current.transform.position - _p.EyePoint;
            if (!_p.Filter(_p.EyeRotation * pos)) goto retry;

            var ray = new Ray(_p.EyePoint, pos);
            Physics.Raycast(ray, out var info, _p.MaxDistance);
            if (info.collider.transform != _etor.Current.transform) goto retry;

            _c = _etor.Current.transform.position;
            return true;
        }
        public void Reset()
        {
            _etor = TargetComponent.Enableds(with: Referee.GetTargetTag(_p.Tag)).GetEnumerator();
        }
    }

    class Pool : ObjectPool<Enumerator>
    {
        readonly TargetCoordinates _p;
        public Pool(TargetCoordinates p)
        {
            _p = p;
        }

        protected override Enumerator Create() => new(_p);
        protected override void Destroy(Enumerator item) { }
    }
}

public class ConeTargetCoordinates : TargetCoordinates
{
    float _a_tan;

    public float Angle
    {
        get => Mathf.Atan(_a_tan);
        set => _a_tan = Mathf.Tan(Angle);
    }

    public ConeTargetCoordinates(Tag tag, float maxDistance, float angle) : base(tag, maxDistance)
    {
        Angle = angle;
    }

    protected override bool Filter(Vector3 p_local)
    {
        return true;
        if (p_local.z < 0) return false;
        var r = _a_tan * p_local.z;
        return (p_local.x * p_local.x + p_local.y * p_local.y) <= r * r;
    }
}
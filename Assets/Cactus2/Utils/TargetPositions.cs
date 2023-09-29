using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetPositions : IEnumerable<Vector3>
{
    readonly Pool _pool;

    Quaternion _rot_rec;

    public string Tag { get; set; }
    public Vector3 EyePoint { get; set; }
    public Quaternion EyeRotation
    {
        get => Quaternion.Inverse(_rot_rec);
        set => _rot_rec= Quaternion.Inverse(value);
    }

    public TargetPositions(string tag)
    {
        _pool = new(this);

        Tag = tag;
    }

    public IEnumerator<Vector3> GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected abstract bool Filter(Vector3 p_local);

    public class Enumerator : IEnumerator<Vector3>
    {
        readonly TargetPositions _p;

        IEnumerator<GameObject> _etor;
        Vector3 _c;

        public Enumerator(TargetPositions p)
        {
            _p = p;

            Reset();
        }

        public Vector3 Current => _c;
        object IEnumerator.Current => _c;

        public void Dispose() => _p._pool.Release(this);
        public bool MoveNext()
        {
            while (_etor.MoveNext())
            {
                var pos = _etor.Current.transform.position - _p.EyePoint;
                if (!_p.Filter(_p._rot_rec * pos)) continue;

                var ray = new Ray(_p.EyePoint, pos);
                Debug.DrawRay(_p.EyePoint, pos, Color.red, 100, false);
                Physics.Raycast(ray, out var info, pos.magnitude);
                if (info.collider.transform != _etor.Current.transform) continue;

                _c = _etor.Current.transform.position;
                return true;
            }
            return false;
        }
        public void Reset()
        {
            _etor = ((IEnumerable<GameObject>)GameObject.FindGameObjectsWithTag(_p.Tag)).GetEnumerator(); //TargetComponent.Enableds(with: Referee.GetTargetTag(_p.Tag)).GetEnumerator();
        }
    }

    class Pool : ObjectPool<Enumerator>
    {
        readonly TargetPositions _p;
        public Pool(TargetPositions p)
        {
            _p = p;
        }

        protected override Enumerator Create() => new(_p);
        protected override void Destroy(Enumerator item) { }
    }
}

public class OmnidirectionalTargetPositions : TargetPositions
{
    protected override bool Filter(Vector3 p_local) => true;

    public OmnidirectionalTargetPositions(string tag) : base(tag) { }
}

public class ConeTargetPositions : TargetPositions
{
    float _a_tan;

    public float Angle
    {
        get => Mathf.Atan(_a_tan);
        set => _a_tan = Mathf.Tan(Angle);
    }

    public ConeTargetPositions(string tag, float maxDistance, float angle) : base(tag)
    {
        Angle = angle;
    }

    protected override bool Filter(Vector3 p_local)
    {
        if (p_local.z < 0) return false;
        var r = _a_tan * p_local.z;
        return (p_local.x * p_local.x + p_local.y * p_local.y) <= r * r;
    }
}
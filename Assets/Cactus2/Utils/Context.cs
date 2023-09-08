#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class Context<TIndex> : IEquatable<Context<TIndex>?> where TIndex : Context<TIndex>.Index
{
    readonly List<WeakReference<TIndex>> _refs;

    public Context()
    {
        _refs = new();
    }

    public IndexCollection Indexes => new(this);

    public override bool Equals(object? obj) => Equals(obj as Context<TIndex>);
    public bool Equals(Context<TIndex>? other) => other is not null && ReferenceEquals(_refs, other._refs);
    public override int GetHashCode() => HashCode.Combine(_refs);

    public static bool operator ==(Context<TIndex>? left, Context<TIndex>? right) => EqualityComparer<Context<TIndex>?>.Default.Equals(left, right);
    public static bool operator !=(Context<TIndex>? left, Context<TIndex>? right) => !(left == right);

    public class Index : IEquatable<Context<TIndex>.Index?>
    {
        public Context<TIndex> Context { get; }
        public int Value { get; }

        public Index(Context<TIndex> context)
        {
            for (int i = 0; i < context._refs.Count; i++)
            {
                if (!context._refs[i].TryGetTarget(out _)) 
                {
                    var target = this as TIndex;
                    Debug.Assert(target is not null);
                    context._refs[i].SetTarget(target);
                    Value = i;
                    break;
                }
            }
            Context = context;
        }

        public override bool Equals(object? obj) => Equals(obj as Context<TIndex>.Index);
        public bool Equals(Context<TIndex>.Index? other) => other is not null && EqualityComparer<Context<TIndex>>.Default.Equals(Context, other.Context) && Value == other.Value;
        public override int GetHashCode() => HashCode.Combine(Context, Value);

        public static bool operator ==(Context<TIndex>.Index? left, Context<TIndex>.Index? right) => EqualityComparer<Context<TIndex>.Index?>.Default.Equals(left, right);
        public static bool operator !=(Context<TIndex>.Index? left, Context<TIndex>.Index? right) => !(left == right);
    }

    public readonly struct IndexCollection : ICollection<TIndex>
    {
        readonly Context<TIndex> _context;

        public int Count
        {
            get
            {
                int c = 0;
                foreach (var @ref in _context._refs)
                {
                    if (@ref.TryGetTarget(out var i)) c++;
                }
                return c;
            }
        }
        public bool IsReadOnly => true;

        public IndexCollection(Context<TIndex> context)
        {
            _context = context;
        }

        public bool Contains(TIndex item)
        {
            var e = EqualityComparer<TIndex>.Default;
            foreach(var r in _context._refs)
            {
                if (r.TryGetTarget(out var t) && e.Equals(t, item)) return true;
            }
            return false;
        }
        public void CopyTo(TIndex[] array, int arrayIndex) => throw new NotImplementedException();
        public Enumerator GetEnumerator() => new(this);

        void ICollection<TIndex>.Add(TIndex item) => throw new NotSupportedException();
        void ICollection<TIndex>.Clear() => throw new NotSupportedException();
        bool ICollection<TIndex>.Remove(TIndex item) => throw new NotSupportedException();
        IEnumerator<TIndex> IEnumerable<TIndex>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TIndex>
        {
            readonly IndexCollection _indexes;
            int _i;
            TIndex _c;

            public Enumerator(IndexCollection indexes)
            {
                _indexes = indexes;
                _i = -1;
                _c = null!;
            }

            public readonly TIndex Current => _c;
            readonly object IEnumerator.Current => _c;

            public readonly void Dispose() { }
            public bool MoveNext()
            {
                var refs = _indexes._context._refs;
                do if (refs.Count <= ++_i) return false;
                while (!refs[_i].TryGetTarget(out _c));
                return true;
            }
            public void Reset()
            {
                _i = -1;
            }
        }
    }
}

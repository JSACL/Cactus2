#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;

/// <summary>
/// 軄は各実体の役割を一意に表します。
/// </summary>
public class Tag : IEquatable<Tag>
{
    readonly string _name;
    readonly int _hash;
    readonly Dictionary<Relationship> _relationships;
    int _index;

    /// <summary>
    /// 軄を名に拠って作します。
    /// </summary>
    /// <param name="name"></param>
    public Tag(string name)
    {
        _name = name;
        _hash = name.GetHashCode();
        _index = _instances.Count;
        _relationships = new();
        Involve(this);
    }

    public Relationship GetRelationship(Tag to)
    {
        return _relationships[to];
    }

    public override bool Equals(object? obj) => obj is Tag info && Equals(info);
    public bool Equals(Tag other) => ReferenceEquals(this, other) || _hash == other._hash && _name == other._name;
    public override int GetHashCode() => _hash;

    static readonly List<WeakReference<Tag>> _instances = new();

    public static Tag GetOrCreate(string name)
    {
        foreach (var instance in _instances) if (instance.TryGetTarget(out var tag) && tag._name == name) return tag;
        return new(name);
    }

    static void Involve(Tag info)
    {
        _instances.Add(new(info));
    }

    static void Clean()
    {
        throw new NotImplementedException();
    //    _instances = _instances.Successed((WeakReference<Tag> x, out Tag r) => x.TryGetTarget(out r)).Select(x => new WeakReference<Tag>(x)).ToList();
    }

    public static Tag Unknown { get; } = new("Unknown");
    public static Tag NaturalStructure => new("Natural");

    public static bool operator ==(Tag left, Tag right) => left.Equals(right);
    public static bool operator !=(Tag left, Tag right) => !(left == right);

    public class Dictionary<T>
    {
        T?[] _arr;

        public Dictionary()
        {
            _arr = Array.Empty<T>();
        }

        public T? this[Tag key]
        {
            get
            {
                if (_arr.Length <= key._index) Extend();
                return _arr[key._index];
            }
            set
            {
                if (_arr.Length <= key._index) Extend();
                _arr[key._index] = value;
            }
        }
        public IEnumerable<Tag> Keys => _instances.Successed((WeakReference<Tag> x, out Tag r) => x.TryGetTarget(out r));
        public ICollection<T?> Values => _arr;
        public int Count => _instances.Count;
        public bool IsReadOnly => false;
        public void Clear() => _arr = Array.Empty<T>();
        public bool Contains(KeyValuePair<Tag, T> item) => _arr.Length > item.Key._index && EqualityComparer<T?>.Default.Equals(_arr[item.Key._index], item.Value);
        public bool ContainsKey(Tag key) => true;
        public IEnumerator<KeyValuePair<Tag, T?>> GetEnumerator()
        {
            foreach (var i in _instances.Successed((WeakReference<Tag> x, out Tag r) => x.TryGetTarget(out r)))
            {
                if (_arr.Length > i._index) yield return new(i, _arr[i._index]);
                else yield return new(i, default);
            }
        }
        //public bool TryGetValue(Tag key, out T? value)
        //{
        //    if (_arr.Length > key._index)
        //    {
        //        value = _arr[key._index];
        //        return true;
        //    }
        //    else
        //    {
        //        value = default;
        //        return false;
        //    }
        //}

        void Extend()
        {
            Array.Resize(ref _arr, _arr.Length == 0 ? 4 : _arr.Length * 2);
        }
    }
}

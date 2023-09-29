using System.Collections;
using System.Collections.Generic;

public readonly struct ConfinedArray<T> : ICollection<T>
{
    readonly T[] _arr;
    readonly int _count;

    public ConfinedArray(T[] array, int count)
    {
        _arr = array;
        _count = count;
    }

    public int Count => _count;
    public bool IsReadOnly => true;
    void ICollection<T>.Add(T item) => ((ICollection<T>)_arr).Add(item);
    void ICollection<T>.Clear() => ((ICollection<T>)_arr).Clear();
    public bool Contains(T item) => ((ICollection<T>)_arr).Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)_arr).CopyTo(array, arrayIndex);
    public ConfinedArrayEnumerator<T> GetEnumerator() => new ConfinedArrayEnumerator<T>(_arr, _count);
    bool ICollection<T>.Remove(T item) => ((ICollection<T>)_arr).Remove(item);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}

public struct ConfinedArrayEnumerator<T> : IEnumerator<T>
{
    readonly T[] _arr;
    readonly int _count;
    int _i;

    public T Current => _arr[_i];
    object IEnumerator.Current => Current;

    public ConfinedArrayEnumerator(T[] array, int count)
    {
        _arr = array;
        _count = count;
        _i = -1;
    }

    public void Dispose() { }
    public bool MoveNext() => ++_i < _count;
    public void Reset()
    {
        _i = -1;
    }
}
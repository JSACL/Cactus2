using System.Collections.Concurrent;
using System;

public abstract class ObjectPool<T>
{
    readonly ConcurrentBag<T> _objects;

    public ObjectPool()
    {
        _objects = new ConcurrentBag<T>();
    }

    public T Get() => _objects.TryTake(out T item) ? item : Create();

    public void Release(T item) => _objects.Add(item);

    protected abstract T Create();

    protected abstract void Destroy(T item);
}
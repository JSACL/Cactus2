using System.Collections.Concurrent;
using System;
using UnityEngine;
using UE = UnityEngine;


public abstract class ObjectPool<T> : IObjectSource<T>
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

public class GameObjectPool : ObjectPool<GameObject>
{
    public GameObject Prefab { get; }

    public GameObjectPool(GameObject prefab)
    {
        Prefab = prefab;
    }

    protected override GameObject Create() => UE::Object.Instantiate(Prefab);
    protected override void Destroy(GameObject item) => UE::Object.Destroy(item);
}

using System;
using UnityEngine;
using UE = UnityEngine;

public interface IObjectSource<T>
{
    public T Get();
    public void Release(T obj);
}

public class ObjectSource<T> : IObjectSource<T>
{
    public Func<T> Constructor { get; }

    public ObjectSource(Func<T> constructor)
    {
        Constructor = constructor;
    }

    public T Get() => Constructor();

    public void Release(T obj) { }
}

public class GameObjectSource : IObjectSource<GameObject>
{
    public GameObject Prefab { get; }

    public GameObjectSource(GameObject prefab)
    {
        Prefab = prefab;
    }

    public GameObject Get() => UE::Object.Instantiate(Prefab);
    public void Release(GameObject obj) => UE::Object.Destroy(obj);
}
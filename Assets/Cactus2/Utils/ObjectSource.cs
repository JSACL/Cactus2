#nullable enable
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UE = UnityEngine;

public interface IObjectSource<T>
{
    int Count { get; }
    T Get();
    ValueTask<T> GetAsync() => new(Get());
    void Release(T obj);
    Task ReleaseAsync(T obj) { Release(obj); return Task.CompletedTask; }
}

public class ObjectSource<T> : IObjectSource<T>
{
    public int Count { get; private set; }
    public Func<T> Constructor { get; }

    public ObjectSource(Func<T> constructor)
    {
        Constructor = constructor;
    }

    public T Get()
    {
        Count++;
        return Constructor();
    }

    public void Release(T obj) 
    {
        Count--;
    }
}

public class GameObjectSource : IObjectSource<GameObject>
{
    GameObject? _obj;
    AsyncOperationHandle<GameObject> _handle;

    public int Count { get; private set; }
    public string Address { get; internal set; }
    public Transform? Parent { get; internal set; }
    public Scene? Scene { get; internal set; }

    public GameObjectSource(string address, Scene scene)
    {
        Address = address;
        Scene = scene;
    }
    public GameObjectSource(string address, Transform parent)
    {
        Address = address;
        Parent = parent;
    }

    public GameObject Get() => GetAsync().Result;
    public async ValueTask<GameObject> GetAsync()
    {
        Count++;

        if (_obj == null)
        {
            _handle = Addressables.LoadAssetAsync<GameObject>(Address);
            _obj = await _handle.Task;
        }

        var obj = UE::Object.Instantiate(_obj, Parent);
        if (Scene is { } scene) SceneManager.MoveGameObjectToScene(obj, scene);
        return obj;
    }

    public void Release(GameObject obj)
    {
        Count--;

        UE::Object.Destroy(obj);

        if (Count == 0)
        {
            Addressables.Release(_handle);
        }
    }
}
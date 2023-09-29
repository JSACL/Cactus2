using System.Collections.Concurrent;
using System;
using UnityEngine;
using UE = UnityEngine;
using UnityEngine.SceneManagement;
using static Utils;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class ObjectPool<T> : IObjectSource<T>
{
    readonly ConcurrentBag<T> _objects;

    public int Count { get; private set; }

    public ObjectPool()
    {
        _objects = new ConcurrentBag<T>();
    }

    public T Get()
    {
        Count++;
        return _objects.TryTake(out T item) ? item : Create();
    }
    public ValueTask<T> GetAsync()
    {
        Count++;
        return _objects.TryTake(out T item) ? new(item) : CreateAsync();
    }

    public void Release(T item)
    {
        Count--;
        _objects.Add(item);
    }
    public Task ReleaseAsync(T item)
    {
        Count--;
        _objects.Add(item);
        return Task.CompletedTask;
    }

    protected abstract T Create();
    protected virtual ValueTask<T> CreateAsync() => new(Create());

    protected abstract void Destroy(T item);
    protected virtual Task DestroyAsync(T item) { Destroy(item); return Task.CompletedTask; }
}

public class GameObjectPool : ObjectPool<GameObject>
{
    public GameObject Prefab { get; }
    public Transform Parent { get; }
    public UE.SceneManagement.Scene? Scene { get; }

    public GameObjectPool(GameObject prefab, Transform parent)
    {
        Prefab = prefab;
        Parent = parent;
    }
    public GameObjectPool(GameObject prefab, UE.SceneManagement.Scene scene)
    {
        Prefab = prefab;
        Scene = scene;
    }

    protected override GameObject Create()
    {
        var obj = UE::Object.Instantiate(Prefab, Parent);
        if (Scene is { } scene)SceneManager.MoveGameObjectToScene(obj, scene);
        return obj;
    }
    protected override void Destroy(GameObject item) => UE::Object.Destroy(item);
}

public class LazyGameObjectPool : ObjectPool<GameObject>
{
    readonly Transform _parent;
    readonly UE.SceneManagement.Scene? _scene;
    readonly AssetReference _reference;
    GameObject _prefab;
    int _count;
    AsyncOperationHandle<GameObject> _handle;

    public LazyGameObjectPool(AssetReference reference, Transform parent)
    {
        _reference = reference;
        _parent = parent;
    }
    public LazyGameObjectPool(AssetReference reference, UE.SceneManagement.Scene scene)
    {
        _reference = reference;
        _scene = scene;
    }

    protected override GameObject Create() => CreateAsync().Result;
    protected override async ValueTask<GameObject> CreateAsync()
    {
        Assert(_count >= 0);
        _count++;

        if (_prefab == null)
        {
            _handle = _reference.LoadAssetAsync<GameObject>();
            _prefab = await _handle.Task;
        }

        var obj = UE::Object.Instantiate(_prefab, _parent);
        if (_scene is { } scene) SceneManager.MoveGameObjectToScene(obj, scene);
        return obj;
    }
    protected override void Destroy(GameObject item)
    {
        _count--;
        Assert(_count >= 0);

        if (_count == 0)
        {
            Addressables.ReleaseInstance(_handle);
        }

        UE::Object.Destroy(item);
    }
}
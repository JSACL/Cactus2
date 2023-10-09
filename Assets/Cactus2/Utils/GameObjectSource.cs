#nullable enable
using System.Threading.Tasks;
using Nonno.Assets;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine;
using UE = UnityEngine;

public class GameObjectSource<TComponent> : IObjectSource<TComponent> where TComponent : Component
{
    GameObject? _obj;
    AsyncOperationHandle<GameObject> _handle;

    public int Count { get; private set; }
    public string Address { get; internal set; }
    public UE::Transform? Parent { get; internal set; }
    public UE.SceneManagement.Scene? Scene { get; internal set; }

    public GameObjectSource(string address, UE.SceneManagement.Scene scene)
    {
        Address = address;
        Scene = scene;
    }
    public GameObjectSource(string address, UE::Transform parent)
    {
        Address = address;
        Parent = parent;
    }

    public TComponent Get() => GetAsync().Result;
    public async ValueTask<TComponent> GetAsync()
    {
        Count++;

        if (_obj == null)
        {
            _handle = Addressables.LoadAssetAsync<GameObject>(Address);
            _obj = await _handle.Task;
        }

        var obj = UE::Object.Instantiate(_obj, Parent);
        if (Scene is { } scene) SceneManager.MoveGameObjectToScene(obj, scene);
        return obj.GetComponent<TComponent>();
    }

    public void Release(TComponent obj)
    {
        Count--;

        UE::Object.Destroy(obj.gameObject);

        if (Count == 0)
        {
            Addressables.Release(_handle);
        }
    }
}

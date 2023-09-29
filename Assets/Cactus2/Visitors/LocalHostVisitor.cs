#nullable enable
using UnityEngine;
using System;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using DB = System.Diagnostics.DebuggerBrowsableAttribute;
using SF = UnityEngine.SerializeField;
using DBS = System.Diagnostics.DebuggerBrowsableState;
using GOS = IObjectSource<UnityEngine.GameObject>;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LocalHostVisitor : IVisitor
{
    readonly ArrayViewModels<IPlayer> _players = new();
    readonly SingleViewModels<IEntity> _firers = new();
    readonly SingleViewModels<IBullet> _bullets = new();
    readonly SingleViewModels<ILaser> _lasers = new();
    readonly SingleViewModels<IEntity> _entities = new();
    readonly SingleViewModels<ISpecies1> _species1s = new();

    public Scene Scene { get; }

    public LocalHostVisitor(Scene scene)
    {
        Scene = scene;

        _players.GameObjectSources = new GOS[] { new GameObjectSource(P("Player"), Scene), new GameObjectSource(P("CombatUI"), Scene) };
        _firers.GameObjectSource = new GameObjectSource(P("Firer"), Scene);
        _bullets.GameObjectSource = new GameObjectSource(P("Bullet"), Scene);
        _lasers.GameObjectSource = new GameObjectSource(P("Laser"), Scene);
        _entities.GameObjectSource = new GameObjectSource(P("Entity"), Scene);
        _species1s.GameObjectSource = new GameObjectSource(P("Species1"), Scene);

        static string P(string name) => $"Assets/Cactus2/Views/{name}.prefab";
    }

    public void Add(IPlayer model) => _players.Add(model);
    public void Remove(IPlayer model) => _players.Remove(model);

    public void Add(IEntity model) => _entities.Add(model);
    public void Remove(IEntity model) => _entities.Remove(model);

    public void Add(IBullet model) => _bullets.Add(model);
    public void Remove(IBullet model) => _bullets.Remove(model);

    public void Add(ILaser model) => _lasers.Add(model);
    public void Remove(ILaser model) => _lasers.Remove(model);

    public void Add(IFirer model) => _firers.Add(model);
    public void Remove(IFirer model) => _firers.Remove(model);

    public void Add(ISpecies1 model) => _species1s.Add(model);
    public void Remove(ISpecies1 model) => _species1s.Remove(model);
}

public class SingleViewModels<TModel> : ICollection<TModel> where TModel : class
{
    GOS? _objectSource;
    readonly Dictionary<TModel, ViewModel<TModel>> _vMs;

    public GOS? GameObjectSource
    {
        get => _objectSource;
        set => _objectSource = value;
    }

    public SingleViewModels()
    {
        _vMs = new();
    }

    public int Count => _vMs.Count;
    public bool IsReadOnly => false;
    public async void Add(TModel item)
    {
        if (!_vMs.TryGetValue(item, out var vM))
        {
            if (_objectSource is null) throw new InvalidOperationException();
            vM = (await _objectSource.GetAsync()).GetComponent<ViewModel<TModel>>();
            _vMs.Add(item, vM);
        }
        vM.Model = item;
    }
    public void Clear() => throw new NotImplementedException();
    public bool Contains(TModel item) => throw new NotImplementedException();
    public void CopyTo(TModel[] array, int arrayIndex) => throw new NotImplementedException();
    public IEnumerator<TModel> GetEnumerator() => _vMs.Keys.GetEnumerator();
    public async void Remove(TModel item)
    {
        if (_vMs.TryGetValue(item, out var vM))
        {
            if (_objectSource is null) throw new InvalidOperationException();
            await _objectSource.ReleaseAsync(vM.gameObject);
            vM.Model = null;
            _ = _vMs.Remove(item);
        }
    }
    void ICollection<TModel>.Add(TModel item) => Add(item);
    bool ICollection<TModel>.Remove(TModel item)
    {
        if (_vMs.ContainsKey(item))
        {
            Remove(item);
            return true;
        }
        else return false;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ArrayViewModels<TModel> : ICollection<TModel> where TModel : class
{
    GOS[] _objectSources;
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 各値には、その所属する対象の異同に関わらず、ViewModelが並びますが、同一のGameObjectに所属するViewModelは隣接することが保証されます。
    /// </remarks>
    readonly Dictionary<TModel, ViewModel<TModel>[]> _vMs;

    public GOS[] GameObjectSources
    {
        get => _objectSources;
        set => _objectSources = value;
    }

    public ArrayViewModels()
    {
        _objectSources = Array.Empty<GOS>();
        _vMs = new();
    }

    public int Count => _vMs.Count;
    public bool IsReadOnly => false;
    public async void Add(TModel item)
    {
        if (!_vMs.TryGetValue(item, out var vMs))
        {
            vMs = new ViewModel<TModel>[_objectSources.Length];
            for (int i = 0; i < vMs.Length; i++)
            {
                vMs[i] = (await _objectSources[i].GetAsync()).GetComponent<ViewModel<TModel>>();
            }
            _vMs.Add(item, vMs);
        }
        foreach (var vM in vMs) vM.Model = item;
    }
    public void Clear() => throw new NotImplementedException();
    public bool Contains(TModel item) => throw new NotImplementedException();
    public void CopyTo(TModel[] array, int arrayIndex) => throw new NotImplementedException();
    public IEnumerator<TModel> GetEnumerator() => _vMs.Keys.GetEnumerator();
    public async void Remove(TModel item)
    {
        if (_vMs.TryGetValue(item, out var vMs))
        {
            //foreach (var vM in vMs) vM.Model = null;
            for (int i = 0; i < vMs.Length; i++)
            {
                vMs[i].Model = null;
                await _objectSources[i].ReleaseAsync(vMs[i].gameObject);
            }
            _ = _vMs.Remove(item);
        }
    }
    bool ICollection<TModel>.Remove(TModel item)
    {
        if (_vMs.ContainsKey(item))
        {
            Remove(item);
            return true;
        }
        return false;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
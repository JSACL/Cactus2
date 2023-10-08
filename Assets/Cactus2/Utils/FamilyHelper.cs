using System.Collections;
using Nonno.Assets;
using Nonno.Assets.Presentation;
using UnityEngine;
using System;

public class FamilyHelper
{
    readonly IndexedDynamicDispatcher _adD = new();
    readonly IndexedDynamicDispatcher _rmD = new();
    readonly Hashtable _vMs = new();
    UnityEngine.SceneManagement.Scene _scene;

    public UnityEngine.SceneManagement.Scene Scene
    {
        get => _scene;
        set => _scene = value;
    }
    public ICollection ViewModels => _vMs.Values;
    public ICollection Models => _vMs.Keys; 

    public FamilyHelper()
    {
        
    }

    public void S<TModel, TPresenter, TViewModel>(string address) where TModel : class where TPresenter : class, IPresenter<TModel>, new() where TViewModel : Component, IViewModel<TPresenter>
    {
        if (!_scene.IsValid()) throw new Exception("シーンが無効です。");
        var source = new GameObjectSource<TViewModel>(address, _scene);
        _adD.Overload<TModel>(x => source.Get().Model = new TPresenter() { Model = x });
        _rmD.Overload<TModel>(x =>
        {
            var vm = (TViewModel)_vMs[x];
            source.Release(vm);
            ((TPresenter)((IViewModel)vm).Model).Model = null;
        });
    }
    public void P<TModel, TPresenter, TViewModel>(string address) where TModel : class where TPresenter : class, IPresenter<TModel>, new() where TViewModel : Component, IViewModel<TPresenter>
    {
        if (!_scene.IsValid()) throw new Exception("シーンが無効です。");
        var source = new ObjectPool<TViewModel>(new GameObjectSource<TViewModel>(address, _scene));
        _adD.Overload<TModel>(x => source.Get().Model = new TPresenter() { Model = x });
        _rmD.Overload<TModel>(x =>
        {
            var vm = (TViewModel)_vMs[x];
            source.Release(vm);
            ((TPresenter)((IViewModel)vm).Model).Model = null;
        });
    }

    public void HandleFamilyChange(object? sender, FamilyChangeEventArgs e)
    {
        switch (e.Action)
        {
        case SceneChangeAction.Add:
            _adD.Dispatch(e.Object);
            break;
        case SceneChangeAction.Remove:
            _rmD.Dispatch(e.Object);
            break;
        default:
            break;
        }
    }
}
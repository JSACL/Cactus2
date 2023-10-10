using System.Collections;
using Nonno.Assets;
using Nonno.Assets.Presentation;
using UnityEngine;
using System;
using Nonno.Assets.Collections;
using Cactus2;
using System.Runtime.CompilerServices;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void S<TModel, TPresenter, TViewModel>(string address) where TModel : class where TPresenter : class, IPresenter<TModel>, new() where TViewModel : Component, IViewModel<TPresenter>
    {
        if (!_scene.IsValid()) throw new Exception("シーンが無効です。");
        var source = new GameObjectSource<TViewModel>(address, _scene);
        _adD.Overload<TModel>(async x =>
        {
            var vm = await source.GetAsync();
            var p = new TPresenter();
            //_vMs.Add(x, vm);
            vm.Model = p;
            p.Model = x;
        });
        _rmD.Overload<TModel>(x =>
        {
            var vm = (TViewModel)_vMs[x];
            //_vMs.Remove(x);
            if (vm is null) return;
            source.Release(vm);
            ((TPresenter)((IViewModel)vm).Model).Model = null;
        });
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void S<TModel, TViewModel>(string address) where TModel : class where TViewModel : Component, IViewModel<TModel>
    {
        if (!_scene.IsValid()) throw new Exception("シーンが無効です。");
        var source = new GameObjectSource<TViewModel>(address, _scene);
        _adD.Overload<TModel>(async x =>
        {
            var vm = await source.GetAsync();
            //_vMs.Add(x, vm);
            vm.Model = x;
        });
        _rmD.Overload<TModel>(x =>
        {
            var vm = (TViewModel)_vMs[x];
            //_vMs.Remove(x);
            if (vm is null) return;
            source.Release(vm);
            vm.Model = null;
        });
    }
    public void P<TModel, TPresenter, TViewModel>(string address) where TModel : class where TPresenter : class, IPresenter<TModel>, new() where TViewModel : Component, IViewModel<TPresenter>
    {
        if (!_scene.IsValid()) throw new Exception("シーンが無効です。");
        var source = new ObjectPool<TViewModel>(new GameObjectSource<TViewModel>(address, _scene));
        _adD.Overload<TModel>(async x => 
        {
            var vm = await source.GetAsync();
            _vMs.Add(x, vm);
            vm.Model = new TPresenter() { Model = x }; 
        });
        _rmD.Overload<TModel>(async x =>
        {
            var vm = (TViewModel)_vMs[x];
            _vMs.Remove(x);
            await source.ReleaseAsync(vm);
            ((TPresenter)((IViewModel)vm).Model).Model = null;
        });
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 最後に呼んで下さい。
    /// </remarks>
    public void IgnoreUnknownModel()
    {
        _adD.Overload<object>(o => { });
        _rmD.Overload<object>(o => { });
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
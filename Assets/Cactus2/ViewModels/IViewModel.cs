#nullable enable
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using CA = System.Diagnostics.CodeAnalysis;
using UE = UnityEngine;

public interface IViewModel<TModel>
{
    TModel? Model { get; set; }
}

public class ViewModels<TModel> where TModel : class
{
    readonly List<IViewModel<TModel>> _viewModels = new();

    public GameObject Prefab { get; }
    public Transform? Parent { get; }

    public ViewModels(GameObject prefab, Transform? parent = null)
    {
        Prefab = prefab;
        Parent = parent;
    }

    public void Add(TModel model)
    {
        for (int i = 0; i < _viewModels.Count; i++)
        {
            if (_viewModels[i].Model is null)
            {
                _viewModels[i].Model = model;
                return;
            }
        }

        var vm = Create();
        vm.Model = model;
        _viewModels.Add(vm);
    }

    public void Remove(TModel model)
    {
        for (int i = 0; i < _viewModels.Count; i++)
        {
            if (Equals(_viewModels[i].Model, model))
            {
                _viewModels[i].Model = null;
            }
        }
    }

    protected virtual IViewModel<TModel> Create()
    {
        var obj = UE::Object.Instantiate(Prefab, Parent);
        return (IViewModel<TModel>)obj.GetComponent(typeof(IViewModel<TModel>));
    }

    protected virtual void Destroy(IViewModel<TModel> viewModel)
    {
        UE::Object.Destroy((UE::Object)viewModel);
    }
}
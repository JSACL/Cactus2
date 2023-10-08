#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using Nonno.Assets;
using UnityEngine;

public class ViewModel<TModel> : MonoBehaviour, IViewModel<TModel> where TModel : class
{
    TModel? _model = null!;
    //readonly IObjectSource<TModel> _source;

    [property: NotNull]
    public TModel? Model
    {
        [return: NotNull]
        get => _model!;
        set
        {
            if (_model is { })
            {
                Disconnect();
                _model = null;
            }
            if (value is { })
            {
                _model = value;
                Connect();
            }
            gameObject.SetActive(_model is { });
        }
    }

    object IViewModel.Model => Model;

    //public ViewModel()
    //{
    //    _source = IObjectSource<TModel>.Default ?? throw new Exception("依存性の注入に失敗しました。");
    //    _model = _source.Get();
    //}

    //protected void OnDestroy()
    //{
    //    _source.Release(Model);
    //}

    protected void Awake()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Connect() { }
    protected virtual void Disconnect() { }
}

public interface IViewModel
{
    object Model { get; }
}

public interface IViewModel<in TModel> : IViewModel
{
    new TModel? Model { set; }
}

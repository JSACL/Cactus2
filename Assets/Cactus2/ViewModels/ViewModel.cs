#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class ViewModel : MonoBehaviour
{
    protected object? _model;

    [Obsolete()]
    public object? Model
    {
        [return: NotNull]
        get => _model;
        set
        {
            if (_model is { })
            {
                Disconnect();
                SetModel(null);
            }
            if (value is { })
            {
                SetModel(value);
                Connect();
            }
            gameObject.SetActive(_model is { });
        }
    }

    //protected virtual void Awake()
    //{
    //    gameObject.SetActive(false);
    //}

    protected virtual void SetModel(object? obj) => _model = obj;
    protected virtual void Connect() { }
    protected virtual void Disconnect() { }
}

public class ViewModel<TModel> : ViewModel where TModel : class
{
    new TModel _model = null!;

    public new TModel? Model
    {
        [return: NotNull]
        get => _model;
        set
        {
            if (_model is { })
            {
                Disconnect();
                SetModel(null);
            }
            if (value is { })
            {
                SetModel(value);
                Connect();
            }
            gameObject.SetActive(_model is { });
        }
    }

    protected sealed override void SetModel(object? obj)
    {
        _model = (TModel?)obj!;
        base._model = obj;
    }
}
#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class ViewModel<TModel> : MonoBehaviour where TModel : class
{
    TModel? _model = null!;

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

    protected virtual void Connect() { }
    protected virtual void Disconnect() { }
}
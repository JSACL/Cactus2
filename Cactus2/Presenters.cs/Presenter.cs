#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace Cactus2.Presenter;
public class Presenter<TModel> : IPresenter<TModel>, IVariablePresenter
{
    TModel? _model;

    public event Action? PropertyChanged;

    [property: NotNull]
    public TModel? Model
    {
        get => _model!;
        set
        {
            if (_model is not null) Disable();
            _model = value;
            if (_model is not null) Enable();
        }
    }

    public Presenter()
    {
    }

    protected virtual void Enable()
    {
    }

    protected virtual void Disable()
    {
    }

    protected void OnPropertyChanged()
    {
        PropertyChanged?.Invoke();
    }
}
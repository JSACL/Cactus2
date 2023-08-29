#nullable enable
using static Utils;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour, IViewModel<IFirer>
{
    IFirer? _model;

    public IFirer? Model
    {
        get => _model;
        set
        {
            if (_model is { })
            {
                _model = null;
            }
            if (value is { })
            {
                _model = value;
            }
            enabled = _model is not null;
        }
    }

    void Update()
    {
        Assert(Model is not null);

        Model.AddTime(Time.deltaTime);
    }
}
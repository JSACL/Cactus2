#nullable enable
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.MathF;
using static ConstantValues;
using static UnityEngine.Input;
using static Utils;
using vec = UnityEngine.Vector3;

public sealed class Species1Behaviour : MonoBehaviour
{
    ISpecies1? _model;

    public ISpecies1? Model
    {
        get => _model;
        set
        {
            if (_model is not null)
            {
                _model = null;
            }
            if (value is not null)
            {
                _model = value;
            }
        }
    }
}
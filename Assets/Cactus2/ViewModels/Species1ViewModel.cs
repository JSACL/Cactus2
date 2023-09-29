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
using qtn = UnityEngine.Quaternion;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;

public class Species1ViewModel : RigidbodyViewModel<ISpecies1>
{
    TargetPositions _tC;

    [Header("Internal")]
    [SerializeField]
    int _groundCount_onFoot;
    [Header("External")]
    [SerializeField]
    ColliderComponent _bodyCES = null!;
    [SerializeField]
    TriggerComponent _footTES = null!;
    [SerializeField]
    TargetComponent _targetComponent = null!;
    [SerializeField]
    Transform _eyeT = null!;

    void Start()
    {
        _bodyCES.Stay += (_, e) => { Model?.Impulse(Model.Position, e.Impulse); };
        _targetComponent.Executed += (_, e) =>
        {
            e.Command.Execute(Model);
        };

        Model.TargetPositions = _tC = new OmnidirectionalTargetPositions(Model.Authority.Name);//, 100, 80);
    }

    protected new void Update()
    {
        base.Update();

        _tC.EyePoint = _eyeT.position;
        _tC.EyeRotation = _eyeT.rotation;
    }
}
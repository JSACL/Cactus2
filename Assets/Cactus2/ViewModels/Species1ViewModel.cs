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
using UE = UnityEngine;

public class Species1ViewModel : RigidbodyViewModel<IRigidbodyPresenter>
{
    public ColliderComponent _bodyCES = null!;
    public TargetComponent targetComponent;
}
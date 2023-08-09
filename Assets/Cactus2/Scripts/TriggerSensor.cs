#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class TriggerSensor : MonoBehaviour
{
    [SerializeField]
    [Obsolete]
    string _name;
    int _count;

    public bool IsOn => _count > 0;
    public int Count => _count;
    [Obsolete]
    public string Name => _name;

    public event ColliderEventHandler? StateChanged;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{_count}++");
        _count++;
        StateChanged?.Invoke(this, new(ColliderFlug.Enter, other, _count));
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"--{_count}");
        _count--;
        Assert(_count is not < 0);
        StateChanged?.Invoke(this, new(ColliderFlug.Exit, other, _count));
    }
}

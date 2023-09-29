#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : SCComponent, IAuthorized
{
    public event CommandEventHandler? Executed;

    public void Execute(ICommand command) => Executed?.Invoke(this, new(command));
}
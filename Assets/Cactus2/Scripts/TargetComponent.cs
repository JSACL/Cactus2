#nullable enable
using System;
using System.Collections.Generic;
using Nonno.Assets;
using Nonno.Assets.Collections;
using UnityEngine;

public class TargetComponent : SCComponent
{
    public ICommand? Command { get; set; }
    public void Affect(Typed info)
    {
        if (Command is { } && Command.CanExecute)
            Command.Execute(info);
    }
}
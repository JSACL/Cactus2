#nullable enable

using System;
using System.Threading.Tasks;
using static Utils;
using static System.Math;
using Nonno.Assets.Presentation;

public class Animal : Entity, IAnimal
{
    public event EventHandler? TransitAnimation;

    public Animal(IScene scene) : base(scene)
    {
        
    }

    protected void OnTransitAnimation() => TransitAnimation?.Invoke(this, EventArgs.Empty);
}

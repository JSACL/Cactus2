#nullable enable

namespace Cactus2;

public class Animal : Entity, IAnimal
{
    public event EventHandler? TransitAnimation;

    public override IScene Scene
    {
        get => _scene;
        set
        {
            _scene.Remove(this);
            _scene = value;
            _scene.Add(this);
        }
    }

    public Animal(IScene scene) : base(scene)
    {
        
    }

    protected void OnTransitAnimation() => TransitAnimation?.Invoke(this, EventArgs.Empty);
}

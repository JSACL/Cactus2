namespace Cactus2;
public class Ground : Entity, IGround
{
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

    public Ground(IScene scene) : base(scene)
    {

    }
}

#nullable enable
using System.Numerics;

namespace Cactus2;
public class FugaEnemy : Animal, IAnimal, IViewer
{
    public const float FORCE_REDUCTION_RATE_PER_SEC = 0.001f;
    LaserGun? _gun;
    bool _searching;
    Appearance? _target;
    readonly Sphere _sphere;

    public LaserGun? Gun { get => _gun; set => _gun = value; }
    public ISet<Vector3> View => _searching ? UniversalSet<Vector3>.Shared : _sphere;
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

    public FugaEnemy(IScene scene) : base(scene)
    {
        _sphere = new Sphere() { Radius = 2.0f };
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (_gun is { })
        {
            _gun.Transform = Transform;
            if (_gun.IsReadyToFire)
            {
                if (_target is { } target)
                {
                    _searching = false;
                    _gun.TargetPosition = target.Transform.Position;
                    _gun.Trigger();
                }
                else
                {
                    _searching = true;
                }
            }
        }
    }

    public void Recognize(Appearance t)
    {
        _target = t;
    }
}
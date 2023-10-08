#nullable enable
using System.Numerics;
using Nonno.Assets;

public class FugaFirer : Weapon
{
    public override string Name => "Fuga Firer";

    public FugaFirer(IScene scene) : base(scene)
    {

    }

    protected override void Fire(Authority tag)
    {
        Scene.Add(new Bullet(Scene)
        {
            Authority = tag,
            Transform = Transform,
            Velocity = new(Vector3.Transform(20 * Vector3.UnitZ, Transform.Rotation), Vector3.Zero),
        });
    }
}

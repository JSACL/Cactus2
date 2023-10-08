#nullable enable
using System;
using System.Numerics;
using Assets.Cactus2;

public class HomingBullet : Bullet, IViewer
{
    readonly Sphere _view;

    public float FuelAmount { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public float AdjustmentPromptness_linear { get; set; } = 0.5f;
    public float AdjustmentPromptness_angular { get; set; } = 0.5f;

    public HomingBullet(IScene scene, float fuelAmount) : base(scene)
    {
        _view = new() { Radius = 2.0f };

        FuelAmount = fuelAmount;
    }

    public ISet<Vector3> View => _view;

    protected override void Update(float deltaTime)
    {
        if (FuelAmount > 0)
        {
            var dif_lin = TargetPosition - Transform.Position;
            var dir_to = Vector3.Normalize(dif_lin);
            var dir_for = Vector3.Normalize(Velocity.Linear);
            var dif_ang = dir_to - dir_for;
            var angV_old = Velocity.Angular;
            var angV_neo = deltaTime * AdjustmentPromptness_angular * dif_ang;
            FuelAmount -= Vector3.Distance(angV_neo, angV_old);

            var linV_old = Velocity.Linear;
            var linV_neo = linV_old += deltaTime * AdjustmentPromptness_linear * dif_lin;
            FuelAmount -= Vector3.Distance(linV_neo, linV_old);

            Velocity = new(linV_neo, angV_neo);
        }

        base.Update(deltaTime);
    }

    public void Recognize(Appearance t)
    {
        _view.Center = t.Transform.Position;

        TargetPosition = t.Transform.Position;
    }
}
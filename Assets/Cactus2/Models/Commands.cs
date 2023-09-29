public class HitCommand : ICommand<IEphemeral>
{
    public bool IsValid { get; set; }
    public Authority Authority { get; set; }
    public float DamageForVitality { get; set; }
    public float DamageForResilience { get; set; }

    public HitCommand()
    {
        Authority = Authority.Unknown;
    }

    public void Execute(IEphemeral ephemeral)
    {
        IsValid = false;

        ephemeral.InflictOnVitality(DamageForVitality);
        ephemeral.InflictOnResilience(DamageForResilience);
    }
}

public class StabilizationCommand : ICommand<IHumanoid>
{
    public bool IsValid => true;

    public Authority Authority { get; set; } = Authority.Unknown;

    public void Execute(IHumanoid humanoid)
    {
        humanoid.FootIsOn = true;
    }
}

public class DestabilizationCommand : ICommand<IHumanoid>
{
    public bool IsValid => true;

    public Authority Authority { get; set; } = Authority.Unknown;

    public void Execute(IHumanoid humanoid)
    {
        humanoid.FootIsOn = false;
    }
}
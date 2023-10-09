namespace Cactus2;
public class Effect : Typed.Object
{
    public Authority Authority { get; set; }
    public bool IsValid { get; protected set; } = true;

    public Effect()
    {
        Authority = Authority.Unknown;
    }
}

public class HitEffect : Effect
{
    public float DamageForVitality { get; set; }
    public float DamageForResilience { get; set; }

    public HitEffect() : base()
    {
    }

}

public class StabilizationEffect : Effect
{
    public StabilizationEffect() : base()
    {

    }
}

public class DestabilizationEffect : Effect
{
    public DestabilizationEffect() : base()
    {

    }
}

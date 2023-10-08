#nullable enable
using Nonno.Assets;

public class AnimationContext : Context<AnimationStateIndex>
{
    public int Layer { get; }

    public AnimationContext(int layer)
    {
        Layer = layer;
    }
}

public class AnimationStateIndex : Context.Index
{
    public AnimationStateIndex(AnimationContext context, int value) : base(context, value)
    {
    }
}

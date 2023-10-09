namespace Cactus2;
public readonly struct Appearance
{
    public Appearance(int hashCode, Transform transform, Displacement velocity, Authority authority)
    {
        HashCode = hashCode;
        Transform = transform;
        Velocity = velocity;
        Authority = authority;
    }

    public int HashCode { get; }
    public Transform Transform { get; }
    public Displacement Velocity { get; }
    public Authority Authority { get; }
}

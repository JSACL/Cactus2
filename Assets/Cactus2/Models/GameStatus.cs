public struct GameStatus
{
    float _hp;
    float _rp;

    public string Name { get; }
    public float Resilience
    {
        get => _rp;
        set
        {
            _rp = value;
            if (_rp < 0) _rp = 0;
        }
    }
    public float Vitality
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp < 0) _hp = 0;
        }
    }

    public GameStatus(string name)
    {
        _hp = default;
        _rp = default;

        Name = name;
    }
}

public struct Harm
{

}

using System;
using Nonno.Assets;

#nullable enable
public class GameStatus : IStatus
{
    float _hp;
    float _rp;

    public event EventHandler? Updated;

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
    public DateTime Time { get; set; }

    public GameStatus(string name)
    {
        _hp = default;
        _rp = default;

        Name = name;
    }

    public void Affect(Typed effect) { }
}

#nullable enable
using System.Collections.Generic;

namespace Cactus2;
public class Player : Humanoid, IPlayer
{
    int _itemNumber;
    readonly List<IItem> _items;
    readonly GameStatus _status;

    public int SelectedItemIndex
    {
        get => _itemNumber;
        set
        {
            _itemNumber = value % _items.Count;
        }
    }
    public SysGC::IList<IItem> Items => _items;
    SysGC::IReadOnlyList<IItem> IPlayer.Items => _items;
    public IStatus Status => _status;
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

    public Player(IScene scene) : base(scene)
    {
        _items = new();
        _status = new("Main Player") { Resilience = 1.0f, Vitality = 1.0f };
    }

    public void Fire(float timeSpan)
    {
        if (_items[SelectedItemIndex] is IWeapon weapon)
        {
            weapon.Trigger();
        }
    }

    protected override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        foreach (var item in _items)
        {
            // TODO: ‚±‚ÌğŒ•ª‚Í‘Ó‘ÄB
            if (item is not IEntity entity) continue;
            entity.Transform = Transform;
        }
    }
}

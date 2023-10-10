using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cactus2.Presenters;

namespace Cactus2.Views;
public class ConsoleSceneView : IPresenter<IScene>
{
    readonly List<IEntity> _entities = new();
    readonly List<object> _others = new();
    IScene? _scene;

    public IScene? Model
    {
        get => _scene;
        set
        {
            if (_scene is { } s1) s1.SceneChanged -= SceneChanged;
            _scene = value;
            if (_scene is { } s2) s2.SceneChanged += SceneChanged; 
        }
    }

    private void SceneChanged(object? sender, FamilyChangeEventArgs e)
    {
        switch (e.Action)
        {
        case SceneChangeAction.Add:
            switch (e.Object.Value)
            {
            case IPlayer p:
                _entities.Add(p);
                var cp = new ControllerPresenter() { Model = p };
                var cpv = new ConsolePlayerView() { Model = cp };
                cpv.StartThread();
                break;
            case IEntity t:
                _entities.Add(t);
                break;
            case { } o:
                _others.Add(o);
                break;
            }
            break;
        case SceneChangeAction.Remove:
            switch (e.Object.Value)
            {
            case IEntity t:
                _entities.Remove(t);
                break;
            case { } o:
                _others.Remove(o);
                break;
            }
            break;
        }
    }

    public void Update()
    {
        var now = DateTime.Now;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var entity in _entities)
        {
            entity.Time = now;
            Console.WriteLine($"{entity.Transform.Position}\t\t{entity}({entity.Authority})");
        }
        Console.ResetColor();
        foreach (var other in _others)
        {
            Console.WriteLine(other);
        }
    }
}

using Cactus2.Presenters;
using Cactus2.Views;
using Nonno.Assets.Presentation;

namespace Cactus2.Windows;

public partial class SceneView : Form, IPresenter<IScene>
{
    readonly List<IEntity> _entities = new();
    readonly List<object> _others = new();
    IScene? _scene;

    public SceneView()
    {
        InitializeComponent();
    }

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

    private void Timer1_Tick(object sender, EventArgs e)
    {
        foreach (var entity in _entities)
        {
            entity.AddTime(0.1f);
        }
    }
}

#nullable enable
using Cactus2.Presenter;

namespace Cactus2;
public class Scene : IScene, IFamilyPresenter
{
    //readonly List<IVisitor> _visitors;
    DateTime _time;
    int _count;

    public event FamilyChangeEventHandler? FamilyChanged;

    public DateTime Time { get => _time; set => _time = value; }
    public IReferee Referee { get; set; }

    public Scene(DateTime time, IReferee? referee = null)
    {
        //_visitors = new();
        _time = time;
        
        Referee = referee ?? new SuperiorReferee();
    }

    public void Add(Typed obj) => FamilyChanged?.Invoke(this, new(SceneChangeAction.Add, obj));
    public void Remove(Typed obj) => FamilyChanged?.Invoke(this, new(SceneChangeAction.Remove, obj));

    //public void Add(object obj, bool assertIsNotVisible = false)
    //{
    //    if (!assertIsNotVisible && obj is IVisible visible)
    //    {
    //        Add(visible);
    //        return;
    //    }
    //}
    //public void Add(IVisible obj)
    //{
    //    Interlocked.Increment(ref _count);
    //    foreach (var visitor in _visitors)
    //    {
    //        obj.Visit(visitor);
    //    }
    //}
    //public void Remove(object obj, bool assertIsNotVisible = false)
    //{
    //    if (!assertIsNotVisible && obj is IVisible visible)
    //    {
    //        Remove(visible);
    //        return;
    //    }
    //}
    //public void Remove(IVisible obj)
    //{
    //    foreach (var visitor in _visitors)
    //    {
    //        obj.Forgo(visitor);
    //    }
    //    Interlocked.Decrement(ref _count);
    //}

    //public void Visit(IVisitor visitor)
    //{
    //    if (_count > 0) throw new InvalidOperationException("お使いの景は、高速化のため、既にシーンに物体が存在する状態で客を迎えることができません。");
    //    _visitors.Add(visitor);
    //    visitor.Visit(this);
    //}
    //public void Forgo(IVisitor visitor)
    //{
    //    if (_count > 0) throw new InvalidOperationException("お使いの景は、高速化のため、既にシーンに物体が存在する状態で客を送ることができません。");
    //    _visitors.Remove(visitor);
    //    visitor.Forgo(this);
    //}
}
#nullable enable
using Cactus2.Presenters;

namespace Cactus2;
public class Scene : IScene
{
    //readonly List<IVisitor> _visitors;
    DateTime _time;

    public event FamilyChangeEventHandler? SceneChanged;

    public DateTime Time { get => _time; set => _time = value; }
    public IReferee Referee { get; set; }

    public Scene(DateTime time, IReferee? referee = null)
    {
        //_visitors = new();
        _time = time;
        
        Referee = referee ?? new SuperiorReferee();
    }

    public void Add(Typed obj) => SceneChanged?.Invoke(this, new(SceneChangeAction.Add, obj));
    public void Remove(Typed obj) => SceneChanged?.Invoke(this, new(SceneChangeAction.Remove, obj));

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
    //    if (_count > 0) throw new InvalidOperationException("���g���̌i�́A�������̂��߁A���ɃV�[���ɕ��̂����݂����Ԃŋq���}���邱�Ƃ��ł��܂���B");
    //    _visitors.Add(visitor);
    //    visitor.Visit(this);
    //}
    //public void Forgo(IVisitor visitor)
    //{
    //    if (_count > 0) throw new InvalidOperationException("���g���̌i�́A�������̂��߁A���ɃV�[���ɕ��̂����݂����Ԃŋq�𑗂邱�Ƃ��ł��܂���B");
    //    _visitors.Remove(visitor);
    //    visitor.Forgo(this);
    //}
}
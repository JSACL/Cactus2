//using System.Collections.Generic;
//using System.Linq;

//public class CompositeVisitor : IVisitor
//{
//    readonly List<IVisitor> _children;

//    public IEnumerable<IVisitor> Children => _children;

//    public CompositeVisitor(params IVisitor[] children)
//    {
//        _children = children.ToList();
//    }

//    public void Add(IEntity model) { foreach (var child in _children) child.Add(model); }
//    public void Add(IBullet model) { foreach (var child in _children) child.Add(model); }
//    public void Add(ILaser model) { foreach (var child in _children) child.Add(model); }
//    public void Add(IPlayer model) { foreach (var child in _children) child.Add(model); }
//    public void Add(IFirer model) { foreach (var child in _children) child.Add(model); }
//    public void Add(ISpecies1 model) { foreach (var child in _children) child.Add(model); }
//    public void Remove(IEntity model) { foreach (var child in _children) child.Remove(model); }
//    public void Remove(IBullet model) { foreach (var child in _children) child.Remove(model); }
//    public void Remove(ILaser model) { foreach (var child in _children) child.Remove(model); }
//    public void Remove(IPlayer model) { foreach (var child in _children) child.Remove(model); }
//    public void Remove(IFirer model) { foreach (var child in _children) child.Remove(model); }
//    public void Remove(ISpecies1 model) { foreach (var child in _children) child.Remove(model); }
//}
//#nullable enable
//using System;
//using UnityEngine;

//public class Appearance : IVisible
//{
//    IVisitor? _visitor;

//    public Vector3 Position { get; }
//    public IVisitor? Visitor 
//    {
//        get => _visitor;
//        set
//        {
//            if (_visitor is { })
//            {
//                _visitor.Remove(this);
//                _visitor = null;
//            }
//            if (value is { })
//            {
//                _visitor = value;
//            }
//        } 
//    }

//    public Appearance(Vector3 position)
//    {
//        Position = position;
//    }
//}

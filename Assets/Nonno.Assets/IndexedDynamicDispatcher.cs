#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

#if UNITY_2017_1_OR_NEWER
namespace Nonno.Assets
{
    public class IndexedDynamicDispatcher
    {
        readonly CorrespondenceTable<TypeIndex, Action<object?>> _table = new(TypeIndex.Context);
        readonly List<KeyValuePair<Type, Action<object?>>> _dels = new();

        public unsafe void Overload<T>(Action<T?> action) where T : class
        {
            _dels.Add(new(typeof(T), obj => action((T?)obj)));
            _table.Clear();
        }

        public void Unload<T>()
        {
            var i = _dels.FindIndex(x => x.Key == typeof(T));
            _dels.RemoveAt(i);
            _table.Clear();
        }

        public void Dispatch(Typed obj)
        {
            var v = _table[obj.Index];
            v ??= Resolve(obj.Index);
            v.Invoke(obj.Value);
        }

        Action<object?> Resolve(TypeIndex index)
        {
            var t = index.Type;

            foreach (var (key, value) in _dels)
            {
                if (key.IsAssignableFrom(t))
                {
                    _table[index] = value;
                    return value;
                }
            }

            throw new Exception("îzëóÇ…é∏îsÇµÇ‹ÇµÇΩÅB");
        }
    }
}
#else
namespace Nonno.Assets
{
    public class IndexedDynamicDispatcher
    {
        readonly CorrespondenceTable<TypeIndex, Value> _table = new(TypeIndex.Context);
        readonly List<KeyValuePair<Type, Value>> _dels = new();

        public unsafe void Overload<T>(Action<T?> action) where T : class
        {
            var vt = new Value<T>(action);
            var v = *(Value*)&vt;
            _dels.Add(new(typeof(T), v));
            _table.Clear();
        }

        public void Unload<T>()
        {
            var i = _dels.FindIndex(x => x.Key == typeof(T));
            _dels.RemoveAt(i);
            _table.Clear();
        }
        
        public void Dispatch(Typed obj)
        {
            var v = _table[obj.Index];
            if (!v.HasValue)
            {
                v = Resolve(obj.Index);
            }
            v.Invoke(obj.Value);
        }

        Value Resolve(TypeIndex index)
        {
            var t = index.Type;

            foreach (var (key, value) in _dels)
            {
                if (key.IsAssignableFrom(t))
                {
                    _table[index] = value;
                    return value;
                }
            }
            //while (t is not null)
            //{
            //    if (_dels.TryGetValue(t, out var r))
            //    {
            //        _table[index] = r;
            //        return r;
            //    }

            //    t = t.BaseType;
            //}

            //t = index.Type.Inter;

            //foreach (var t2 in t.GetInterfaces())
            //{
            //    if (_dels.TryGetValue(t2, out var i))
            //    {
            //        _table[index] = i;
            //        return i;
            //    }
            //}

            throw new Exception("îzëóÇ…é∏îsÇµÇ‹ÇµÇΩÅB");
        }

        [StructLayout(LayoutKind.Sequential)]
        readonly struct Value
        {
            readonly Action<object?> _action;

            public bool HasValue => _action is not null;

            public Value(Action<object?> action)
            {
                _action = action;
            }

            public void Invoke(object? value) 
            {
#if DEBUG
                if (value is not null)
                Debug.Assert(_action.GetType().GenericTypeArguments[0].IsAssignableFrom(value.GetType()));
#endif
                _action(value); 
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        readonly struct Value<T>
        {
            readonly Action<T?> _action;

            public Value(Action<T?> action)
            {
                _action = action;
            }

            public void Invoke(T? value) => _action(value);
        }
    }
}
#endif
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static Nonno.Assets.Collections.Utils;

namespace Cactus2.Presenters;
public class ControllerPresenter : Presenter<IPlayer>, IControllerPresenter
{
    readonly IndexedSwitch<ExternalInterruption> _switch;
    readonly RelativeValueInterruption _mouse_x, _mouse_y, _mouse_wheel;
    readonly AbsoluteValueInterruption<bool> _w, _s, _d, _a, _m_l, _m_r, _shift, _space;

    int _mouse_x_val, _mouse_y_val;

    public ReadOnlySpan<ExternalInterruption> ValidInterruptions => _switch.CaseIndexes;

    public Transform Transform => Transform.Identity;

    public ControllerPresenter()
    {
        _w = GetInterruption<AbsoluteValueInterruption<bool>>("w");
        _s = GetInterruption<AbsoluteValueInterruption<bool>>("s");
        _d = GetInterruption<AbsoluteValueInterruption<bool>>("d");
        _a = GetInterruption<AbsoluteValueInterruption<bool>>("a");
        _m_l = GetInterruption<AbsoluteValueInterruption<bool>>("mouse-left");
        _m_r = GetInterruption<AbsoluteValueInterruption<bool>>("mouse-right");
        _shift = GetInterruption<AbsoluteValueInterruption<bool>>("shift");
        _space = GetInterruption<AbsoluteValueInterruption<bool>>("space");
        _mouse_x = GetInterruption<RelativeValueInterruption>("mouse-x");
        _mouse_y = GetInterruption<RelativeValueInterruption>("mouse-y");
        _mouse_wheel = GetInterruption<RelativeValueInterruption>("mouse-wheel");

        _switch = new(ExternalInterruption.Context);
        _switch.CaseDefault(_w);
        _switch.CaseDefault(_s);
        _switch.CaseDefault(_d);
        _switch.CaseDefault(_a);
        _switch.CaseDefault(_m_l);
        _switch.CaseDefault(_m_r);
        _switch.CaseDefault(_shift);
        _switch.CaseDefault(_space);
        _switch.CaseDefault(_mouse_x);
        _switch.CaseDefault(_mouse_y);
        _switch.CaseDefault(_mouse_wheel);
        _switch.Case(GetInterruption<ExternalInterruption>("e"), x => Model.SelectedItemIndex++);
        _switch.Case(GetInterruption<ExternalInterruption>("escape"), x => Model = null);
    }

    public void Interrupt(ExternalInterruption ei)
    {
        _switch.Switch(ei);
    }

    public void AddTime(float deltaTime)
    {
        int yaw = _mouse_x.Value - _mouse_x_val, pitch = _mouse_y.Value - _mouse_y_val;
        _mouse_x_val = _mouse_x.Value; _mouse_y_val = _mouse_y.Value;

        Model.Turn((float)yaw / 100, (float)pitch / 100);
        Model.IsRunning = _shift.Value;
        Model.Seek(_w.Value, _s.Value, _d.Value, _a.Value, deltaTime * 100);
        if (_m_l.Value) Model.Fire(deltaTime);
    }
}

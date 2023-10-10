using System.Numerics;
using Cactus2.Presenters;
using Nonno.Assets.Collections;
using UnityEngine;

public class ControllerViewModel : ViewModel<IControllerPresenter>
{
    readonly CorrespondenceTable<ExternalInterruption, (int, int)> _codes = new(ExternalInterruption.Context);

    protected void Update()
    {
        var vIs = Model.ValidInterruptions;
        for (int i = 0; i < vIs.Length; i++)
        {
            var e = vIs[i];
            var (k, code) = _codes[e];
            switch (k)
            {
            case 0:
                switch (e)
                {
                case AbsoluteValueInterruption<bool> c:
                    _codes[e] = c.Name switch
                    {
                        "a" => (1, (int)KeyCode.A),
                        "b" => (1, (int)KeyCode.B),
                        "c" => (1, (int)KeyCode.C),
                        "d" => (1, (int)KeyCode.D),
                        "e" => (1, (int)KeyCode.E),
                        "f" => (1, (int)KeyCode.F),
                        "g" => (1, (int)KeyCode.G),
                        "h" => (1, (int)KeyCode.H),
                        "i" => (1, (int)KeyCode.I),
                        "j" => (1, (int)KeyCode.J),
                        "k" => (1, (int)KeyCode.K),
                        "l" => (1, (int)KeyCode.L),
                        "m" => (1, (int)KeyCode.M),
                        "n" => (1, (int)KeyCode.N),
                        "o" => (1, (int)KeyCode.O),
                        "p" => (1, (int)KeyCode.P),
                        "q" => (1, (int)KeyCode.Q),
                        "r" => (1, (int)KeyCode.R),
                        "s" => (1, (int)KeyCode.S),
                        "t" => (1, (int)KeyCode.T),
                        "u" => (1, (int)KeyCode.U),
                        "v" => (1, (int)KeyCode.V),
                        "w" => (1, (int)KeyCode.W),
                        "x" => (1, (int)KeyCode.X),
                        "y" => (1, (int)KeyCode.Y),
                        "z" => (1, (int)KeyCode.Z),
                        "spacebar" => (1, (int)KeyCode.Space),
                        "shift-left" => (1, (int)KeyCode.LeftShift),
                        "mouse-left" => (4, 0),
                        "mouse-right" => (4, 2),
                        _ => (-1, 0)
                    };
                    break;
                case AbsoluteValueInterruption<System.Numerics.Vector2>:
                    _codes[e] = (9, 0);
                    break;
                case RelativeValueInterruption r:
                    _codes[e] = r.Name switch
                    {
                        "mouse-x" => (7, 0),
                        "mouse-y" => (8, 0),
                        _ => (-1, 0)
                    };
                    break;
                default:
                    Debug.Log($"äOïîäÑçû {e.Name} ÇÕèàóùÇ≥ÇÍÇ‹ÇπÇÒÅB");
                    _codes[e] = (-1, 0);
                    break;
                }
                break;
            case 1:
                ((AbsoluteValueInterruption<bool>)e).Value = Input.GetKey((KeyCode)code);
                Model.Interrupt(e);
                break;
            case 2:
                if (Input.GetKeyDown((KeyCode)code)) Model.Interrupt(e);
                break;
            case 3:
                if (Input.GetKeyUp((KeyCode)code)) Model.Interrupt(e);
                break;
            case 4:
                ((AbsoluteValueInterruption<bool>)e).Value = Input.GetMouseButton(code);
                Model.Interrupt(e);
                break;
            case 5:
                if (Input.GetMouseButtonDown(code)) Model.Interrupt(e);
                break;
            case 6:
                if (Input.GetMouseButtonUp(code)) Model.Interrupt(e);
                break;
            case 7:
                var v1 = (int)(100 * Input.GetAxisRaw("Mouse X"));
                ((RelativeValueInterruption)e).Add(v1);
                if (v1 != 0) Model.Interrupt(e);
                break;
            case 8:
                var v2 = (int)(100 * Input.GetAxisRaw("Mouse Y"));
                ((RelativeValueInterruption)e).Add(v2);
                if (v2 != 0) Model.Interrupt(e);
                break;
            case 9:
                var pos = Input.mousePosition;
                ((AbsoluteValueInterruption<System.Numerics.Vector2>)e).Value = new(pos.x, pos.y);
                Model.Interrupt(e);
                break;
            default:
                break;
            }
        }

        Model.AddTime(Time.deltaTime);
    }
}
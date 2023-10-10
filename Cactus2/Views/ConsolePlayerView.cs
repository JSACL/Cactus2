using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cactus2.Presenters;
using static Nonno.Assets.Collections.Utils;

namespace Cactus2.Views;
public class ConsolePlayerView : IPresenter<IControllerPresenter>
{
    readonly AbsoluteValueInterruption<bool> _w = GetInterruption<AbsoluteValueInterruption<bool>>("w");
    readonly AbsoluteValueInterruption<bool> _s = GetInterruption<AbsoluteValueInterruption<bool>>("s");
    readonly AbsoluteValueInterruption<bool> _d = GetInterruption<AbsoluteValueInterruption<bool>>("d");
    readonly AbsoluteValueInterruption<bool> _a = GetInterruption<AbsoluteValueInterruption<bool>>("a");
    readonly AbsoluteValueInterruption<bool> _space = GetInterruption<AbsoluteValueInterruption<bool>>("spacebar");

    public IControllerPresenter? Model { private get; set; }

    public void StartThread()
    {
        var t = new Thread(_ => StartLoop());
        t.IsBackground = false;
        t.Start();
    }

    public void StartLoop()
    {
        while (true)
        {
            if (Model is null) break;

            var info = Console.ReadKey();
            switch (info.Key)
            {
            case ConsoleKey.W:
                _w.Value = true;
                Model.Interrupt(_w);
                break;
            case ConsoleKey.S:
                _s.Value = true;
                Model.Interrupt(_s);
                break;
            case ConsoleKey.D:
                _d.Value = true;
                Model.Interrupt(_d);
                break;
            case ConsoleKey.A:
                _a.Value = true;
                Model.Interrupt(_a);
                break;
            case ConsoleKey.Spacebar:
                _space.Value = true;
                Model.Interrupt(_space);
                break;
            }
        }
    }
}

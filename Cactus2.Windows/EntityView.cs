using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nonno.Assets.Presentation;

namespace Cactus2.Windows;
public partial class EntityView : UserControl, IPresenter<IEntity>
{
    public EntityView()
    {
        InitializeComponent();
    }

    public IEntity? Model { get; set; }

    private void Timer1_Tick(object sender, EventArgs e)
    {
        if (Model is null) return;

        Model.AddTime(0.1f);
        var pos = Model.Transform.Position;
        Location = new((int)pos.X, (int)pos.Y);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cactus2;
using Cactus2.Presenters;

public class BulletViewModel : EntityViewModel<IBulletPresenter>
{
    public StayEffectComponent stay;

    protected override void Connect()
    {
        stay.Effect = Model.HitEffect;
        base.Connect();
    }
    protected override void Disconnect()
    {
        base.Disconnect();
        stay.Effect = null;
    }
}

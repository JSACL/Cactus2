using Cactus2.Presenter;
using UnityEngine;
using UE = UnityEngine;

public class StatusGaugeViewModel : ViewModel<IStatusGaugePresenter>
{
    public UE::Transform HPBar { get; }
    public UE::Transform RPBar { get; }

    protected override void Connect()
    {
        Model.PropertyChanged += Model_PropertyChanged;
        base.Connect();
    }
    protected override void Disconnect() 
    {
        base.Disconnect();
        Model.PropertyChanged -= Model_PropertyChanged;
    }

    private void Model_PropertyChanged()
    {
        Relength(HPBar, Model.HP);
        Relength(RPBar, Model.RP);

        static void Relength(UE::Transform transform, float value)
        {
            var s = transform.localScale;
            s.x = value;
            transform.localScale = s;
        }
    }

    private void Update() => Model.AddTime(Time.deltaTime);
}


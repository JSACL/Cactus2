using Cactus2.Presenters;
using TMPro;
using UE = UnityEngine;

public class WeaponViewModel : EntityViewModel<IWeaponPresenter>
{
    public TextMeshPro text;
    public UE::Transform gauge;

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
        text.text = Model.Name;
        gauge.localScale = new(Model.Value1, 1);
    }
}


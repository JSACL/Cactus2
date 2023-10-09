#nullable enable

using Cactus2.Presenter;
using TMPro;

public class ItemViewModel : ViewModel<IItemPresenter>
{
    public TextMeshPro text;

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
    }
}

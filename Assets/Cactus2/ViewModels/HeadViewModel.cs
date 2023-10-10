using Cactus2;
using Cactus2.Presenters;
using UE = UnityEngine;

public class HeadViewModel : ViewModel<IHeadPresenter>
{
    public UE::Transform head;

    protected override void Connect()
    {
        Model.PropertyChanged += Model_PropertyChanged;
        Model.FamilyChanged += Model_FamilyChanged;
        base.Connect();
    }
    protected override void Disconnect()
    {
        base.Disconnect();
        Model.PropertyChanged -= Model_PropertyChanged;
        Model.FamilyChanged -= Model_FamilyChanged;
    }

    private void Model_PropertyChanged()
    {
        head.localRotation = Model.HeadLocalRotation.ToUnityQuaternion();
    }

    private void Model_FamilyChanged(object sender, FamilyChangeEventArgs e)
    {
        switch (e.Action)
        {
        case SceneChangeAction.Add:
            break;
        case SceneChangeAction.Remove:
            break;
        }
    }
}

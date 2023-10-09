using Cactus2;
using Cactus2.Presenter;
using UnityEngine;
using Time = UnityEngine.Time;

public class CombatUIViewModel : ViewModel<ICombatUIPresenter>
{
    public ItemListViewModel itemList;
    public StatusGaugeViewModel statusGauge;

    public CombatUIViewModel()
    {
    }

    protected override void Connect()
    {
        base.Connect();
        Model.FamilyChanged += Model_FamilyChanged;
    }
    protected override void Disconnect()
    {
        Model.FamilyChanged -= Model_FamilyChanged;
        base.Disconnect();
    }

    private void Model_FamilyChanged(object sender, FamilyChangeEventArgs e)
    {
        switch (e.Action)
        {
        case SceneChangeAction.Add:
            if (e.Object.Value is IStatusGaugePresenter sgp) statusGauge.Model = sgp;
            if (e.Object.Value is IFamilyPresenter iproc) itemList.Model = iproc;
            break;
        case SceneChangeAction.Remove:
            if (Equals(e.Object.Value, statusGauge.Model)) statusGauge.Model = null;
            if (Equals(e.Object.Value, itemList.Model)) itemList.Model = null;
            break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) Model.Select();
        Model.AddTime(Time.deltaTime);
    }
}

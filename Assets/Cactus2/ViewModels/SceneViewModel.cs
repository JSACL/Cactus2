using Cactus2;
using UnityEngine.SceneManagement;
using Time = UnityEngine.Time;
using UESM = UnityEngine.SceneManagement;

public class SceneViewModel : ViewModel<Cactus2.Scene>
{
    readonly FamilyHelper _helper;

    public UESM::Scene Scene => _helper.Scene;

    public SceneViewModel()
    {
        _helper = new();
    }

    protected new void Awake()
    {
        base.Awake();

        _helper.Scene = SceneManager.GetActiveScene();

        //_helper.S<IPlayer, HumanoidPresenter<IPlayer>, HumanoidViewModel>("Assets/Cactus2/Views/Player.prefab");
        _helper.IgnoreUnknownModel();
    }

    protected override void Connect()
    {
        base.Connect();
        Model.FamilyChanged += _helper.HandleFamilyChange;
    }
    protected override void Disconnect()
    {
        Model.FamilyChanged -= _helper.HandleFamilyChange;
        base.Disconnect();
    }

    protected void Update()
    {
        ((IScene)Model).AddTime(Time.deltaTime);
    }
}

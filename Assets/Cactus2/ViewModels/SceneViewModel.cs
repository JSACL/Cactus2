using Cactus2;
using Cactus2.Presenters;
using UnityEngine.SceneManagement;
using Time = UnityEngine.Time;
using UESM = UnityEngine.SceneManagement;

public class SceneViewModel : ViewModel<IScene>
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

        _helper.S<IPlayer, HumanoidPresenter<IPlayer>, HumanoidViewModel>("Assets/Cactus2/Views/Player.prefab");
        _helper.S<IBullet, BulletPresenter<IBullet>, EntityViewModel>("Assets/Cactus2/Views/Bullet.prefab");
        _helper.S<IWeapon, WeaponPresenter<IWeapon>, EntityViewModel>("Assets/Cactus2/Views/Entity.prefab");
        _helper.S<IGround, GroundViewModel>("Assets/Cactus2/Views/Ground.prefab");
        _helper.IgnoreUnknownModel();
    }

    protected override void Connect()
    {
        base.Connect();
        Model.SceneChanged += _helper.HandleFamilyChange;
    }
    protected override void Disconnect()
    {
        Model.SceneChanged -= _helper.HandleFamilyChange;
        base.Disconnect();
    }

    protected void Update()
    {
        Model.AddTime(Time.deltaTime);
    }
}

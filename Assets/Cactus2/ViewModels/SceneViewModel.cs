using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Nonno.Assets;
using Nonno.Assets.Presentation;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UESM = UnityEngine.SceneManagement;

public class SceneViewModel : ViewModel<Scene>
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

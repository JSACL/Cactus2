#nullable enable

using System.Collections.Generic;
using Nonno.Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemListViewModel : ViewModel<IFamilyPresenter>
{
    readonly FamilyHelper _h;

    public ItemListViewModel()
    {
        _h = new();
    }

    protected new void Awake()
    {
        base.Awake();

        _h.Scene = SceneManager.GetActiveScene();

        _h.S<IWeapon, WeaponPresenter<IWeapon>, WeaponViewModel>("Assets/Cactus2/Views/Weapon.prefab");
    }

    protected override void Connect()
    {
        Model.FamilyChanged += _h.HandleFamilyChange;
        base.Connect();
    }
    protected override void Disconnect()
    {
        base.Disconnect();
        Model.FamilyChanged -= _h.HandleFamilyChange;
    }
}
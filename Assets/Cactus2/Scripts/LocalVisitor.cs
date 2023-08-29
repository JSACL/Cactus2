#nullable enable
using UnityEngine;
using System;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class LocalVisitor : MonoBehaviour, IVisitor
{
    [SerializeField]
    GameObject _playerView;
    [SerializeField]
    GameObject _combatUIView;
    [SerializeField]
    GameObject _firerView;
    [SerializeField]
    GameObject _bulletView;

    ViewModels<IPlayer> _playerVMs;
    ViewModels<IPlayer> _combatUIVMs;
    ViewModels<IBullet> _bulletVMs;
    ViewModels<IFirer> _firerVMs;

    private void Start()
    {
        _playerVMs = new(_playerView);
        _combatUIVMs = new(_combatUIView);
        _bulletVMs = new(_bulletView);
        _firerVMs = new(_firerView);

        var firer1 = new Firer1()
        {
            Visitor = this
        };
        var player = new Player(firer1, firer1, firer1)
        {
            Vigor = 1,
            RepairPoint = 1,
            Visitor = this
        };
    }

    public void Add(IPlayer model)
    {
        _playerVMs.Add(model);
        _combatUIVMs.Add(model);
    }
    public void Add(IEntity model)
    {
        
    }
    public void Add(IBullet model)
    {
        _bulletVMs.Add(model);
    }
    public void Add(IFirer model)
    {
        _firerVMs.Add(model);
    }

    public void Remove(IPlayer model)
    {
        _playerVMs.Remove(model);
        _combatUIVMs.Remove(model);
    }
    public void Remove(IEntity model)
    {

    }
    public void Remove(IBullet model)
    {
        _bulletVMs.Remove(model);
    }
    public void Remove(IFirer model)
    {
        _firerVMs.Remove(model);
    }
}

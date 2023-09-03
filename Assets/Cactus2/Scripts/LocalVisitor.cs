#nullable enable
using UnityEngine;
using System;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using DB = System.Diagnostics.DebuggerBrowsableAttribute;
using SF = UnityEngine.SerializeField;
using DBS = System.Diagnostics.DebuggerBrowsableState;
using GOS = IObjectSource<UnityEngine.GameObject>;
using Unity.VisualScripting;

public class LocalVisitor : MonoBehaviour, IVisitor
{
    //[Header("開始時充現有View以新Model否")]
    //[SF]
    //bool _attachModelOnStarting;
    [Header("掟各対Model所応ViewModel")]
    public GameObject playerView;
    public GameObject combatUIView;
    public GameObject firerView;
    public GameObject bulletView;
    public GameObject laserView;
    public GameObject entityView;
    public GameObject species1View;

    GOS _playerViewSource;
    GOS _combatUIViewSource;
    GOS _firerViewSource;
    GOS _bulletViewSource;
    GOS _laserViewSource;
    GOS _entityViewSource;
    GOS _species1ViewSource;

    [DB(DBS.Never)]
    readonly Dictionary<object, ViewModel[]> _vMs = new();

    public IEnumerable<object> Models => _vMs.Keys;

    private void Awake()
    {
        _playerViewSource = new GameObjectSource(playerView);
        _combatUIViewSource = new GameObjectSource(combatUIView);
        _firerViewSource = new GameObjectSource(firerView);
        _bulletViewSource = new GameObjectPool(bulletView);
        _laserViewSource = new GameObjectPool(laserView);
        _entityViewSource = new GameObjectPool(entityView);
        _species1ViewSource = new GameObjectPool(species1View);
    }

    void Start()
    {
        var p = new Player() { Visitor = this };
        Referee.SetInfo(p, new(10));
        p.Items.Add(new FugaFirer() { Visitor = this });
        p.Items.Add(new FugaFirer() { Visitor = this });
        p.Items.Add(new FugaFirer() { Visitor = this });
        var s = new FugaEnemy() { Visitor = this, Velocity = Vector3.forward, Position = new Vector3(0, 10, 0) };
    }

    public void AddStatic(object model, params ViewModel[] with)
    {
        _vMs.Add(model, with);
    }

    public void Add(IPlayer model)
    {
        if (_vMs.ContainsKey(model)) return;
        var pVM = _playerViewSource.Get().GetComponent<ViewModel>();
        var cUIVM = _combatUIViewSource.Get().GetComponent<ViewModel>();
        pVM.Model = model;
        cUIVM.Model = model;
        _vMs.Add(model, new ViewModel[] { pVM, cUIVM });
    }
    public void Remove(IPlayer model)
    {
        var vMs = _vMs[model];
        _playerViewSource.Release(vMs[0].gameObject);
        _combatUIViewSource.Release(vMs[1].gameObject);
        vMs[0].Model = null;
        vMs[1].Model = null;
        _vMs.Remove(model);
    }

    public void Add(IEntity model)
    {
        if (_vMs.ContainsKey(model)) return;
        var eVM = _entityViewSource.Get().GetComponent<ViewModel>();
        eVM.Model = model;
        _vMs.Add(model, new ViewModel[] { eVM });
    }
    public void Remove(IEntity model)
    {
        var vMs = _vMs[model];
        _entityViewSource.Release(vMs[0].gameObject);
        vMs[0].Model = null;
        _vMs.Remove(model);
    }

    public void Add(IBullet model)
    {
        if (_vMs.ContainsKey(model)) return;
        var eVM = _bulletViewSource.Get().GetComponent<ViewModel>();
        eVM.Model = model;
        _vMs.Add(model, new ViewModel[] { eVM });
    }
    public void Remove(IBullet model)
    {
        var vMs = _vMs[model];
        _bulletViewSource.Release(vMs[0].gameObject);
        vMs[0].Model = null;
        _vMs.Remove(model);
    }

    public void Add(ILaser model)
    {
        if (_vMs.ContainsKey(model)) return;
        var eVM = _laserViewSource.Get().GetComponent<ViewModel>();
        eVM.Model = model;
        _vMs.Add(model, new ViewModel[] { eVM });
    }
    public void Remove(ILaser model)
    {
        var vMs = _vMs[model];
        _laserViewSource.Release(vMs[0].gameObject);
        vMs[0].Model = null;
        _vMs.Remove(model);
    }

    public void Add(IFirer model)
    {
        if (_vMs.ContainsKey(model)) return;
        var eVM = _firerViewSource.Get().GetComponent<ViewModel>();
        eVM.Model = model;
        _vMs.Add(model, new ViewModel[] { eVM });
    }
    public void Remove(IFirer model)
    {
        var vMs = _vMs[model];
        _firerViewSource.Release(vMs[0].gameObject);
        vMs[0].Model = null;
        _vMs.Remove(model);
    }

    public void Add(ISpecies1 model)
    {
        if (_vMs.ContainsKey(model)) return;
        var eVM = _species1ViewSource.Get().GetComponent<ViewModel>();
        eVM.Model = model;
        _vMs.Add(model, new ViewModel[] { eVM });
    }
    public void Remove(ISpecies1 model)
    {
        var vMs = _vMs[model];
        _species1ViewSource.Release(vMs[0].gameObject);
        vMs[0].Model = null;
        _vMs.Remove(model);
    }
}

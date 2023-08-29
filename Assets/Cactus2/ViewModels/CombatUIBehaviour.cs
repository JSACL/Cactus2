using System;
using System.Linq;
using UnityEngine;
using static Utils;

public class CombatUIBehaviour : MonoBehaviour, IViewModel<IPlayer>
{
    IPlayer _model;

    [Header("クールタイムを表すパネルを格納")]
    [SerializeField]
    GameObject[] _gaugePanels_cooldownTime;
    float[] _initialGaugeLengthes_cooldownTime;

    [Header("現在選択中のスキルを示すパネル")]
    [SerializeField]
    GameObject _markPanel_selectedItem;
    [Header("MarkPanel_selectedItemの表示位置")]
    public Transform[] _markPivots_selectedItem;

    [Header("現在のHPを示すパネル")]
    [SerializeField]
    GameObject _gaugePanel_hitPoint;
    float _initialGaugeLength_hitPoint;

    [Header("現在のRPを示すパネル")]
    [SerializeField]
    GameObject _gaugePanel_repairPoint;
    float _initialGaugeLength_repairPoint;

    [Header("ロックオンパネル")]
    [SerializeField]
    GameObject _focusPanel;

    public IPlayer Model
    {
        get => _model;
        set
        {
            if (_model is { })
            {
                _model = null;
            }
            if (value is { })
            {
                _model = value;
            }
            enabled = _model is { };
        }
    }

    void Start()
    {
        _initialGaugeLengthes_cooldownTime = _gaugePanels_cooldownTime.Select(x => x.transform.localScale.x).ToArray();
        _initialGaugeLength_hitPoint = _gaugePanel_hitPoint.transform.localScale.x;
        _initialGaugeLength_repairPoint = _gaugePanel_repairPoint.transform.localScale.x;
    }

    void Update()
    {
        Assert(Model is not null);

        if (Input.GetKeyDown(KeyCode.E)) Model.SelectedItemIndex++;

        _markPanel_selectedItem.transform.position = _markPivots_selectedItem[Model.SelectedItemIndex].position;

        Relength(_gaugePanel_hitPoint, Model.Vigor / ConstantValues.PLAYER_VIGOR_STANDARD * _initialGaugeLength_hitPoint);
        Relength(_gaugePanel_repairPoint, Model.Vigor / ConstantValues.PLAYER_VIGOR_STANDARD * _initialGaugeLength_repairPoint);
        for (int i = 0; i < _initialGaugeLengthes_cooldownTime.Length; i++)
        {
            Relength(_gaugePanels_cooldownTime[i], Model.Items[i].CooldownTimeRemaining / Model.Items[i].CooldownTime * _initialGaugeLengthes_cooldownTime[i]);
        }

        static void Relength(GameObject obj, float length)
        {
            var lS = obj.transform.localScale;
            lS.x = length;
            obj.transform.localScale = lS;
        }
    }
}
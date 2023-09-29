using System;
using System.Linq;
using UnityEngine;
using static Utils;

public class CombatUIViewModel : ViewModel<IPlayer>
{
    [Header("�N�[���^�C����\���p�l�����i�[")]
    [SerializeField]
    GameObject[] _gaugePanels_cooldownTime;
    float[] _initialGaugeLengthes_cooldownTime;

    [Header("���ݑI�𒆂̃X�L���������p�l��")]
    [SerializeField]
    GameObject _markPanel_selectedItem;
    [Header("MarkPanel_selectedItem�̕\���ʒu")]
    public Transform[] _markPivots_selectedItem;

    [Header("���݂�HP�������p�l��")]
    [SerializeField]
    GameObject _gaugePanel_hitPoint;
    float _initialGaugeLength_hitPoint;

    [Header("���݂�RP�������p�l��")]
    [SerializeField]
    GameObject _gaugePanel_repairPoint;
    float _initialGaugeLength_repairPoint;

    [Header("���b�N�I���p�l��")]
    [SerializeField]
    GameObject _focusPanel;

    [Header("���S�����")]
    [SerializeField]
    GameObject _died;

    protected override void Connect()
    {
        base.Connect();

        Model.Died += Model_Died;
    }
    protected override void Disconnect()
    {
        Model.Died -= Model_Died;

        base.Disconnect();
    }

    private void Model_Died(object sender, EventArgs e)
    {
        _died.SetActive(true);
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

        Relength(_gaugePanel_hitPoint, Model.Vitality / ConstantValues.PLAYER_VIGOR_STANDARD * _initialGaugeLength_hitPoint);
        Relength(_gaugePanel_repairPoint, Model.Vitality / ConstantValues.PLAYER_VIGOR_STANDARD * _initialGaugeLength_repairPoint);
        for (int i = 0; i < _initialGaugeLengthes_cooldownTime.Length; i++)
        {
            if (Model.Items[i] is IWeapon w)
            {
            Relength(_gaugePanels_cooldownTime[i], w.CooldownTimeRemaining / w.CooldownTime * _initialGaugeLengthes_cooldownTime[i]);
            }
        }

        static void Relength(GameObject obj, float length)
        {
            var lS = obj.transform.localScale;
            lS.x = length;
            obj.transform.localScale = lS;
        }
    }
}
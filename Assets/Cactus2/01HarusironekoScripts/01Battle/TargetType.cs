using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetType : MonoBehaviour
{
    public int Group;
    [Header("Playerならtrue")]
    public bool IsPlayer;
    [Header("IsPlayerの時のみ設定")]
    public BattleOperation BattleOperationScript;
    public string Name;
    public int HP;
    private int TmpHP;
    public int MaxHP;
    public int RP;
    private int TmpRP;
    public int MaxRP;
    [Header("RP0の時、RPの自動回復を初めるまでの時間")]
    public float RPRecoverTime;
    private float RPRecoverTimer = 0f;
    [Header("RPの自動回復間隔")]
    public float RPDurationTime;
    [Header("HPが最小の時のRPの自動回復間隔の遅延")]
    public float WorstRPDurationTime;
    private float RPDurationTimer = 0f;
    [Header("RPの自動回復間隔がHPの割合にどれくらい影響を受けるか。")]
    public float RPsHPRelation;
    //trueになるとRPの自動回復を行う。
    private bool RPRecover;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HP != TmpHP)
        {
            if(HP < 0)
            {
                HP =0;
            }
            if(HP > MaxHP)
            {
                HP = MaxHP;
            }
            if(IsPlayer)
            {
                float Rate = (float)HP/(float)MaxHP;
                BattleOperationScript.HPPanel.transform.localScale = new Vector3(Rate*BattleOperationScript.FirstHPPanelScale , BattleOperationScript.HPPanel.transform.localScale.y,BattleOperationScript.HPPanel.transform.localScale.z);
            }
            TmpHP = HP;
        }
        if(RP != TmpRP)
        {
            if(RP < 0)
            {
                RP =0;
            }
            if(RP > MaxRP)
            {
                HP = MaxRP;
            }
            if(IsPlayer)
            {
                float Rate = (float)RP/(float)MaxRP;
                BattleOperationScript.RPPanel.transform.localScale = new Vector3(Rate*BattleOperationScript.FirstRPPanelScale , BattleOperationScript.RPPanel.transform.localScale.y,BattleOperationScript.RPPanel.transform.localScale.z);
            }
            if(RP < MaxRP)
            {
                if(RP==0)
                {
                    RPRecover = false;
                    RPRecoverTimer = 0f;
                }else
                {
                    RPRecover = true;
                }
            }else
            {
                RPRecover = false;
            }
            TmpRP = RP;
        }
        if(RP == 0 && !RPRecover)
        {
            RPRecoverTimer += Time.deltaTime;
            if(RPRecoverTimer >  RPRecoverTime)
            {
                RPRecover = true; 
            }
        }
        if(RPRecover)
        {
            RPDurationTimer += Time.deltaTime;
            if(RPDurationTimer > RPDurationTime + (WorstRPDurationTime*(1 - ((float)HP/(float)MaxHP))* RPsHPRelation))
            {
                RP++;
                RPDurationTimer = 0f;
            }
        }
    }
}

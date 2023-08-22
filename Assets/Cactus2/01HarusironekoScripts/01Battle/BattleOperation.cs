using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOperation : MonoBehaviour
{
    [Header("クールタイムを表すパネルを格納")]
    public GameObject[] CoolPanel;
    [HideInInspector]
    public float[] FirstCoolPanelScale;
    [Header("Fire.csを格納")]
    public Fire[] FireScript;
    [Header("現在選択中のスキルを示すパネル")]
    public GameObject FirePanel;
    [Header("FirePanelの表示位置")]
    public Transform[] FireDisplay;
    [Header("現在のHPを示すパネル")]
    public GameObject HPPanel;
    [HideInInspector]
    public float FirstHPPanelScale;
    [Header("現在のRPを示すパネル")]
    public GameObject RPPanel;
    [Header("ロックオンパネル")]
    public GameObject RockOnPanel;
    [HideInInspector]
    public float FirstRPPanelScale;
    [HideInInspector]
    public bool[] Fired;
    private int FireNumber = 0;
    void Start()
    {
        Array.Resize(ref FirstCoolPanelScale,CoolPanel.Length);
        Array.Resize(ref Fired,CoolPanel.Length);
        for(int i=0;i<CoolPanel.Length;i++)
        {
            FirstCoolPanelScale[i] = CoolPanel[i].transform.localScale.x;
        }
        FirstHPPanelScale = HPPanel.transform.localScale.x;
        FirstRPPanelScale = RPPanel.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FireNumber++;
            if(FireNumber > FireScript.Length -1)
            {
                FireNumber = 0;
            }
            FirePanel.transform.position = FireDisplay[FireNumber].position;
        }
        if(Input.GetMouseButtonDown(0) && !Fired[FireNumber])
        {
            FireScript[FireNumber].FireBullet();
        }
    }
}

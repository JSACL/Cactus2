using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("このオブジェクトが何番目の攻撃かを設定します。複数のプリセットを作る場合は、自動割り当てスクリプトを作ってください。")]
    public int ID;
    [Header("弾丸。ここを配列にして、発射数を管理すれば複数弾を実装できます。")]
    public GameObject Bullet;
    [Header("このオブジェクトは、弾丸の発射口を指定してください。")]
    public GameObject Pivot;
    [Header("このオブジェクトの親であるBattleOperationScriptを指定してください。")]
    public BattleOperation BattleOperationScript;
    [Header("このオブジェクトのクールタイムを設定します")]
    public float CoolTime;
    private float CoolTimer = 0f;
    [HideInInspector]
    public bool Fired;
    public bool TmpFired;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(Fired)
        {
            CoolTimer += Time.deltaTime;
            float Rate = CoolTimer/CoolTime;
            BattleOperationScript.CoolPanel[ID].transform.localScale = new Vector3(Rate*BattleOperationScript.FirstCoolPanelScale[ID] , BattleOperationScript.CoolPanel[ID].transform.localScale.y,BattleOperationScript.CoolPanel[ID].transform.localScale.z);
            if(CoolTimer > CoolTime)
            {
                Fired = false;
            }
        }
        if(TmpFired != Fired)
        {
            BattleOperationScript.Fired[ID] = Fired;
        }
        TmpFired = Fired;
    }
    public void FireBullet()
    {
        CoolTimer = 0f;
        Bullet.transform.position = Pivot.transform.position;
        Bullet.transform.rotation = Pivot.transform.rotation;
        Fired = true;
        Bullet.SetActive(true);
    }
}

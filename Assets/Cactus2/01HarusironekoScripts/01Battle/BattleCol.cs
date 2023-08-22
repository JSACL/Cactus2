using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCol : MonoBehaviour
{
    [Header("自分のTargetTypeScriptを格納してください")]
    public TargetType TargetTypeScript;
    [Header("Player側なら0,敵側なら1。")]
    public int Group =0;
    [Header("追従するTransformを格納してください")]
    public Transform Pivot;
    [Header("RPが0になる前のダメージ倍率")]
    public float UnRPBreakRate = 0.2f;
    [Header("RPが0になった時のダメージ倍率")]
    public float RPBreakRate =2f;
    //当たった攻撃のBullet.cs
    Bullet BulletScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = Pivot.position;
        this.gameObject.transform.rotation = Pivot.rotation;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 23)
        {
            BulletScript = other.GetComponent<Bullet>();
            if(BulletScript != null)
            {
                if(Group != BulletScript.Group)
                {
                    if(TargetTypeScript.RP != 0)
                    {
                        TargetTypeScript.HP -= (int)((float)BulletScript.HPDamage * UnRPBreakRate);
                    }else
                    {
                        TargetTypeScript.HP -= (int)((float)BulletScript.HPDamage * RPBreakRate);
                    }
                    TargetTypeScript.RP -= BulletScript.RPDamage;
                }
            }
        }
    }
}

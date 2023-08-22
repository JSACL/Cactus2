using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //攻撃対象。
    public GameObject Target;
    [Header("0は味方、1は敵。拡張性を鑑みてintで管理している。同じGroupによる攻撃は無効")]
    public int Group = 0;
    [Header("HPに対するダメージ")]
    public int HPDamage;
    [Header("RPに対するダメージ")]
    public int RPDamage;
    [Header("0:何も起こらない。1:直進と追尾")]
    [Header("弾丸のタイプ")]
    public int Type;
    [Header("自分が生成した攻撃である事を示す。")]
    public bool IsMyBullet;
    [Header("トリガーイベントで消える事を示す。")]
    public bool DeleteOnTrigger;
    [Header("DeleteOnTriggerがOnの時、デフォルトレイヤーでのみ消滅が起こる。")]
    public bool BreakOnlyDefault;
    [Header("一定距離に近づいたら目標にくっつく。高速の攻撃を安定させる。")]
    public bool AttachTarget;
    [Header("AttachTargetがOnの時、目標にくっつく距離")]
    public float AttachDistance;
    [HideInInspector]//AttachTargetがOnの時、目標にくっついているかどうかを示す。
    public bool Attached;
    [Header("自然消滅を行うか否か")]
    public bool NaturalDelete;
    [Header("消滅時間")]
    public float DeleteTime = 0f;
    //消滅時間の計測
    private float DeleteTimer = 0f;
    //初速と加速度から導き出される速度
    private float velocity;
    [Header("初速")]
    public float velocity_ini;
    [Header("加速度")]
    public float acceleration;
    [Header("AudioSourceを付けたゲームオブジェクトを弾丸一つ毎に用意。")]
    [Header("これらは新しく生成する方法もあるけど最初から用意しておいた方がバグが少なく、軽くしやすい。")]
    public GameObject Speaker;
    [Header("破壊時SE")]
    public AudioClip BreakSE;
    AudioSource audioSource;
    [Header("ヒットエフェクトを弾丸一つ毎に用意")]
    public GameObject HitEffect;


    void Start()
    {
        
    }
    void OnEnable()
    {
        Attached = false;
        DeleteTimer = 0f;
        if(Speaker != null)
        {
            Speaker.transform.position = this.transform.position;
        }
        velocity = velocity_ini;
    }
    void Update()
    {
        //新しく挙動を記述する場合はここに
        switch(Type)
        {
            case 0:
                break;
            case 1:
                Missile();
                break;
            default:
                break;
        }
        if(NaturalDelete)
        {
            DeleteDecision();
        }
    }
    void DeleteDecision()
    {
        DeleteTimer += Time.deltaTime;
        if(DeleteTimer>DeleteTime)
        {
            this.gameObject.SetActive(false);
        }
    }
    void Missile()
    {
        //初期化忘れるな
        if(Target != null)
        {
            this.gameObject.transform.LookAt(Target.transform.position);
        }
        velocity += acceleration * Time.deltaTime;
        if(acceleration != 0f)
        {
            this.gameObject.transform.position += this.gameObject.transform.forward * (((velocity * velocity) - (velocity_ini*velocity_ini))/(2f*acceleration));
        }else
        {
            this.gameObject.transform.position += this.gameObject.transform.forward * (velocity* Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(DeleteOnTrigger)
        {
            if(!BreakOnlyDefault)
            {
                if(other.gameObject.GetComponent<TargetType>()!= null)
                {
                    if(other.gameObject.GetComponent<TargetType>().Group != Group)
                    {
                        this.gameObject.SetActive(false);
                        HitEffect.transform.position = this.transform.position;
                        HitEffect.SetActive(true);
                        Speaker.transform.position = this.transform.position;
                        audioSource = Speaker.GetComponent<AudioSource>();
                        audioSource.PlayOneShot(BreakSE);
                    }
                }
                if(other.gameObject.layer == 0)
                {
                    this.gameObject.SetActive(false);
                    HitEffect.transform.position = this.transform.position;
                    HitEffect.SetActive(true);
                    Speaker.transform.position = this.transform.position;
                    audioSource = Speaker.GetComponent<AudioSource>();
                    audioSource.PlayOneShot(BreakSE);
                }
            }
            else if (BreakOnlyDefault && other.gameObject.layer == 0)
            {
                this.gameObject.SetActive(false);
                HitEffect.transform.position = this.transform.position;
                HitEffect.SetActive(true);
                Speaker.transform.position = this.transform.position;
                audioSource = Speaker.GetComponent<AudioSource>();
                audioSource.PlayOneShot(BreakSE);
            }
        }
    }
}


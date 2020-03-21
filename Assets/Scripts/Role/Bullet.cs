using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹逻辑(玩家和敌方共用脚本)
/// </summary>
public class Bullet : MonoBehaviour
{
    [Tooltip("当前角色")]
    public Role role = Role.PlayerBullet;
    public float bulletSpeed = 2f;
    [Tooltip("子弹的攻击力 默认为1")]
    public int buttetATK = 1;
    [Tooltip("子弹容器")]
    public Transform bulletContainer;
    // public bool isPlayerBullet = true;   //是否是玩家的子弹
    void Start()
    {
        bulletContainer = GameObject.Find("BulletContainer").transform;
        if (bulletContainer)
        {
            gameObject.transform.SetParent(bulletContainer);
        }
    }
    public enum Role
    {
        PlayerBullet,
        EnemyBullet
    }
    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 玩家或者敌人的子弹  与地图资源交互事件
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.tag)
        {
            case "Player"://敌人子弹击中玩家 
                if (role == Role.EnemyBullet)
                {
                    Destroy(gameObject);
                    coll.GetComponent<Player>().Level -= buttetATK;
                    //判断当前坦克的生命值(等级)是否为0
                    if (coll.GetComponent<Player>().Level >= 0)
                    {
                        //没死 
                        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[7]);//播放音效
                    }
                    else
                    {
                        //死了
                        coll.SendMessage("Die");
                        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[0]);//播放音效
                    }
                }
                break;
            case "Enemy": //玩家子弹击中敌人
                if (role == Role.PlayerBullet)
                {
                    Destroy(gameObject);
                    int life =-1;
                    //判断是否是boss
                    if (coll.GetComponent<EnemyBoss>())
                    {
                        //Boss 则处理变色、更新坦克属性 处理 
                        coll.GetComponent<EnemyBoss>().CurEnemyLife -= buttetATK;
                        Debug.Log(coll.GetComponent<EnemyBoss>().CurEnemyLife);
                        coll.GetComponent<EnemyBoss>().ChangeColor();
                        life = coll.GetComponent<EnemyBoss>().CurEnemyLife;
                    }
                    else
                    {
                        coll.GetComponent<Enemy>().CurEnemyLife -= buttetATK;
                        life = coll.GetComponent<Enemy>().CurEnemyLife;
                    } 

                    //判断当前子弹能否杀死当前敌人
                    if (life > 0)
                    {
                        //没死
                        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[7]);//播放音效
                    }
                    else
                    {
                        //死了 
                        coll.SendMessage("Die", false); //不能摧毁无敌坦克
                        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[5]);//播放音效
                    }
                }
                break;
            case "PlayerBullet":    //子弹击中玩家子弹
                if (tag != coll.tag)
                {
                    Destroy(coll.gameObject);
                    Destroy(gameObject);
                }
                break;
            case "EnemyBullet":    //子弹击中敌方子弹
                if (tag != coll.tag)
                {
                    Destroy(coll.gameObject);
                    Destroy(gameObject);
                }
                break;
            case "Core":
                //鸟窝
                coll.SendMessage("CoreDestory");
                Destroy(gameObject);
                AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[5]);//播放音效 
                Debug.Log("CoreDestory");
                break;
            case "Wall":
                //砖
                Destroy(coll.gameObject);
                Destroy(gameObject);
                if (role == Role.PlayerBullet)
                {
                    AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[7]);//播放音效 
                }
                break;
            case "Rock":
                //墙
                Destroy(gameObject); 
                //穿墙逻辑 规则   1.玩家坦克达到[6,7]等级   2.敌方红色BOSS坦克可以穿墙
                if (role == Role.PlayerBullet)
                {
                    Player findPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();
                    if (findPlayer &&findPlayer.Level>=6)
                    {
                        Destroy(coll.gameObject);
                        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[5]);//播放音效 
                    }
                    else
                    {
                        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[7]);//播放音效 
                    }

                }
                else  //敌方红色BOSS坦克  ==>等价与 子弹攻击力=3
                {
                    if(buttetATK==3)
                        Destroy(coll.gameObject);
                } 
                break;
            default:
                break;
        }
    }
    
}

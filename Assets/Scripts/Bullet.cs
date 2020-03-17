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
    private void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.tag)
        {
            case "Player"://子弹击中玩家 
                if (role == Role.EnemyBullet)
                {
                    coll.SendMessage("Die"); 
                    Destroy(gameObject);
                    AudioManager.GetInstance.PlayAudioSource( AudioManager.AudioSourceType.Default,AudioManager.GetInstance.audioClip[0]);//播放音效
                }
                break;
            case "Enemy": //子弹击中敌人
                if (role == Role.PlayerBullet)
                {
                    coll.SendMessage("Die");
                    Destroy(gameObject); 
                    AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default,AudioManager.GetInstance.audioClip[3]);//播放音效
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
                AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[6]);//播放音效 
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
                if (role == Role.PlayerBullet)
                {
                    AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[7]);//播放音效 
                }
                break;
            default:
                break;
        }
    }
}

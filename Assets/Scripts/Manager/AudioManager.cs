using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourcDefault;   //普通播放器
    public AudioSource audioSourceMove;     //移动坦克播放器
    public AudioSource audioSourceBGM;     //BGM播放器

     
    [Tooltip("0.玩家死亡,1.玩家坦克移动，2.玩家坦克闲置，3.敌方坦克被摧毁，" +
        "4.发射子弹，5.敌方坦克爆炸，6.获得奖励，7.鸟窝被杀，8.受伤/与砖、墙交互" +
        "9.开局BGM")]
    public AudioClip[] audioClip;

  
    private static AudioManager m_Instance;
    public static AudioManager GetInstance
    {
        set { m_Instance = value; }
        get { return m_Instance; }
    }
    void Awake()
    {
        m_Instance = this;
    }
    // Use this for initialization
    void Start()
    {

    }
    /// <summary>
    /// 播放器类型
    /// </summary>
    public enum AudioSourceType
    {
        Default,
        Move,
        BGM
    }

    /// <summary>
    /// 播放指定音效
    /// </summary>
    /// <param name="audioSouce"></param>
    /// <param name="audioClip"></param>
    public void PlayAudioSource(AudioSourceType audioSouce, AudioClip audioClip)
    {
        switch (audioSouce)
        {
            case AudioSourceType.Default:
                if (audioSourcDefault.clip != audioClip)
                {
                    audioSourcDefault.clip = audioClip; 
                } 
                audioSourcDefault.Play();
                break;
            case AudioSourceType.Move:
                if (audioSourceMove.clip != audioClip)
                {
                    audioSourceMove.clip = audioClip; 
                }  
                audioSourceMove.Play(); 
                break;
            case AudioSourceType.BGM:
                if (audioSourceBGM.clip != audioClip)
                {
                    audioSourceBGM.clip = audioClip;
                }
                audioSourceBGM.Play();
                break;
            default:
                break;
        }
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  GameManager : MonoBehaviour
{ 
    [Header("游戏的基本属性设置")]
    [Tooltip("生产坦克的上限值")]
    public int createTankMax = 20;
    public int curEnemyTank;



    private static GameManager instance;
    public static GameManager GetInstance
    {
        set { value = instance; }
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
    }
    void Start()
    { 
        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.BGM, AudioManager.GetInstance.audioClip[8]);//播放音效 
    }

     
    // Update is called once per frame
    void Update()
    {
        UpdateData();
    }
    //更新数据
    private void UpdateData()
    {
        //更新当前敌方坦克数量
        if (curEnemyTank!= MapCreate.GetInstance.curEnemyCount)
        {
            curEnemyTank = MapCreate.GetInstance.curEnemyCount;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 奖励机制
///1.加一条生命Award_Life
///2.静止点坦克移动 Award_NotMove
///3.鸟窝周边砖块变成墙 Award_CoreRock
///4.歼灭全体坦克 Award_Bomb
///5.坦克等级提高 Award_Level
///6.坦克无敌Award_God
/// </summary>
public class Award : MonoBehaviour
{
    //当前奖励 
    public AwardType awardType;

    [Header("奖励参数设置")]
    [Tooltip("静止移动的时间")]
    public float notMoveTime = 5;
    private float m_CurNotMoveTimer = 0;
    private bool m_IsNotMove = false;
    [Tooltip("老窝变成墙的时间")]
    public float coreRockTime = 3;
    [Tooltip("无敌时间")]
    public float tankGodTime = 10;
    //[Tooltip("全屏炸弹是否能摧毁无敌状态的坦克")]
    //public   bool canBombGodTank = true;
    [Tooltip("全屏炸弹是否能摧毁无敌状态的坦克")]
    public bool canBombGodTank = true;

    [Tooltip("获得当前奖励的坦克Tag值")]
    private string getAwardTankTag;


    private GameObject m_EnemyContainer;   //场景存放敌人坦克的容器
    //private GameObject m_player;
    private static Award instance;
    public static Award GetInstance
    {
        get { return instance; }
        set { instance = value; }
    }
    /// <summary>
    /// 奖励类型
    /// </summary>
    public enum AwardType
    {
        LifeAdd,
        NoMove,
        CoreRock,
        Bomb,
        LevelAdd,
        God
    }
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        m_EnemyContainer = GameObject.Find("EnemyContainer");
    }

    /// <summary>
    /// 获得相应奖励
    /// </summary>
    private void GetAward()
    {
        switch (awardType)
        {
            case AwardType.LifeAdd:
                LifeAdd();
                break;
            case AwardType.NoMove:
                m_IsNotMove = true;
                break;
            case AwardType.CoreRock:
                //变墙
                CoreRock(1);
                //变回砖 
                StartCoroutine(WaitSomeTime(coreRockTime, () => CoreRock(2)));
                break;
            case AwardType.Bomb:
                BombAllTank();
                break;
            case AwardType.LevelAdd:
                TankLevelAdd();
                break;
            case AwardType.God:
                TankGod();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 拾取奖励
    /// </summary>
    /// <param name="coll"></param>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        getAwardTankTag = coll.gameObject.tag;
        if (getAwardTankTag == "Player")
        {
            Debug.Log("玩家获得奖励：" + awardType);
            GetAward();
            //销毁奖励道具 
            Destroy(gameObject, Mathf.Max(new float[] { notMoveTime, coreRockTime, tankGodTime }));
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.BGM, AudioManager.GetInstance.audioClip[6]);//播放音效 
        }
        //噩梦模式的敌人触碰奖励
        else if (GameManager.GetInstance.getSelectMode == SelectScene.SelectMode.Hard && getAwardTankTag == "Enemy")
        {
            Debug.Log("敌人获得奖励：" + awardType);
            //销毁奖励道具 
            Destroy(gameObject, Mathf.Max(new float[] { notMoveTime, coreRockTime, tankGodTime }));
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void Update()
    {
        //静止移动逻辑
        if (m_IsNotMove)
        {
            m_CurNotMoveTimer += Time.deltaTime;
            if (m_CurNotMoveTimer <= notMoveTime)
            {
                NotMove();
            }
            else
            {
                m_CurNotMoveTimer = 0;
                m_IsNotMove = false;
            }
        }
    }

    #region 奖励机制的具体实现 
    /// <summary>
    /// 增加一条生命 
    /// </summary>
    /// <returns></returns>
    private void LifeAdd()
    {
        PlayerManager.GetInstance.Life++;
    }
    /// <summary>
    /// 静止移动
    /// </summary>
    /// <param name="enemyNotMove">是否为敌方坦克静止移动？ 默认是</param>
    private void NotMove(bool enemyNotMove = true)
    {
        //找到全局敌方坦克
        Enemy[] tra = m_EnemyContainer.GetComponentsInChildren<Enemy>();
        foreach (Enemy item in tra)
        {
            item.canAutoMove = false;
        }
    }
    //鸟巢加固 1-墙 2-砖
    private void CoreRock(int index)
    {
        MapCreate.GetInstance.CoreBorder(index);
    }
    //炸掉场上所有（敌人）坦克
    private void BombAllTank()
    {
        //找到全局敌方坦克
        for (int i = 0; i < m_EnemyContainer.transform.childCount; i++)
        {
            m_EnemyContainer.transform.GetChild(i).gameObject.SendMessage("Die", canBombGodTank);//能摧毁无敌坦克
        }
    }
    //坦克等级提高  共有[0,7]级
    private void TankLevelAdd()
    {
        GameObject m_player = GameObject.FindWithTag("Player");
        if (m_player)
        {
            if (m_player.GetComponent<Player>().Level < 7)
                m_player.GetComponent<Player>().Level++;
            else
                LifeAdd();
        }
    }
    //坦克无敌 
    private void TankGod()
    {
        GameObject m_player = GameObject.FindWithTag("Player");
        if (m_player)
        {
            m_player.GetComponent<Player>().IsGod = true;
            m_player.GetComponent<Player>().godTime = tankGodTime;
            m_player.GetComponent<Player>().TankGod(ref tankGodTime);
        }
    }
    #endregion




    IEnumerator WaitSomeTime(float time, UnityAction action)
    {
        yield return new WaitForSeconds(time);
        if (action != null)
        {
            action.Invoke();
        }
    }

}


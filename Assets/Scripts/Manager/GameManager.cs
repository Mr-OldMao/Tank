using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏管理器
/// 1.用于配置游戏和核心数据
/// 2.接收玩家选择的模式
/// 3.游戏结束逻辑 
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("游戏的基本属性设置")]
    //[Tooltip("生产坦克的上限值")]
    //public int createTankMax = 20;
    //public int curEnemyTank; 

    //属性
    [Tooltip("鸟窝爆炸图片")]
    public Sprite eff_CoreDestory;
    [Tooltip("鸟窝爆炸特效")]
    public GameObject go_Destory;


    //接收SelectScene场景中的选择数据
    public SelectScene.SelectMode getSelectMode;

    //游戏结束？
    private bool m_IsGameOver = false;
    private GameMode gameModeScript;

    public bool isGameOver
    {
        get { return m_IsGameOver; }
    }

    private static GameManager instance;
    public static GameManager GetInstance
    {
        set { value = instance; }
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
        gameModeScript = GetComponent<GameMode>();
    }



    void Start()
    {
        getSelectMode = SelectScene.selectType;
        Debug.Log("GM:" + getSelectMode);
        LoadSelectPattern();

        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.BGM, AudioManager.GetInstance.audioClip[8]);//播放音效 
    }

    /// <summary>
    /// 加载玩家所选择的模式
    /// </summary>
    private void LoadSelectPattern()
    {
        switch (getSelectMode)
        {
            case SelectScene.SelectMode.Normal:
                gameModeScript.NormalMode();
                break;
            case SelectScene.SelectMode.Hard:
                gameModeScript.HardMode();
                break;
            case SelectScene.SelectMode.NoEnd:
                gameModeScript.NotEndMode();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 结束游戏
    /// 1.生命已耗尽 or
    /// 2.鸟窝已爆炸 or
    /// 3.时间已规0
    /// </summary>
    public void IsGameOvoer(GameOverType gameOverType)
    {
        m_IsGameOver = true;
        UIManager.isGameOver = true;
        switch (gameOverType)
        {
            case GameOverType.notLife:
                UIManager.GetInstance.txt_GameOverCause.text = "死因:没有剩余生命";
                break;
            case GameOverType.coreDestory:
                try
                {
                    GameObject core = GameObject.FindWithTag("Core");
                    Debug.Log(core);
                    core.GetComponent<SpriteRenderer>().sprite = eff_CoreDestory;
                    Instantiate(go_Destory, core.transform.position, transform.rotation);
                }
                catch (System.Exception)
                {
                    throw;
                }
                UIManager.GetInstance.txt_GameOverCause.text = "死因:鸟窝被毁";
                break;
            case GameOverType.notTime:
                UIManager.GetInstance.txt_GameOverCause.text = " 死因:规定时间内未歼灭敌人";
                break;
            default:
                break;
        }
    }

   

    /// <summary>
    /// 游戏结束原因
    /// </summary>
    public enum GameOverType
    {
        /// <summary>
        /// 没有剩余生命
        /// </summary>
        notLife,
        /// <summary>
        /// 鸟窝被毁
        /// </summary>
        coreDestory,
        /// <summary>
        /// 无剩余时间
        /// </summary>
        notTime
    }

}

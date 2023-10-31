using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家管理器
/// 玩家重生：玩家管理器-》玩家出生特效-》玩家实体
/// </summary>
public class PlayerManager : MonoBehaviour
{
    //属性值
    [Tooltip("生命条数")]
    [SerializeField]
    private int m_Life; 
    public int Life
    {
        get { return m_Life; }
        set { m_Life = value; }
    }
    public int score = 0;
    public bool isDie = false;                //是否死亡 
    //实体
    public GameObject bornPlayer;            //玩家出生


    private static PlayerManager m_Instance ;
    public static PlayerManager GetInstance
    {
        get { return m_Instance; }
        set { m_Instance = value; }
    }

    void Awake()
    {
        m_Life = CoreData.playerLife;
        m_Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    { 
        if (isDie)
        {
            BornAgain();
        }
    }

    //重生
    private void BornAgain()
    {
        if (m_Life <= 0)
        {
            //游戏结束
            GameManager.GetInstance.IsGameOvoer(GameManager.GameOverType.notLife); 
        }
        else
        { 
            //重生
            m_Life--;
            GameObject bornPlayerIns = Instantiate(bornPlayer, new Vector3(-2, -8, 0), Quaternion.identity);
            bornPlayerIns.GetComponent<Born>().isCreatePlayer = true;
            isDie = false;
        }
    }

}

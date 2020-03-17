using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家管理器
/// </summary>
public class PlayerManager : MonoBehaviour
{
    //属性值
    [Tooltip("生命")]
    public int lifeValue = 3;
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
        if (lifeValue <= 0)
        {
            //游戏结束
            UIManager.isGameOver = true;
        }
        else
        { 
            //重生
            lifeValue--;
            GameObject bornPlayerIns = Instantiate(bornPlayer, new Vector3(-2, -8, 0), Quaternion.identity);
            bornPlayerIns.GetComponent<Born>().isCreatePlayer = true;
            isDie = false;
        }
    }

}

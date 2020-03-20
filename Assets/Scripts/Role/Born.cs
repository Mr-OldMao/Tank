using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 产生玩家或敌人 
/// 共八种坦克
/// 1.基本坦克 红2 白2
/// 2.Boss坦克 红 绿 黄 白
/// 概率
/// 70%产生基本坦克 30% 产声Boss坦克
/// 基本坦克中 两红两白各25%
/// Boss坦克中 各个颜色25%概率
/// </summary>
public class Born : MonoBehaviour
{
    [Tooltip("玩家坦克实例")]
    public GameObject prefab_PlayerTank;
    [Tooltip("若干敌人坦克实例")]
    public GameObject[] prefab_EnemyTanks;
    [Tooltip("默认当前脚本产生敌人，修改为true则产生玩家")]
    public bool isCreatePlayer = false; 
    public GameObject enemyContainer; //存放地方坦克的容器
    // Use this for initialization
    void Start()
    {
        enemyContainer = GameObject.Find("EnemyContainer");
        Invoke("BornTank", 1f);
    }
    
    /// <summary>
    /// 概率
    /// </summary>
    /// <param name="idds">0~1</param>
    private bool Odds(float idds)
    {
        bool result = false;
        if (idds < 0 || idds > 1) return result;
        float num= Random.Range(0f, 1f);
        if (num >= idds) result = true;
        else result = false;
        return result;
    }

    /// <summary>
    /// 产生玩家或者敌人
    /// </summary>
    private void BornTank()
    {
        if (isCreatePlayer)
        {
            //产生玩家坦克
            Instantiate(prefab_PlayerTank, transform.position, transform.rotation);
            //销毁出生点
            Destroy(gameObject); 
        }
        else
        {
            //产生敌方坦克
            //GameObject ins =Instantiate(prefab_EnemyTanks[Random.Range(0, prefab_EnemyTanks.Length)], transform.position, transform.rotation); 
            GameObject ins = null;
            //70%产生基本坦克 30% 产声Boss坦克
            if (Odds(0.7f))
            {
                //普通
                //普通白
                if (Odds(0.5f))
                {
                    //普通白1
                    if (Odds(0.5f))
                        ins = Instantiate(prefab_EnemyTanks[0], transform.position, transform.rotation);
                    //普通白2
                    else
                        ins = Instantiate(prefab_EnemyTanks[2], transform.position, transform.rotation);
                }
                //普通红
                else
                {
                    //普通红1
                    if (Odds(0.5f))
                        ins = Instantiate(prefab_EnemyTanks[1], transform.position, transform.rotation);
                    //普通红2
                    else
                        ins = Instantiate(prefab_EnemyTanks[3], transform.position, transform.rotation);
                }
            }
            else
            {
                //Boss
                ins = Instantiate(prefab_EnemyTanks[4], transform.position, transform.rotation);
                if (Odds(0.5f))
                {
                    //BOSS红
                    if (Odds(0.5f))
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.red;
                    //BOSS绿
                    else
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.green;
                } 
                else
                {
                    //BOSS黄
                    if (Odds(0.5f))
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.yellow;   
                    //BOSS白
                    else
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.white;
                }
            }
             
            //放入容器
            if (enemyContainer)
                ins.transform.SetParent(enemyContainer.transform);
        } 
    }
}

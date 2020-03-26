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
    private GameObject m_PlayerTankClone;
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
        float num = Random.Range(0f, 1f);
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
            if (GameObject.FindWithTag("Player"))
            {
                Destroy(GameObject.FindWithTag("Player"));
                //产生玩家坦克
                m_PlayerTankClone = Instantiate(prefab_PlayerTank, transform.position, transform.rotation);
            }
            else
            {
                //产生玩家坦克
                m_PlayerTankClone = Instantiate(prefab_PlayerTank, transform.position, transform.rotation);
            }
            //销毁出生点
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("产生敌人中----BornTank()");
            //产生敌方坦克
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
                    {
                        ins = Instantiate(prefab_EnemyTanks[0], transform.position, transform.rotation);
                        InitEnemyTankProperty(ins, false, CoreData.enemy_NormalWhite1_Life,
                            CoreData.enemy_NormalWhite1_MoveSpeed, CoreData.enemy_NormalWhite1_AutoMoveHZ,
                            CoreData.enemy_NormalWhite1_AutoMoveTime, CoreData.enemy_NormalWhite1_BulletAtk,
                            CoreData.enemy_NormalWhite1_AutoAttackHZ, CoreData.enemy_NormalWhite1_GodTimer);
                    }

                    //普通白3
                    else
                    {
                        ins = Instantiate(prefab_EnemyTanks[2], transform.position, transform.rotation);
                        InitEnemyTankProperty(ins, false, CoreData.enemy_NormalWhite3_Life,
                           CoreData.enemy_NormalWhite3_MoveSpeed, CoreData.enemy_NormalWhite3_AutoMoveHZ,
                           CoreData.enemy_NormalWhite3_AutoMoveTime, CoreData.enemy_NormalWhite3_BulletAtk,
                           CoreData.enemy_NormalWhite3_AutoAttackHZ, CoreData.enemy_NormalWhite3_GodTimer);
                    }
                }
                //普通红
                else
                {
                    //普通红2
                    if (Odds(0.5f))
                    {
                        ins = Instantiate(prefab_EnemyTanks[1], transform.position, transform.rotation);
                        InitEnemyTankProperty(ins, false, CoreData.enemy_NormalRed2_Life,
                           CoreData.enemy_NormalRed2_MoveSpeed, CoreData.enemy_NormalRed2_AutoMoveHZ,
                           CoreData.enemy_NormalRed2_AutoMoveTime, CoreData.enemy_NormalRed2_BulletAtk,
                           CoreData.enemy_NormalRed2_AutoAttackHZ, CoreData.enemy_NormalRed2_GodTimer);
                    }
                    //普通红4
                    else
                    {
                        ins = Instantiate(prefab_EnemyTanks[3], transform.position, transform.rotation);
                        InitEnemyTankProperty(ins, false, CoreData.enemy_NormalRed4_Life,
                           CoreData.enemy_NormalRed4_MoveSpeed, CoreData.enemy_NormalRed4_AutoMoveHZ,
                           CoreData.enemy_NormalRed4_AutoMoveTime, CoreData.enemy_NormalRed4_BulletAtk,
                           CoreData.enemy_NormalRed4_AutoAttackHZ, CoreData.enemy_NormalRed4_GodTimer);
                    }
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
                    {
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.red;
                        InitEnemyTankProperty(ins, true, CoreData.enemy_BossRed_Life,
                           CoreData.enemy_BossRed_MoveSpeed, CoreData.enemy_BossRed_AutoMoveHZ,
                           CoreData.enemy_BossRed_AutoMoveTime, CoreData.enemy_BossRed_BulletAtk,
                           CoreData.enemy_BossRed_AutoAttackHZ, CoreData.enemy_BossRed_GodTimer);
                    }
                    //BOSS绿
                    else
                    {
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.green;
                        InitEnemyTankProperty(ins, true, CoreData.enemy_BossGreen_Life,
                           CoreData.enemy_BossGreen_MoveSpeed, CoreData.enemy_BossGreen_AutoMoveHZ,
                           CoreData.enemy_BossGreen_AutoMoveTime, CoreData.enemy_BossGreen_BulletAtk,
                           CoreData.enemy_BossGreen_AutoAttackHZ, CoreData.enemy_BossGreen_GodTimer);
                    }
                }
                else
                {
                    //BOSS黄
                    if (Odds(0.5f))
                    {
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.yellow;
                        InitEnemyTankProperty(ins, true, CoreData.enemy_BossYellow_Life,
                           CoreData.enemy_BossYellow_MoveSpeed, CoreData.enemy_BossYellow_AutoMoveHZ,
                           CoreData.enemy_BossYellow_AutoMoveTime, CoreData.enemy_BossYellow_BulletAtk,
                          CoreData.enemy_BossYellow_AutoAttackHZ, CoreData.enemy_BossYellow_GodTimer);
                    }
                    //BOSS白
                    else
                    {
                        ins.GetComponent<EnemyBoss>().enemyColorType = EnemyBoss.EnemyColorType.white;
                        InitEnemyTankProperty(ins, true, CoreData.enemy_BossWhite_Life,
                          CoreData.enemy_BossWhite_MoveSpeed, CoreData.enemy_BossWhite_AutoMoveHZ,
                          CoreData.enemy_BossWhite_AutoMoveTime, CoreData.enemy_BossWhite_BulletAtk,
                          CoreData.enemy_BossWhite_AutoAttackHZ, CoreData.enemy_BossWhite_GodTimer);
                    }
                }
            }
            //放入容器
            if (enemyContainer)
                ins.transform.SetParent(enemyContainer.transform);
        }
        Destroy(gameObject);
    }
    /// <summary>
    /// 配置敌人属性
    /// </summary>
    /// <param name="enemy">敌方坦克</param>
    /// <param name="isBossTank">是否为BOSS坦克</param>
    /// <param name="Life">生命值</param>
    /// <param name="speed">移速</param>
    /// <param name="autoMoveHZ">自动移动的频率  (秒/次)</param>
    /// <param name="autoMoveTim">每次移动的时长  (秒)</param>
    /// <param name="bulletAtk">子弹攻击力</param>
    /// <param name="autoAttackHZ">攻击频率 (秒/次)</param>
    /// <param name="godTimer">无敌时长 (秒)</param>
    private void InitEnemyTankProperty(GameObject enemy, bool isBossTank,
        int Life, float moveSpeed, float autoMoveHZ, float autoMoveTime, int bulletAtk
        , float autoAttackHZ, float godTimer)
    {
        if (isBossTank)
        {
            //BOSS
            EnemyBoss enemyBoss = enemy.GetComponent<EnemyBoss>();
            if (enemyBoss == null)
                Debug.LogError("未找到EnemyBoss脚本");
            else
            {
                enemyBoss.CurEnemyLife = Life;
                enemyBoss.moveSpeed = moveSpeed;
                enemyBoss.autoAttackHZ = autoMoveHZ;
                enemyBoss.autoMoveTime = autoMoveTime;
                enemyBoss.bulletAtk = bulletAtk;
                enemyBoss.autoAttackHZ = autoAttackHZ;
                enemyBoss.godTimer = godTimer;
            }
        }
        else
        {
            Enemy enemyNormal = enemy.GetComponent<Enemy>();
            if (enemyNormal == null)
                Debug.LogError("未找到Enemy脚本");
            else
            {
                enemyNormal.CurEnemyLife = Life;
                enemyNormal.moveSpeed = moveSpeed;
                enemyNormal.autoAttackHZ = autoMoveHZ;
                enemyNormal.autoMoveTime = autoMoveTime;
                enemyNormal.bulletAtk = bulletAtk;
                enemyNormal.autoAttackHZ = autoAttackHZ;
                enemyNormal.godTimer = godTimer;
            }
        }
    }
}

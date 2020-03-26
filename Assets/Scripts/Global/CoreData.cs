using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏的核心数据 3.0
/// 主要用于游戏的初始化
/// </summary>
public class CoreData : MonoBehaviour
{
    #region 其他
    /// <summary>
    /// 生成敌人的频率 秒/个
    /// </summary>
    public static float createEnemyTimer = 3f;

    /// <summary>
    /// 地图中允许敌人共同存在的上限 
    /// </summary>
    public static int  enemyMaxCount = 20;
    ///// <summary>
    ///// 到达地图场上敌军上限后,摧毁敌人还是否可以继续生产敌人
    ///// 普通、噩梦模式为true  
    ///// </summary>
    //public static bool isGetMaxCountContinueCreateEnemy = true;
    /// <summary>
    /// 本关需要产生敌人的数量
    /// </summary>
    public static int needCreateEnemyCount = -1;

    /// <summary>
    /// 起始关卡
    /// </summary>
    public static int startLevelNum = 1;
    #endregion

    #region 玩家属性
    /// <summary>
    /// 玩家初始化生命
    /// </summary>
    public static int playerLife = 3;
    /// <summary>
    ///  玩家初始化移速
    /// </summary>
    public static float playerMoveSpeed = 2f;
    /// <summary>
    ///  玩家初始化攻击频率 (秒/次)
    /// </summary>
    public static float playerAttackHZ = 0.2f;
    /// <summary>
    ///  玩家初始化等级
    /// </summary>
    public static int playerLevel = 0;
    /// <summary>
    ///  玩家初始化出生时无敌时间
    /// </summary>
    public static float playerGodTime = 3f;
    #endregion

    #region 敌方属性 
    #region 敌方 白色普通坦克Enemy1
    /// <summary>
    /// 生命值
    /// </summary>
    public static int enemy_NormalWhite1_Life = 1;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_NormalWhite1_MoveSpeed = 1f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_NormalWhite1_AutoMoveHZ = 6f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_NormalWhite1_AutoMoveTime = 2f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_NormalWhite1_BulletAtk = 1;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_NormalWhite1_AutoAttackHZ =2f;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_NormalWhite1_GodTimer = 2f;
    #endregion

    #region 敌方 红色普通坦克Enemy2
    /// <summary>
    /// 生命值
    /// </summary>
    public static int enemy_NormalRed2_Life = 2;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_NormalRed2_MoveSpeed = 1f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_NormalRed2_AutoMoveHZ = 6f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_NormalRed2_AutoMoveTime = 4f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_NormalRed2_BulletAtk = 1;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_NormalRed2_AutoAttackHZ = 2f;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_NormalRed2_GodTimer = 2f;
    #endregion

    #region 敌方 白色普通坦克Enemy3
    /// <summary>
    /// 生命值
    /// </summary>
    public static int enemy_NormalWhite3_Life = 1;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_NormalWhite3_MoveSpeed = 2f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_NormalWhite3_AutoMoveHZ = 4f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_NormalWhite3_AutoMoveTime = 1f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_NormalWhite3_BulletAtk = 1;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_NormalWhite3_AutoAttackHZ = 2f;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_NormalWhite3_GodTimer = 2f;


    #endregion

    #region 敌方 红色普通坦克Enemy4
    /// <summary>
    /// 生命值
    /// </summary>
    public static int enemy_NormalRed4_Life = 2;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_NormalRed4_MoveSpeed = 2f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_NormalRed4_AutoMoveHZ = 3f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_NormalRed4_AutoMoveTime = 1.5f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_NormalRed4_BulletAtk = 1;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_NormalRed4_AutoAttackHZ = 2f;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_NormalRed4_GodTimer = 2f;
    #endregion

    #region 敌方 红Boss坦克
    /// <summary>
    /// 红色Boss坦克生命值
    /// </summary>
    public static int enemy_BossRed_Life = 8;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_BossRed_MoveSpeed = 1.5f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_BossRed_AutoMoveHZ = 5f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_BossRed_AutoMoveTime = 2f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_BossRed_BulletAtk = 3;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_BossRed_AutoAttackHZ = 1.5f;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_BossRed_GodTimer = 2f;
    #endregion

    #region 敌方 绿Boss坦克
    /// <summary>
    /// 绿色Boss坦克生命值
    /// </summary>
    public static int enemy_BossGreen_Life = 6;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_BossGreen_MoveSpeed = 1f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_BossGreen_AutoMoveHZ = 6f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_BossGreen_AutoMoveTime = 3f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_BossGreen_BulletAtk = 2;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_BossGreen_AutoAttackHZ = 1.5f;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_BossGreen_GodTimer = 2f;
    #endregion

    #region 敌方 黄Boss坦克
    /// <summary>
    /// 黄色Boss坦克生命值
    /// </summary>
    public static int enemy_BossYellow_Life = 3;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_BossYellow_MoveSpeed = 1f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_BossYellow_AutoMoveHZ = 4f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_BossYellow_AutoMoveTime = 2f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_BossYellow_BulletAtk = 2;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_BossYellow_AutoAttackHZ = 2f;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_BossYellow_GodTimer = 2f;
    #endregion

    #region 敌方 白Boss坦克
    /// <summary>
    /// 白色Boss坦克生命值
    /// </summary>
    public static int enemy_BossWhite_Life = 1;
    /// <summary>
    /// 移速
    /// </summary>
    public static float enemy_BossWhite_MoveSpeed = 1f;
    /// <summary>
    /// 自动移动的频率  (秒/次)
    /// </summary>
    public static float enemy_BossWhite_AutoMoveHZ = 3f;
    /// <summary>
    /// 每次移动的时长  (秒)
    /// </summary>
    public static float enemy_BossWhite_AutoMoveTime = 2f;
    /// <summary>
    /// 子弹攻击力
    /// </summary>
    public static int enemy_BossWhite_BulletAtk = 1;
    /// <summary>
    /// 攻击频率 (秒/次)
    /// </summary>
    public static float enemy_BossWhite_AutoAttackHZ = 2;
    /// <summary>
    /// 无敌时长 (秒)
    /// </summary>
    public static float enemy_BossWhite_GodTimer = 2f;
    #endregion
    #endregion 
}

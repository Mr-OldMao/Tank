using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 三种游戏模式 3.0
/// </summary>
public class GameMode : MonoBehaviour
{
    /// <summary>
    /// 当前游戏关数
    /// </summary>
    public int curLevel = 1;
    //通关
    public static bool passCurLevel = false;
    //是否开始监听 通关判定
    public static bool isListenPassCurLevel = true;
    //无尽模式计时器  每过多少秒等价于通关一次游戏
    private float NormalModeTimer = 30f;



    void Start()
    {
        InvokeRepeating("PassCurLevel", 2f, 2f);
    }
    void Update()
    {
        //无尽模式   随着时间增长-》curLevel值增大
        if (GameManager.GetInstance.getSelectMode == SelectScene.SelectMode.NoEnd)
        {
            NormalModeTimer -= Time.deltaTime;
            if (NormalModeTimer <= 0)
            {
                NormalModeTimer = 30f;
                curLevel++;
                Debug.Log("无尽模式当前level："+ curLevel);
           
            //敌人生成频率 
            CoreData.createEnemyTimer =
                (CoreData.createEnemyTimer - 0.4f * (curLevel - 1) >= 0.5f) ?
                CoreData.createEnemyTimer -= 0.4f * (curLevel - 1) : CoreData.createEnemyTimer = 0.4f;
            //敌人初始化属性变更   形参与【普通模式】相同
            ChangeEnemyProperty(5, 10, 0.1f, 0.1f, 10, 0.02f, 0.1f);
            }
        }
    }
    //通关判断
    private void PassCurLevel()
    {
        if (!passCurLevel && isListenPassCurLevel)
        {
            //当前场上敌人剩余个数为0  &&  本关累计产生敌人个数=本关需要产生的敌人个数
            if (MapCreate.GetInstance.curEnemyCount == 0 && MapCreate.GetInstance.addEnemyCount == CoreData.needCreateEnemyCount)
            {
                //通关
                passCurLevel = true;
                curLevel++;
                Debug.Log("已通过第" + curLevel + "关");
                if (GameManager.GetInstance.getSelectMode == SelectScene.SelectMode.Normal)
                {
                    NormalMode();
                }
                else
                {
                    HardMode();
                }
                Debug.Log("已载入第" + curLevel + "关");
            }
        }
    }
    /// <summary>
    /// 普通模式
    /// </summary>
    public void NormalMode()
    {
        //关卡UI显示
        UIManager.GetInstance.ShowLevelUI(curLevel);
        //倒计时UI显示
        UIManager.GetInstance.ShowTimerUI(150,true);
        //设置规则  
        //敌军数量
        CoreData.needCreateEnemyCount = curLevel * 5;
        //敌人生成频率 
        CoreData.createEnemyTimer =
            (CoreData.createEnemyTimer - 0.4f * (curLevel - 1) >= 0.5f) ?
            CoreData.createEnemyTimer -= 0.4f * (curLevel - 1) : CoreData.createEnemyTimer = 0.4f;
        //通关后逻辑
        if (passCurLevel)
        {
            passCurLevel = false;
            //进入下一关
            //清除当前玩家  ---或者初始化位置位置
            try
            {
                Destroy(GameObject.FindWithTag("Player"));
            }
            catch (System.Exception)
            {
                throw;
            }
            //销毁场上所有子弹
            GameObject bulletContainer;
            try
            {
                bulletContainer = GameObject.Find("BulletContainer");
                for (int i = 0; i < bulletContainer.transform.childCount; i++)
                    Destroy(bulletContainer.transform.GetChild(i).gameObject);
            }
            catch (System.Exception)
            {
                throw;
            }

            //敌人初始化属性变更
            ChangeEnemyProperty(5, 10, 0.1f, 0.1f, 10, 0.02f, 0.1f);

            //敌人红白坦克的出现概率 
            // TODO~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //重新铺设地图 
            MapCreate.GetInstance.ReInit();
            //背景音乐
            AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.BGM, AudioManager.GetInstance.audioClip[8]);//播放音效 
            //玩家生命+1
            PlayerManager.GetInstance.Life++;
            
        }
    }
    public void HardMode()
    {
        //关卡UI显示
        UIManager.GetInstance.ShowLevelUI(curLevel);
        //倒计时UI显示
        UIManager.GetInstance.ShowTimerUI(120, true);
        //设置规则  
        //敌军数量 
        CoreData.needCreateEnemyCount = curLevel * 10;
        //敌人生成频率 
        CoreData.createEnemyTimer =
            (CoreData.createEnemyTimer - 0.4f * (curLevel - 1) >= 0.5f) ?
            CoreData.createEnemyTimer -= 0.4f * (curLevel - 1) : CoreData.createEnemyTimer = 0.4f;
        //通关后逻辑
        if (passCurLevel)
        {
            passCurLevel = false;
            //进入下一关   
            //销毁场上所有奖励
            GameObject awardContainer;
            try
            {
                awardContainer = GameObject.Find("AwardContainer");
                for (int i = 0; i < awardContainer.transform.childCount; i++)
                    Destroy(awardContainer.transform.GetChild(i).gameObject);
            }
            catch (System.Exception)
            {

                throw;
            }
            //销毁场上所有子弹
            GameObject bulletContainer;
            try
            {
                bulletContainer = GameObject.Find("BulletContainer");
                for (int i = 0; i < bulletContainer.transform.childCount; i++)
                    Destroy(bulletContainer.transform.GetChild(i).gameObject);
            }
            catch (System.Exception)
            {
                throw;
            }
            //清除当前玩家  ---或者初始化位置位置
            try
            {
                Destroy(GameObject.FindWithTag("Player"));
            }
            catch (System.Exception)
            {
                throw;
            }
            //敌人初始化属性变更
            ChangeEnemyProperty(int.MaxValue, 20, 0.15F, 0.1F, 5, 0.05F, 0.2F);
            //重新铺设地图 
            MapCreate.GetInstance.ReInit();
            //背景音乐
            AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.BGM, AudioManager.GetInstance.audioClip[8]);//播放音效 
            //玩家生命+1
            PlayerManager.GetInstance.Life++;
        }
    }
    public void NotEndMode()
    {
        //通关UI显示
        UIManager.GetInstance.ShowNotEndUI();
        //正计时UI显示
        UIManager.GetInstance.ShowTimerUI(0, false);
        //设置规则  
        //敌军数量 
        CoreData.needCreateEnemyCount = int.MaxValue;
    }



    /// <summary>
    /// 改变（加强）敌人属性 每种属性都有相应的算法 
    /// 每次升级调用一次
    /// </summary>
    /// <param name="LifeNum">多少关加一次生命 推荐5, 普通模式生命值上限10  噩梦生命值上限无</param>
    /// <param name="LifeMax">可以加多少次 推荐10</param>
    /// <param name="MoveSpeedNum">每过一关加多少移速 推荐0.1，  移速上限：3</param>
    /// <param name="AutoMoveHZNum">每过一关加多少自动移动的频率 推荐0.1f  移动频率上限：2</param>
    /// <param name="BulletAtkNum">多少关加一次子弹攻击力 推荐10  攻击力上限：普通坦克4，boss坦克8</param>
    /// <param name="AutoATKHZNum">每过一关加多少攻击频率 推荐0.02  攻击频率上限：1</param>
    /// <param name="GodTimerNum">每过一关加多少无敌时间 推荐0.1  无敌时间上限：5</param>
    public void ChangeEnemyProperty(int LifeNum, int LifeMax, float MoveSpeedNum,
        float AutoMoveHZNum, int BulletAtkNum, float AutoATKHZNum, float GodTimerNum)
    {
        #region 生命值
        //规则:生命值 每五级+1      普通模式上限10  噩梦生命值上限无
        //算法:(第一次初始化的生命 + 当前关卡数/5 > 第一次初始化的生命 + lifeMax)？第一次初始化的生命 + 当前关卡数/5 : 第一次初始化的生命 + lifeMax
        int lifeNum = LifeNum; //每五关加一次      加了 curLevel/lifeNum  生命 
        int lifeMax = LifeMax;//新增生命上限      生命上限: 第一次初始化的生命 + lifeMax
        if (curLevel % lifeNum == 0)
        {
            CoreData.enemy_NormalWhite1_Life = (1 + curLevel / lifeNum > 1 + lifeMax) ? 1 + curLevel / lifeNum : 1 + lifeMax;
            CoreData.enemy_NormalRed2_Life = (1 + curLevel / lifeNum > 1 + lifeMax) ? 1 + curLevel / lifeNum : 1 + lifeMax;
            CoreData.enemy_NormalWhite3_Life = (1 + curLevel / lifeNum > 1 + lifeMax) ? 1 + curLevel / lifeNum : 1 + lifeMax;
            CoreData.enemy_NormalRed4_Life = (1 + curLevel / lifeNum > 1 + lifeMax) ? 1 + curLevel / lifeNum : 1 + lifeMax;
            CoreData.enemy_BossRed_Life = (8 + curLevel / lifeNum > 1 + lifeMax) ? 8 + curLevel / lifeNum : 1 + lifeMax;
            CoreData.enemy_BossGreen_Life = (6 + curLevel / lifeNum > 1 + lifeMax) ? 6 + curLevel / lifeNum : 1 + lifeMax;
            CoreData.enemy_BossYellow_Life = (3 + curLevel / lifeNum > 1 + lifeMax) ? 3 + curLevel / lifeNum : 1 + lifeMax;
            CoreData.enemy_BossWhite_Life = (1 + curLevel / lifeNum > 1 + lifeMax) ? 1 + curLevel / lifeNum : 1 + lifeMax;
        }

        #endregion

        #region 移速
        //规则：每过一关加移速+n(考虑上限)
        //算法：当前移速 =  上一次的移速+ (当前关卡数-1)*0.1f  (当前移速 + 0.1f < 3 ) ？当前移速 + 0.1f :3 
        float moveSpeedNum = MoveSpeedNum;
        CoreData.enemy_NormalWhite1_MoveSpeed =
            (CoreData.enemy_NormalWhite1_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_NormalWhite1_MoveSpeed + moveSpeedNum : 3;
        CoreData.enemy_NormalRed2_MoveSpeed =
           (CoreData.enemy_NormalRed2_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_NormalRed2_MoveSpeed + moveSpeedNum : 3;
        CoreData.enemy_NormalWhite3_MoveSpeed =
            (CoreData.enemy_NormalWhite3_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_NormalWhite3_MoveSpeed + moveSpeedNum : 3;
        CoreData.enemy_NormalRed4_MoveSpeed =
            (CoreData.enemy_NormalRed4_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_NormalRed4_MoveSpeed + moveSpeedNum : 3;
        CoreData.enemy_BossRed_MoveSpeed =
            (CoreData.enemy_BossRed_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_BossRed_MoveSpeed + moveSpeedNum : 3;
        CoreData.enemy_BossGreen_MoveSpeed =
            (CoreData.enemy_BossGreen_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_BossGreen_MoveSpeed + moveSpeedNum : 3;
        CoreData.enemy_BossYellow_MoveSpeed =
            (CoreData.enemy_BossYellow_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_BossYellow_MoveSpeed + moveSpeedNum : 3;
        CoreData.enemy_BossWhite_MoveSpeed =
            (CoreData.enemy_BossWhite_MoveSpeed + moveSpeedNum < 3) ? CoreData.enemy_BossWhite_MoveSpeed + moveSpeedNum : 3;
        #endregion

        #region 自动移动的频率  (秒/次)
        //规则：每过一关减去一定触发时间n（上限2秒/1次）
        //算法：当前自动移动的频率 = (上一次自动移动的频率 - 0.1f > 2 ) ？当前自动移动的频率 - 0.1f :2  
        float autoMoveHZNum = AutoMoveHZNum;
        CoreData.enemy_NormalWhite1_AutoMoveHZ =
            (CoreData.enemy_NormalWhite1_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_NormalWhite1_AutoMoveHZ - autoMoveHZNum : 2f;
        CoreData.enemy_NormalRed2_AutoMoveHZ =
            (CoreData.enemy_NormalRed2_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_NormalRed2_AutoMoveHZ - autoMoveHZNum : 2f;
        CoreData.enemy_NormalWhite3_AutoMoveHZ =
            (CoreData.enemy_NormalWhite3_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_NormalWhite3_AutoMoveHZ - autoMoveHZNum : 2f;
        CoreData.enemy_NormalRed4_AutoMoveHZ =
            (CoreData.enemy_NormalRed4_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_NormalRed4_AutoMoveHZ - autoMoveHZNum : 2f;
        CoreData.enemy_BossRed_AutoMoveHZ =
            (CoreData.enemy_BossRed_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_BossRed_AutoMoveHZ - autoMoveHZNum : 2f;
        CoreData.enemy_BossGreen_AutoMoveHZ =
            (CoreData.enemy_BossGreen_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_BossGreen_AutoMoveHZ - autoMoveHZNum : 2f;
        CoreData.enemy_BossYellow_AutoMoveHZ =
            (CoreData.enemy_BossYellow_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_BossYellow_AutoMoveHZ - autoMoveHZNum : 2f;
        CoreData.enemy_BossWhite_AutoMoveHZ =
            (CoreData.enemy_BossWhite_AutoMoveHZ - 0.1f > 2f) ? CoreData.enemy_BossWhite_AutoMoveHZ - autoMoveHZNum : 2f;
        #endregion

        #region 每次移动的时长(秒) 被注掉 建议不改 因为改移速就够了
        ////每次移动的时长(秒)   
        ////算法： 第一次初始化的 每次移动的时长+ 每一关+0.1f
        //float autoMoveTimeNum = 0.1f;
        //CoreData.enemy_NormalWhite1_AutoMoveTime = 2f + (curLevel - 1) * autoMoveTimeNum;
        //CoreData.enemy_NormalRed2_AutoMoveTime = 4f + (curLevel - 1) * autoMoveTimeNum;
        //CoreData.enemy_NormalWhite3_AutoMoveTime = 1f + (curLevel - 1) * autoMoveTimeNum;
        //CoreData.enemy_NormalRed4_AutoMoveTime = 1.5f + (curLevel - 1) * autoMoveTimeNum;
        //CoreData.enemy_BossRed_AutoMoveTime = 2f + (curLevel - 1) * autoMoveTimeNum;
        //CoreData.enemy_BossGreen_AutoMoveTime = 3f + (curLevel - 1) * autoMoveTimeNum;
        //CoreData.enemy_BossYellow_AutoMoveTime = 2f + (curLevel - 1) * autoMoveTimeNum;
        //CoreData.enemy_BossWhite_AutoMoveTime = 2f + (curLevel - 1) * autoMoveTimeNum; 
        #endregion

        #region 子弹攻击力
        //规则：每n关子弹攻击力+1   普通坦克上限 4， Boss坦克上限8
        //算法：当前子弹攻击力 =上一次子弹攻击力 + 当前关卡数/n > 4或者8 ？ 当前子弹攻击力 + 当前关卡数/n,:  4或者8
        int bulletAtkNum = BulletAtkNum; //每十关加一次        加 curLevel/bulletAtkNum 攻击
        CoreData.enemy_NormalWhite1_BulletAtk = (1 + curLevel / bulletAtkNum <= 4) ? 1 + curLevel / bulletAtkNum : 4;
        CoreData.enemy_NormalRed2_BulletAtk = (1 + curLevel / bulletAtkNum <= 4) ? 1 + curLevel / bulletAtkNum : 4;
        CoreData.enemy_NormalWhite3_BulletAtk = (1 + curLevel / bulletAtkNum <= 4) ? 1 + curLevel / bulletAtkNum : 4;
        CoreData.enemy_NormalRed4_BulletAtk = (1 + curLevel / bulletAtkNum <= 4) ? 1 + curLevel / bulletAtkNum : 4;
        CoreData.enemy_BossRed_BulletAtk = (3 + curLevel / bulletAtkNum <= 8) ? 1 + curLevel / bulletAtkNum : 8;
        CoreData.enemy_BossGreen_BulletAtk = (2 + curLevel / bulletAtkNum <= 8) ? 1 + curLevel / bulletAtkNum : 8;
        CoreData.enemy_BossYellow_BulletAtk = (2 + curLevel / bulletAtkNum <= 8) ? 1 + curLevel / bulletAtkNum : 8;
        CoreData.enemy_BossWhite_BulletAtk = (1 + curLevel / bulletAtkNum <= 8) ? 1 + curLevel / bulletAtkNum : 8;
        #endregion

        #region 攻击频率 (秒/次
        //攻击频率(秒/次)  每过一关减去一定触发时间n（上限1秒/次）
        //算法:  当前攻击频率 - n > 1f? 当前攻击频率 - n : 1;
        float autoAttackHZNum = AutoATKHZNum;
        CoreData.enemy_NormalWhite1_AutoAttackHZ =
            (CoreData.enemy_NormalWhite1_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_NormalWhite1_AutoAttackHZ - autoAttackHZNum : 1f;
        CoreData.enemy_NormalRed2_AutoAttackHZ =
            (CoreData.enemy_NormalRed2_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_NormalRed2_AutoAttackHZ - autoAttackHZNum : 1f;
        CoreData.enemy_NormalWhite3_AutoAttackHZ =
            (CoreData.enemy_NormalWhite3_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_NormalWhite3_AutoAttackHZ - autoAttackHZNum : 1f;
        CoreData.enemy_NormalRed4_AutoAttackHZ =
            (CoreData.enemy_NormalRed4_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_NormalRed4_AutoAttackHZ - autoAttackHZNum : 1f;
        CoreData.enemy_BossRed_AutoAttackHZ =
            (CoreData.enemy_BossRed_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_BossRed_AutoAttackHZ - autoAttackHZNum : 1f;
        CoreData.enemy_BossGreen_AutoAttackHZ =
            (CoreData.enemy_BossGreen_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_BossGreen_AutoAttackHZ - autoAttackHZNum : 1f;
        CoreData.enemy_BossYellow_AutoAttackHZ =
            (CoreData.enemy_BossYellow_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_BossYellow_AutoAttackHZ - autoAttackHZNum : 1f;
        CoreData.enemy_BossWhite_AutoAttackHZ =
            (CoreData.enemy_BossWhite_AutoAttackHZ - autoAttackHZNum > 1f) ? CoreData.enemy_BossWhite_AutoAttackHZ - autoAttackHZNum : 1f;
        #endregion

        #region 无敌时长 (秒)
        //规则：每关加一定的无敌时间n，（上限5秒）
        //算法：当前无敌时长 + n < 5f? 当前无敌时长 +0.02  : 5f;
        float godTimerNum = 0.02f;
        CoreData.enemy_NormalWhite1_GodTimer =
             (CoreData.enemy_NormalWhite1_GodTimer + godTimerNum < 5f) ? CoreData.enemy_NormalWhite1_GodTimer + godTimerNum : 5f;
        CoreData.enemy_NormalRed2_AutoAttackHZ =
             (CoreData.enemy_NormalRed2_AutoAttackHZ + godTimerNum < 5f) ? CoreData.enemy_NormalRed2_AutoAttackHZ + godTimerNum : 5f;
        CoreData.enemy_NormalWhite3_AutoAttackHZ =
             (CoreData.enemy_NormalWhite3_AutoAttackHZ + godTimerNum < 5f) ? CoreData.enemy_NormalWhite3_AutoAttackHZ + godTimerNum : 5f;
        CoreData.enemy_NormalRed4_AutoAttackHZ =
             (CoreData.enemy_NormalRed4_AutoAttackHZ + godTimerNum < 5f) ? CoreData.enemy_NormalRed4_AutoAttackHZ + godTimerNum : 5f;
        CoreData.enemy_BossRed_AutoAttackHZ =
             (CoreData.enemy_BossRed_AutoAttackHZ + godTimerNum < 5f) ? CoreData.enemy_BossRed_AutoAttackHZ + godTimerNum : 5f;
        CoreData.enemy_BossGreen_AutoAttackHZ =
             (CoreData.enemy_BossGreen_AutoAttackHZ + godTimerNum < 5f) ? CoreData.enemy_BossGreen_AutoAttackHZ + godTimerNum : 5f;
        CoreData.enemy_BossYellow_AutoAttackHZ =
             (CoreData.enemy_BossYellow_AutoAttackHZ + godTimerNum < 5f) ? CoreData.enemy_BossYellow_AutoAttackHZ + godTimerNum : 5f;
        CoreData.enemy_BossWhite_AutoAttackHZ =
             (CoreData.enemy_BossWhite_AutoAttackHZ + godTimerNum < 5f) ? CoreData.enemy_BossWhite_AutoAttackHZ + godTimerNum : 5f;
        #endregion
    }




}

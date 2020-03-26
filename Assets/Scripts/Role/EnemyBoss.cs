using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 新型敌方坦克
/// </summary>
public class EnemyBoss : Enemy
{ 
    public EnemyColorType enemyColorType = EnemyColorType.white;
    [Header("红、绿、黄、白")]
    public Sprite[] upArr;
    public Sprite[] rightArr;
    public Sprite[] downArr;
    public Sprite[] leftArr;

    //当前是否为红色BOSS坦克
    //public bool curIsRedBossTank = false;
    //是否已爆出奖励
    private bool isCreateAward = false;

    private void Start()
    {
        GetTankColorType();
    }


    /// <summary>
    /// 根据枚举类型颜色 更换贴图以及数据
    /// </summary>
    private void GetTankColorType()
    {
        switch (enemyColorType)
        {
            case EnemyColorType.red:
                RedTank();
                //curIsRedBossTank = true;
                break;
            case EnemyColorType.green:
                GreenTank();
                break;
            case EnemyColorType.yellow:
                YellowTank();
                break;
            case EnemyColorType.white:
                WhiteTank();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 变色处理 3.0 
    /// </summary> 
    public void ChangeColor()
    {
        //记录当前生命值
        int curLife = CurEnemyLife;
        if (CurEnemyLife <= 0 || CurEnemyLife > CoreData.enemy_BossRed_Life) return;
        if (CurEnemyLife > CoreData.enemy_BossGreen_Life && CurEnemyLife <= CoreData.enemy_BossRed_Life)
        {
            RedTank();
            CurEnemyLife = curLife;
            //  curIsRedBossTank = true;
        }

        else if (CurEnemyLife > CoreData.enemy_BossYellow_Life && CurEnemyLife <= CoreData.enemy_BossGreen_Life)
        {
            GreenTank();
            CurEnemyLife = curLife;
            // curIsRedBossTank = false;
        }
        else if (CurEnemyLife > CoreData.enemy_BossWhite_Life && CurEnemyLife <= CoreData.enemy_BossYellow_Life)
        {
            YellowTank();
            CurEnemyLife = curLife;
            //curIsRedBossTank = false;
        }
        else
        {
            WhiteTank();
            CurEnemyLife = curLife;
            // curIsRedBossTank = false;
        }
        //创建随机奖励
        if (enemyColorType == EnemyColorType.red && !isCreateAward)
        {
            GameObject.Find("AwardContainer").GetComponent<CreateAward>().CreateRandomAward();
            isCreateAward = true;
        }
    }

    ///// <summary>
    ///// 变色处理  old2.1
    ///// 敌方坦克： 
    ///// 初始化绿色生命值6=》被击中三次=》变黄色=》被击中2次=》变白色=》再被击中一次=》死亡
    ///// 初始化红色生命值8 =》被击中两次=》绿色生命值6=》被击中三次=》变黄色=》被击中2次=》变白色=》再被击中一次=》死亡
    ///// </summary> 
    //public void ChangeColor()
    //{
    //    //记录当前生命值
    //    int curLife = CurEnemyLife;
    //    if (CurEnemyLife <= 0 || CurEnemyLife > 8) return;
    //    if (CurEnemyLife > 6 && CurEnemyLife <= 8)
    //    {
    //        RedTank();
    //        CurEnemyLife = curLife;
    //      //  curIsRedBossTank = true;
    //    }
           
    //    else if (CurEnemyLife > 3 && CurEnemyLife <= 6)
    //    {
    //        GreenTank();
    //        CurEnemyLife = curLife;
    //       // curIsRedBossTank = false;
    //    }
    //    else if (CurEnemyLife > 1 && CurEnemyLife <= 3)
    //    {
    //        YellowTank();
    //        CurEnemyLife = curLife;
    //        //curIsRedBossTank = false;
    //    }
    //    else
    //    {
    //        WhiteTank();
    //        CurEnemyLife = curLife;
    //       // curIsRedBossTank = false;
    //    }
    //    //创建随机奖励
    //    if (enemyColorType ==  EnemyColorType.red && !isCreateAward)
    //    {
    //        GameObject.Find("AwardContainer").GetComponent<CreateAward>().CreateRandomAward();
    //        isCreateAward = true;
    //    }
    //}


    #region 四种颜色坦克的属性配置

    private void RedTank()
    {
        //贴图
        up = upArr[0];
        right = rightArr[0];
        down = downArr[0];
        left = leftArr[0];
        //属性
        CurEnemyLife = 8;      //生命值
        bulletAtk = 3;         //攻击力 
        autoAttackHZ = 1.5f;   //攻击频率 秒/次
        moveSpeed = 1.5f;      //移动速度
        autoMoveHZ = 5;        //移动频率 秒/次
        autoMoveTime = 2;      //每次移动时长 秒  
    }
    private void GreenTank()
    {
        //贴图
        up = upArr[1];
        right = rightArr[1];
        down = downArr[1];
        left = leftArr[1];
        //属性
        CurEnemyLife = 6;      //生命值
        bulletAtk = 2;           //攻击力 
        autoAttackHZ = 1.5f;   //攻击频率 秒/次
        moveSpeed = 1;         //移动速度
        autoMoveHZ = 6;        //移动频率 秒/次
        autoMoveTime = 3;      //每次移动时长 秒 
    }
    private void YellowTank()
    {
        //贴图
        up = upArr[2];
        right = rightArr[2];
        down = downArr[2];
        left = leftArr[2];
        //属性
        CurEnemyLife = 3;      //生命值
        bulletAtk = 2;           //攻击力 
        autoAttackHZ = 2f;   //攻击频率 秒/次
        moveSpeed = 1;         //移动速度
        autoMoveHZ = 4;        //移动频率 秒/次
        autoMoveTime = 2;      //每次移动时长 秒 
    }
    private void WhiteTank()
    {
        //贴图
        up = upArr[3];
        right = rightArr[3];
        down = downArr[3];
        left = leftArr[3];
        //属性
        CurEnemyLife = 1;      //生命值
        bulletAtk = 1;           //攻击力 
        autoAttackHZ = 2f;   //攻击频率 秒/次
        moveSpeed = 1;         //移动速度
        autoMoveHZ = 3;        //移动频率 秒/次
        autoMoveTime = 2;      //每次移动时长 秒 
    }
    #endregion


    public enum EnemyColorType
    {
        red,
        green,
        yellow,
        white
    }
}

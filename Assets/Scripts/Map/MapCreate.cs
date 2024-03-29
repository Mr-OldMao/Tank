﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 创建地图
/// 1）创建初始玩家、敌人的出生点特效-》玩家、敌人实体
/// 2）创建地图上砖、墙、边界墙、水、草、鸟窝
/// </summary>
public class MapCreate : MonoBehaviour
{
    //实体
    [Tooltip("0.鸟窝，1.墙，2.砖，3.水，4.草，5.边界墙")]
    public GameObject[] mapItem;
    [Tooltip("坦克出生点特效")]
    public GameObject eff_Born;
    private GameObject[] m_CoreBorderContainer = new GameObject[5]; //鸟窝边界容器
    //属性 
    public List<Vector3> itemPostionList = new List<Vector3>(); //标记已用坐标(坦克不标记)
    [Tooltip("当前敌人个数")]
    public int curEnemyCount = 0;


    private static MapCreate m_Instance;
    public static MapCreate GetInstance
    {
        get { return m_Instance; }
        set { m_Instance = value; }
    }

    //[Tooltip("自动生成敌人的周期（时间间隔）")]
    //public float createEnemyTimer = 5;

    private bool isGetEnemyMax = false;      //是否到达了 敌军上限
    public int addEnemyCount = 0;           //累计产生的敌军个数

    void Start()
    {
        m_Instance = this;
        Init();
    }

    /// <summary>
    /// 重新初始化 通关后逻辑
    /// </summary>
    public void ReInit()
    {
        //暂停 通关判定
        GameMode.isListenPassCurLevel = false;
        //计数器归零
        addEnemyCount = 0;
        //坐标列表清空
        itemPostionList.Clear();
        //清除墙体
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject); 
        } 
        //禁止之前Invoke创造敌人
        CancelInvoke("CreateEnemy"); 
        Init();

        GameMode.isListenPassCurLevel = true; 
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        //生成玩家
        GameObject playerIns = Instantiate(eff_Born, new Vector3(-2, -8, 0), Quaternion.identity);
        playerIns.GetComponent<Born>().isCreatePlayer = true;
        //生成敌人  1s后开始生成，之后每隔3S生成一次 
        InvokeRepeating("CreateEnemy", 1f, CoreData.createEnemyTimer);
        //实例化鸟窝 0,-7,0
        CreateItem(mapItem[0], new Vector3(0, -8, 0), Quaternion.identity);
        //实例化鸟窝边界
        CoreBorder(2);
        //实例化边界透明墙
        for (int x = -11; x <= 11; x++)
        {
            CreateItem(mapItem[5], new Vector3(x, 9, 0), Quaternion.identity);
            CreateItem(mapItem[5], new Vector3(x, -9, 0), Quaternion.identity);
        }
        for (int y = -8; y <= 8; y++)
        {
            CreateItem(mapItem[5], new Vector3(-11, y, 0), Quaternion.identity);
            CreateItem(mapItem[5], new Vector3(11, y, 0), Quaternion.identity);
        }
        //自动生成地图墙壁 水 草
        for (int x = 0; x < 20; x++)
        {
            CreateItem(mapItem[1], CreateRangomPos(), Quaternion.identity);
            CreateItem(mapItem[3], CreateRangomPos(), Quaternion.identity);
            CreateItem(mapItem[4], CreateRangomPos(), Quaternion.identity);
        }
        //自动生成地图砖
        for (int i = 0; i < 50; i++)
        {
            CreateItem(mapItem[2], CreateRangomPos(), Quaternion.identity);
        } 
    }

    /// <summary>
    /// 创建物体
    /// 1.先判断创建的位置是否已被标记，未被标记才可完成创建
    /// 2.自动标记坐标
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="pos"></param>
    /// <param name="roa"></param>
    private GameObject CreateItem(GameObject prop, Vector3 pos, Quaternion roa)
    {
        //判断创建的位置是否已被标记
        if (!JudgeCurPosUsed(pos))
        {
            GameObject obj = Instantiate(prop, pos, roa);
            obj.transform.SetParent(gameObject.transform);
            //标记已用坐标
            itemPostionList.Add(pos);
            return obj;
        }
        else
            return null;
    }




    /// <summary> 
    /// 判定当前坐标是否被使用过   T-已经被使用  F-未被使用
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool JudgeCurPosUsed(Vector3 pos)
    {
        if (itemPostionList.Exists(p => p == pos))
        {
            //Debug.Log("该坐标已被使用");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 移除当前所标记的坐标
    /// </summary>
    /// <param name="pos"></param>
    public void RemoveCurPosUsed(Vector3 pos)
    {
        if (JudgeCurPosUsed(pos))
        {
            itemPostionList.Remove(pos);
        }
    }



    /// <summary>
    /// 产生合法的随机位置 并返回 
    /// </summary>
    /// <param name="isAllMap">是否为整张地图的随机位置</param>
    /// <returns></returns>
    public Vector3 CreateRangomPos(bool isAllMap = false)
    {
        while (true)
        {
            Vector3 createPos = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            //x != +-10  y!= +-8  创建地图时这四行为边界不生成物体 x:[-9,9]  y:[-7,7]
            if (isAllMap)
            {
                createPos = new Vector3(Random.Range(-10, 11), Random.Range(-8, 9), 0);
            }
            if (!JudgeCurPosUsed(createPos))
            {
                return createPos;
            }
        }
    }

    /// <summary>
    /// 鸟窝边界  1-墙 2-砖
    /// </summary>
    /// <param name="index"></param>
    public void CoreBorder(int index)
    {
        //移除鸟窝边界所标记的坐标
        RemoveCurPosUsed(new Vector3(-1, -7, 0));
        RemoveCurPosUsed(new Vector3(1, -7, 0));
        RemoveCurPosUsed(new Vector3(-1, -8, 0));
        RemoveCurPosUsed(new Vector3(1, -8, 0));
        RemoveCurPosUsed(new Vector3(0, -7, 0));
        //移除容器中的实体
        foreach (GameObject item in m_CoreBorderContainer)
        {
            if (item != null) Destroy(item);
        }
        //创建墙/砖 并存放在容器中 
        m_CoreBorderContainer[0] = CreateItem(mapItem[index], new Vector3(-1, -7, 0), Quaternion.identity);
        m_CoreBorderContainer[1] = CreateItem(mapItem[index], new Vector3(1, -7, 0), Quaternion.identity);
        m_CoreBorderContainer[2] = CreateItem(mapItem[index], new Vector3(-1, -8, 0), Quaternion.identity);
        m_CoreBorderContainer[3] = CreateItem(mapItem[index], new Vector3(1, -8, 0), Quaternion.identity);
        m_CoreBorderContainer[4] = CreateItem(mapItem[index], new Vector3(0, -7, 0), Quaternion.identity);
    }

    void Update()
    {
        //判断当前是否到达了敌军上限
        //前提： 当前场上敌军<  Max场上敌军
        if (curEnemyCount < CoreData.enemyMaxCount)
        {
            isGetEnemyMax = false;
        }
        else
        {
            isGetEnemyMax = true;
        }
    }

    /// <summary>
    /// 随机生成敌人
    /// 产生规则：
    /// 1.累计生产的军 < 本关所需要生产的敌军  
    /// 2.未达到场上敌军上限
    /// </summary>
    private void CreateEnemy()
    {
        //累计生产的军<所需要生产的敌军
        if (addEnemyCount < CoreData.needCreateEnemyCount)
        {
            //未达到场上敌军上限   
            if (!isGetEnemyMax)
            {
                //0-左 1-中 2-右
                int num = Random.Range(0, 3);
                //前提： 当前场上敌军<  Max场上敌军
                if (curEnemyCount < CoreData.enemyMaxCount)
                {
                    curEnemyCount++;
                    addEnemyCount++;
                    Debug.Log("当前场上敌人还剩下：" + curEnemyCount + ",本关累计已产生敌人：" + addEnemyCount +
                       "本关需要产生的敌人个数：" + CoreData.needCreateEnemyCount + ",还需生产敌人：" + (CoreData.needCreateEnemyCount - addEnemyCount));
                    switch (num)
                    {
                        case 0:
                            Instantiate(eff_Born, new Vector3(-10, 8, 0), Quaternion.identity);
                            break;
                        case 1:
                            Instantiate(eff_Born, new Vector3(0, 8, 0), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(eff_Born, new Vector3(10, 8, 0), Quaternion.identity);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    isGetEnemyMax = true;
                }
            }

        }
    }
}

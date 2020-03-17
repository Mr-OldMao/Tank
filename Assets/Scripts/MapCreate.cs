using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 创建地图
/// 1）创建敌人
/// </summary>
public class MapCreate : MonoBehaviour
{
    [Tooltip("0.鸟窝，1.墙，2.砖，3.水，4.草，5.边界墙")]
    public GameObject[] mapItem;
    [Tooltip("坦克出生点特效")]
    public GameObject eff_Born;

    //[Tooltip("0玩家，1白一级坦克，2红一级坦克，3白二级坦克，4红二级坦克")]
    //public GameObject[] tankItem;
    //属性 
    private List<Vector3> itemPostionList = new List<Vector3>(); //标记已用坐标

    void Start()
    {
        Init();  
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        //实例化鸟窝 0,-7,0
        CreateItem(mapItem[0], new Vector3(0, -8, 0), Quaternion.identity);
        CreateItem(mapItem[2], new Vector3(-1, -7, 0), Quaternion.identity);
        CreateItem(mapItem[2], new Vector3(1, -7, 0), Quaternion.identity);
        CreateItem(mapItem[2], new Vector3(-1, -8, 0), Quaternion.identity);
        CreateItem(mapItem[2], new Vector3(1, -8, 0), Quaternion.identity);
        CreateItem(mapItem[2], new Vector3(0, -7, 0), Quaternion.identity);
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
        for (int x =0; x < 20; x++)
        {
            CreateItem(mapItem[1], CreateRangomPos(), Quaternion.identity);
            CreateItem(mapItem[3], CreateRangomPos(), Quaternion.identity); 
            CreateItem(mapItem[4], CreateRangomPos(), Quaternion.identity);
        }
        //自动生成地图砖
        for (int i = 0; i <50  ; i++)
        {
            CreateItem(mapItem[2], CreateRangomPos(), Quaternion.identity);
        }
        //生成玩家
         GameObject insPlayer =  CreateItem(eff_Born, new Vector3(-2, -8, 0), Quaternion.identity);
        insPlayer.GetComponent<Born>().isCreatePlayer = true;
        //生成敌人  1s后开始生成，之后每隔3S生成一次
        InvokeRepeating("CreateEnemy", 1f, 3f);
    }

    /// <summary>
    /// 创建物体
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="pos"></param>
    /// <param name="roa"></param>
    private GameObject CreateItem(GameObject prop, Vector3 pos, Quaternion roa)
    {
        GameObject obj = Instantiate(prop, pos, roa);
        obj.transform.SetParent(gameObject.transform);
        //标记已用坐标
        itemPostionList.Add(pos);
        return obj;
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
    /// 产生合法的随机位置 并返回
    /// </summary>
    /// <returns></returns>
    private Vector3 CreateRangomPos()
    {
        //x != +-10  y!= +-8 这四行为边界不生成物体 x:[-9,9]  y:[-7,7]
        while (true)
        {
            Vector3 createPos = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            if (!JudgeCurPosUsed(createPos))
            { 
                return createPos;
            }
        }
    }

     
    /// <summary>
    /// 随机生成敌人
    /// </summary>
    private void CreateEnemy()
    {
        //0-左 1-中 2-右
        int num = Random.Range(0, 3);
        switch (num)
        {
            case 0:
                CreateItem(eff_Born, new Vector3(-10, 8, 0), Quaternion.identity); 
                break;
            case 1: 
                CreateItem(eff_Born, new Vector3(0, 8, 0), Quaternion.identity); 
                break;
            case 2: 
                CreateItem(eff_Born, new Vector3(10, 8, 0), Quaternion.identity);
                break;
            default:
                break;
        }
    }
}

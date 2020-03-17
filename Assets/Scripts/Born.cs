using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 产生玩家或敌人 
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

    // Update is called once per frame
    void Update()
    {

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
            GameObject ins =Instantiate(prefab_EnemyTanks[Random.Range(0, prefab_EnemyTanks.Length)], transform.position, transform.rotation);
            //放入容器
            if (enemyContainer)
                ins.transform.SetParent(enemyContainer.transform);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 创建奖励道具(随机位置，随机道具)
/// </summary>
public class CreateAward : MonoBehaviour
{

    [Tooltip("奖励实体:0生命，1静止移动，2鸟窝边界，3歼灭全体坦克，4无敌，5等级")]
    public GameObject[] awardPrefab;
 


    /// <summary>
    /// 创建随机奖励
    /// </summary>
    public  void CreateRandomAward()
    {
        int randomNum = Random.Range(0, 8);
        GameObject insAward;
        if (randomNum>=5)
            insAward  =Instantiate(awardPrefab[5], MapCreate.GetInstance.CreateRangomPos(true), Quaternion.identity);
        else
            insAward= Instantiate(awardPrefab[randomNum], MapCreate.GetInstance.CreateRangomPos(true), Quaternion.identity);
        insAward.transform.SetParent(this.transform);
        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.BGM, AudioManager.GetInstance.audioClip[6]);//播放音效 
    } 
}

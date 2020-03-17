using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.BGM, AudioManager.GetInstance.audioClip[8]);//播放音效 
    }

     
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 选择场景
/// 功能：
/// 1.跳转创建
/// 2.获取到玩家所选模式
/// 
/// </summary>
public class SelectScene : MonoBehaviour
{
    public Button btn_Normal;
    public Button btn_Hard;
    public Button btn_NoEnd;

    public static SelectMode selectType;

    /// <summary>
    /// 游戏关卡
    /// </summary>
    public enum SelectMode
    {
        Normal,
        Hard,
        NoEnd
    }

    // Use this for initialization
    void Start()
    {
        btn_Normal.onClick.AddListener(() =>
        {
            selectType = SelectMode.Normal;
            SceneManager.LoadScene("Main");
        });
        btn_Hard.onClick.AddListener(() =>
        {
            selectType = SelectMode.Hard;
            SceneManager.LoadScene("Main");
        });

        btn_NoEnd.onClick.AddListener(() =>
        {
            selectType = SelectMode.NoEnd;
            SceneManager.LoadScene("Main");
        }); 
    }

    // Update is called once per frame
    void Update()
    {
 
    }
}

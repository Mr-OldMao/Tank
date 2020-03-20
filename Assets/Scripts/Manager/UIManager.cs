using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    //UI组件
    public Image img_Life;
    public Image img_Score;
    public Text txt_Life;
    public Text txt_Score;
    public Image imt_GameOver;
    public Button btn_Restart;
    public static bool isGameOver = false;  //游戏是否结束
  
    void Start()
    {
        imt_GameOver.gameObject.SetActive(false);
        btn_Restart.onClick.AddListener(() =>
        {
            //重新开始 
            SceneManager.LoadScene("Main");
            isGameOver = false;
        } 
        );
    }
    


    void Update()
    {
        txt_Life.text =":"+ PlayerManager.GetInstance.lifeValue.ToString();
        txt_Score.text = ":" + PlayerManager.GetInstance.score.ToString();

        //显示游戏结束UI
        if (isGameOver && !imt_GameOver.isActiveAndEnabled)
        {
            imt_GameOver.gameObject.SetActive(true); 
        }
    }
     
}

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
    //游戏结束
    public Image img_GameOver;
    public Button btn_Restart;
    public static bool isGameOver = false;  //游戏是否结束 

    //模式
    public Image img_Normal;
    public Image img_Hard;
    public Image img_NotEnd;
    public Text txt_curLevel_Normal;
    public Text txt_curLevel_Hard;
    //ESC
    public Canvas can_Esc;
    public Button btn_BackStart;
    public Button btn_BackSelectMode;
    public Button btn_ExitGame;

    //游戏时间
    public Text txt_TimerM;     //分
    public Text txt_TimerS;     //秒
    private int m_TimerM = 0;
    private int m_TimerS = 0;
    private float m_TimerTemp;   //临时中间变量
    private bool m_IsStartTimer = false;    //是否开始计时
    private bool m_IsLast;      //是否为倒计时

    //游戏结束原因
    public Text txt_GameOverCause;


    private static UIManager m_Instace;
    public static UIManager GetInstance
    {
        get { return m_Instace; }
        set { m_Instace = value; }
    }
    void Awake()
    {
        m_Instace = this;
    }
    void Start()
    {
        can_Esc.gameObject.SetActive(false);
        img_GameOver.gameObject.SetActive(false);
        //重头开始 
        btn_Restart.onClick.AddListener(() =>
        {
            isGameOver = false;
            img_GameOver.gameObject.SetActive(false);
            SceneManager.LoadScene("Main");
        }
        );

        btn_BackStart.onClick.AddListener(() =>
        {
            isGameOver = false;
            img_GameOver.gameObject.SetActive(false);
            SceneManager.LoadScene("StartScene");
        }
        );
        btn_BackSelectMode.onClick.AddListener(() =>
        {
            isGameOver = false;
            img_GameOver.gameObject.SetActive(false);
            SceneManager.LoadScene("SelectScene");
        });
        btn_ExitGame.onClick.AddListener(() => Application.Quit());
    }


    /// <summary>
    /// 显示关卡UI
    /// </summary>
    public void ShowLevelUI(int levelNum)
    {
        if (GameManager.GetInstance.getSelectMode == SelectScene.SelectMode.Normal)
        {
            txt_curLevel_Normal.text = levelNum.ToString();
            img_Normal.gameObject.SetActive(true);
            img_Hard.gameObject.SetActive(false);
            img_NotEnd.gameObject.SetActive(false);
            StartCoroutine(WaitSomeTimeHide(img_Normal.gameObject, 3f));
        }
        else
        {
            txt_curLevel_Hard.text = levelNum.ToString();
            img_Hard.gameObject.SetActive(true);
            img_Normal.gameObject.SetActive(false);
            img_NotEnd.gameObject.SetActive(false);
            StartCoroutine(WaitSomeTimeHide(img_Hard.gameObject, 3f));
        }
    }

    //显示无尽模式UI
    public void ShowNotEndUI()
    {
        img_Normal.gameObject.SetActive(false);
        img_Hard.gameObject.SetActive(false);
        img_NotEnd.gameObject.SetActive(true);
        StartCoroutine(WaitSomeTimeHide(img_NotEnd.gameObject, 3f));
    }

    /// <summary>
    /// 显示时间计时器UI
    /// </summary>
    /// <param name="int">初始时间 （秒）</param>
    /// <param name="isLast">是否为倒计时</param>
    public void ShowTimerUI(int timer, bool isLast)
    {
        m_TimerTemp = timer; ;
        m_TimerM = timer / 60;
        m_TimerS = timer % 60;
        txt_TimerM.text = m_TimerM.ToString();
        //秒数小于十则在前补0 
        txt_TimerS.text = (m_TimerS < 10) ? "0" + m_TimerS.ToString() : m_TimerS.ToString();
        txt_TimerM.gameObject.SetActive(true);
        txt_TimerS.gameObject.SetActive(true);
        m_IsStartTimer = true;
        m_IsLast = isLast;
    }


    void Update()
    {
        //生命 杀敌数 实时显示
        txt_Life.text = ":" + PlayerManager.GetInstance.Life.ToString();
        txt_Score.text = ":" + PlayerManager.GetInstance.score.ToString();
        //游戏计时器
        if (m_IsStartTimer)
        {
            if (m_IsLast)
            {
                m_TimerTemp -= Time.deltaTime; //倒计时
                if (m_TimerTemp < 0)
                {
                    //游戏结束
                    GameManager.GetInstance.IsGameOvoer(GameManager.GameOverType.notTime);
                    //游戏计时器 重置
                    m_IsStartTimer = false;
                    m_TimerTemp = 0;
                }
            }
            else
                m_TimerTemp += Time.deltaTime; //正计时
            m_TimerM = (int)m_TimerTemp / 60;
            m_TimerS = (int)m_TimerTemp % 60;
            txt_TimerM.text = m_TimerM.ToString();
            //秒数小于十则在前补0 
            txt_TimerS.text = (m_TimerS < 10) ? "0" + m_TimerS.ToString() : m_TimerS.ToString();
        }



        //显示游戏结束UI
        if (isGameOver)
        {
            img_GameOver.gameObject.SetActive(true);
            //游戏计时器 重置
            m_IsStartTimer = false;
        }

        //ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowAndHide(can_Esc.gameObject);
        }
    }

    /// <summary>
    /// 延时隐藏
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitSomeTimeHide(GameObject target, float timer)
    {
        yield return new WaitForSeconds(timer);
        target.gameObject.SetActive(false);
    }

    private void ShowAndHide(GameObject obj)
    {
        if (obj.activeSelf) obj.SetActive(false);
        else obj.SetActive(true);
    }
}

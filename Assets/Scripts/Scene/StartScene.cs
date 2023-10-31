using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 开始场景
/// </summary>
public class StartScene : MonoBehaviour
{
    public Image img_First;
    public Image img_Double;
    public Button btnStart;
     
    void Start()
    {
        img_First.gameObject.SetActive(true);
        img_Double.gameObject.SetActive(false);
        btnStart.onClick.AddListener(() => SceneManager.LoadScene("SelectScene"));
    }

    // Update is called once per frame
    void Update()
    {
        Change();
    }
    private void Change()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            img_First.gameObject.SetActive(true);
            img_Double.gameObject.SetActive(false); 
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            img_First.gameObject.SetActive(false);
            img_Double.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //SceneManager.LoadScene("Main");
            SceneManager.LoadScene("SelectScene");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //参数 
    public float moveSpeed = 2f;              //移动速度
    public float attackHZ = 0.2f;             //攻击频率
    private float m_AttackHZTimer = 0;        //攻击频率计时器
    private bool m_IsAttack = true;           //允许攻击
    public Vector3 bulletEulerAngles;         //子弹应旋转的欧拉角
    public bool IsProtected = true;           //无敌状态
    public float protectedTimer = 3f;         //无敌时间（出生时） 
    //实体
    public GameObject go_Bullet;
    [Tooltip("坦克爆炸状态特效")]
    public GameObject eff_Explode;
    [Tooltip("坦克无敌状态特效")]
    public GameObject eff_Eff_Shield;
    [Tooltip("坦克上下左右移动精灵图片")]
    public Sprite up;
    public Sprite right;
    public Sprite down;
    public Sprite left;

    private Core coreState;     //鸟窝状态
    private bool isPlayMoveAudio =false; //正在播放移动音效
    // Use this for initialization
    void Start()
    {
        Invoke("Init", 0.3f); 
    }
    private void Init()
    {
        coreState = GameObject.FindGameObjectWithTag("Core").GetComponent<Core>(); 
    }
    // Update is called once per frame
    void Update()
    {
        //子弹攻击频率
        if (m_IsAttack == false)
        {
            m_AttackHZTimer += Time.deltaTime;
            if (m_AttackHZTimer > attackHZ)
            {
                m_IsAttack = true;
                m_AttackHZTimer = 0;
            }
        }
        //无敌状态倒计时
        if (IsProtected)
        {
            eff_Eff_Shield.SetActive(true);
            protectedTimer -= Time.deltaTime;
            if (protectedTimer <= 0)
            {
                IsProtected = false;
                eff_Eff_Shield.SetActive(false);
            }
        }
    }
    void FixedUpdate()
    {
        //鸟窝不炸才可 移动 攻击
        if (coreState)
        {
            if (!coreState.isCoreDie)
            {
                Move();
                if (m_IsAttack) Attack();
            } 
        }
    }

    //玩家移动
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Debug.Log("h:" + h + ",,,,,,,v:" + v);
        //音效
        if (h != 0 || v != 0)  //在移动
        {
            if (!isPlayMoveAudio)
            {
                AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Move, AudioManager.GetInstance.audioClip[1]);//播放音效 
                isPlayMoveAudio = true;
            } 
        }
        else //不在移动
        {
            if (isPlayMoveAudio)
            {
                AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Move, AudioManager.GetInstance.audioClip[2]);//播放音效 
                isPlayMoveAudio = false;
            } 
        }


        //通过h、v值 更换坦克方向贴图
        if (h > 0)
        {
            bulletEulerAngles = new Vector3(0, 0, -90);
            GetComponent<SpriteRenderer>().sprite = right;
        }
        if (h < 0)
        {
            bulletEulerAngles = new Vector3(0, 0, 90);
            GetComponent<SpriteRenderer>().sprite = left;
        }
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime * moveSpeed);
        if (h != 0) return;         //避免水平和竖直按键一起按
        if (v > 0)
        {
            bulletEulerAngles = new Vector3(0, 0, 0);
            GetComponent<SpriteRenderer>().sprite = up;
        }
        if (v < 0)
        {
            bulletEulerAngles = new Vector3(0, 0, 180);
            GetComponent<SpriteRenderer>().sprite = down;
        }
        transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime * moveSpeed);
        //Debug.Log("Horizontal:" + h + ",Vertical:" + v);
    }
    //玩家攻击
    private void Attack()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //生成子弹     子弹朝向问题： Quaternion.EulerAngles()---欧拉角(transform.rotation)转成四元素
            GameObject bullet = Instantiate(go_Bullet, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));

            m_IsAttack = false;
        }
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    private void Die()
    {
        if (!IsProtected)
        {
            //生成死亡爆炸粒子
            GameObject ins_Explode = Instantiate(eff_Explode, transform.position, transform.rotation);
            //摧毁玩家
            Destroy(this.gameObject);
            PlayerManager.GetInstance.isDie = true;
        }
    }

}

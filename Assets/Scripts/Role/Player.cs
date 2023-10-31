using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //参数 
    [SerializeField]
    private float moveSpeed = 2f;              //移动速度
    [SerializeField]
    private float attackHZ = 0.2f;             //攻击频率
    [SerializeField]
    private int level = 0;                    //当前坦克等级  共八个等级 默认出生为0级 也为当前生命的生命值 

    private float m_AttackHZTimer = 0;        //攻击频率计时器
    private bool m_IsAttack = true;           //允许攻击
    public Vector3 bulletEulerAngles;         //子弹应旋转的欧拉角
    public bool IsGod = true;                 //无敌状态
    public float godTime = 3f;                //无敌时间（出生时） 

    //实体
    public GameObject go_Bullet;
    [Tooltip("坦克爆炸状态特效")]
    public GameObject eff_Explode;
    [Tooltip("坦克无敌状态特效")]
    public GameObject eff_Eff_Shield;
    [Tooltip("坦克上下左右移动精灵图片")]
    public Sprite[] up;
    public Sprite[] right;
    public Sprite[] down;
    public Sprite[] left;

    private bool isPlayMoveAudio = false; //正在播放移动音效
    private TouchPanel m_TouchPanel;
    /// <summary>
    /// get set 坦克等级  间接修改移动速度和攻击频率
    /// </summary>
    public int Level
    {
        get { return level; }
        set
        {
            if (value < 8)
            {
                //修改移动速度和攻击频率 
                if (value > level)
                    ChangeMoveAndAttackHZ();
                else
                    ChangeMoveAndAttackHZ(false);
                //更新坦克等级
                level = value;
            }
        }
    }

    void Awake()
    {
        InitData();
        m_TouchPanel = FindObjectOfType<TouchPanel>();
        //MoveByEasyTouch();
        UIManager.GetInstance.btnFire.onClick.AddListener(()=> Attack());
    }
    void Start()
    {
        Invoke("Init", 0.3f);
    }
    private void InitData()
    {
        moveSpeed = CoreData.playerMoveSpeed;
        attackHZ = CoreData.playerAttackHZ;
        level = CoreData.playerLevel;
        godTime = CoreData.playerGodTime;
    }

    /// <summary>
    /// 修改移动速度和攻击频率  根据当前等级
    /// </summary>
    public void ChangeMoveAndAttackHZ(bool isAdd = true)
    {
        //升级
        if (isAdd)
        {
            //[0,2] 增加移动+0.1   攻击频率提高0.015
            if (level >= 0 && level < 3)
            {
                moveSpeed += 0.1f;
                attackHZ -= 0.015f;
            }
            //[3,5] 增加移动+0.15   攻击频率提高0.02
            else if (level >= 3 && level < 6)
            {
                moveSpeed += 0.1f;
                attackHZ -= 0.02f;
            }
            else if (level >= 6 && level < 8)
            {
                //moveSpeed += 0.1f;
                attackHZ -= 0.015f;
            }
        }
        //降级
        else
        {
            if (level >= 0 && level < 3)
            {
                moveSpeed -= 0.1f;
                attackHZ += 0.015f;
            }
            else if (level >= 3 && level < 6)
            {
                moveSpeed -= 0.15f;
                attackHZ += 0.02f;
            }
            else if (level >= 6 && level < 8)
            {
                moveSpeed -= 0.1f;
                attackHZ += 0.015f;
            }
        }
    }

    /// <summary>
    /// 修改子弹射速、攻击力  根据当前等级
    /// </summary>
    private void ChangeBulletSpeedAndATK(ref float oldBulSpeed, ref int oldBulATK)
    {
        //子弹射速  默认射速为4  
        if (level >= 0 && level < 8)
        {
            oldBulSpeed += level * 0.5f;
        }
        //子弹攻击力
        if (level >= 6 && level < 8)
        {
            oldBulATK = 2;
        }
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
        if (IsGod)
        {
            TankGod(ref godTime);
        }
    }
    void FixedUpdate()
    {
        if (!GameManager.GetInstance.isGameOver)
        {
            //float h = Input.GetAxisRaw("Horizontal");
            //float v = Input.GetAxisRaw("Vertical");
            float h = m_TouchPanel._hor;
            float v = m_TouchPanel._ver;
            
            Move(h, v);

            if (m_IsAttack)
            {
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.J))
                {
                    Attack();
                }
            } 
        }
    }




    //坦克无敌
    public void TankGod(ref float godTime)
    {
        eff_Eff_Shield.SetActive(true);
        godTime -= Time.deltaTime;
        //Debug.Log("TankGod godTime:" + godTime);
        if (godTime <= 0)
        {
            IsGod = false;
            eff_Eff_Shield.SetActive(false);
            //godBornTimer = godTime;  
        }
    }

    //private void MoveByEasyTouch()
    //{
    //    ETCJoystick JoystickLeft = GameObject.FindObjectOfType<ETCJoystick>();
    //    JoystickLeft.onMove.AddListener((v2) =>
    //    {
    //        Move(v2.x, v2.y);
    //    });
    //}

    //玩家移动
    private void Move(float h, float v)
    {
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
        if (Mathf.Abs(h) > Mathf.Abs(v))
        {
            if (h > 0)
            {
                bulletEulerAngles = new Vector3(0, 0, -90);
                GetComponent<SpriteRenderer>().sprite = right[level];
            }
            if (h < 0)
            {
                bulletEulerAngles = new Vector3(0, 0, 90);
                GetComponent<SpriteRenderer>().sprite = left[level];
            }
            transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime * moveSpeed);
        }
        else
        {
            if (v > 0)
            {
                bulletEulerAngles = new Vector3(0, 0, 0);
                GetComponent<SpriteRenderer>().sprite = up[level];
            }
            if (v < 0)
            {
                bulletEulerAngles = new Vector3(0, 0, 180);
                GetComponent<SpriteRenderer>().sprite = down[level];
            }
            transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime * moveSpeed);
        }

        //Debug.Log("Horizontal:" + h + ",Vertical:" + v);


        ////通过h、v值 更换坦克方向贴图 
        //if (h > 0)
        //{
        //    bulletEulerAngles = new Vector3(0, 0, -90);
        //    GetComponent<SpriteRenderer>().sprite = right[level];
        //}
        //if (h < 0)
        //{
        //    bulletEulerAngles = new Vector3(0, 0, 90);
        //    GetComponent<SpriteRenderer>().sprite = left[level];
        //}
        //transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime * moveSpeed);
        //if (h != 0) return;         //避免水平和竖直按键一起按
        //if (v > 0)
        //{
        //    bulletEulerAngles = new Vector3(0, 0, 0);
        //    GetComponent<SpriteRenderer>().sprite = up[level];
        //}
        //if (v < 0)
        //{
        //    bulletEulerAngles = new Vector3(0, 0, 180);
        //    GetComponent<SpriteRenderer>().sprite = down[level];
        //}
        //transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime * moveSpeed);
        ////Debug.Log("Horizontal:" + h + ",Vertical:" + v);
    }
    //玩家攻击
    private void Attack()
    {
        //生成子弹     子弹朝向问题： Quaternion.EulerAngles()---欧拉角(transform.rotation)转成四元素
        GameObject bullet = Instantiate(go_Bullet, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
        ChangeBulletSpeedAndATK(ref bullet.GetComponent<Bullet>().bulletSpeed,
            ref bullet.GetComponent<Bullet>().buttetATK);
        m_IsAttack = false;

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        //敌我坦克接触
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("Die", false); //不能销毁无敌敌人
            if (IsGod == false)
            {
                Die();
            }
            AudioManager.GetInstance.PlayAudioSource(AudioManager.AudioSourceType.Default, AudioManager.GetInstance.audioClip[0]);//播放音效
        }
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    private void Die()
    {
        if (!IsGod)
        {
            //生成死亡爆炸粒子
            GameObject ins_Explode = Instantiate(eff_Explode, transform.position, transform.rotation);
            //摧毁玩家
            Destroy(this.gameObject);
            PlayerManager.GetInstance.isDie = true;
        }
    }

}

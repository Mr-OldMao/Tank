using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人AI
/// 功能：
/// 1.每隔固定的时间间隔（attackHZ）攻击一次
/// 2.每隔固定的时间间隔（autoMoveHZ）移动一次  移动的时间（autoMoveTime）
/// 3.敌方坦克生命
/// </summary>
public class Enemy : MonoBehaviour
{
    //参数 
    [SerializeField]
    private int m_CurEnemyLife = 1;               //当前坦克生命值
    public float moveSpeed = 2f;                  //移动速度
    public int bulletAtk = 1;                     //子弹攻击力
    public float autoAttackHZ = 0.2f;             //攻击频率
    private float m_AutoAttackHZTimer = 0;        //攻击频率计时器
    private bool m_IsAutoAttack = true;           //允许攻击
    private Vector3 bulletEulerAngles;            //子弹应旋转的欧拉角
    public bool IsGod = true;                     //无敌状态
    public float godTimer = 2f;                   //无敌时间（出生时）
    public bool isRedTank = false;                //是否为红色坦克


    [Tooltip("是否允许电脑自动移动")]
    public bool canAutoMove = true;               //是否允许电脑自动移动
    [Tooltip("电脑自动移动频率")]
    public float autoMoveHZ = 3f;                 //电脑自动移动频率 
    private float m_AutoMoveHZTimer = 2f;         //自动移动频率计时器
    [Tooltip("电脑每次自动移动的时间")]
    public float autoMoveTime = 2f;               //电脑每次自动移动的时间 
    private float m_CurMoveTime;                  //当前自动移动的时间
    //实体
    public GameObject go_Bullet;
    [Tooltip("坦克爆炸状态特效")]
    public GameObject eff_Explode;
    [Tooltip("坦克无敌状态特效")]
    public GameObject eff_Shield;
    [Tooltip("坦克上下左右移动精灵图片")]
    public Sprite up;
    public Sprite right;
    public Sprite down;
    public Sprite left;



    float h = 0; //水平移动参数
    float v = -1; //竖直移动参数

    public int CurEnemyLife
    {
        get { return m_CurEnemyLife; }
        set { m_CurEnemyLife = value; }
    }
    void Start()
    {
        //优化：可让坦克一出生就移动
        m_AutoMoveHZTimer = autoMoveHZ - 0.5f;
        m_CurMoveTime = autoMoveTime;
        //红色坦克生命值为2
        if (isRedTank)
        {
            m_CurEnemyLife = 2;
        }
    }

     void Update()
    {
        //子弹攻击频率
        if (m_IsAutoAttack == false)
        {
            m_AutoAttackHZTimer += Time.deltaTime;
            if (m_AutoAttackHZTimer > autoAttackHZ)
            {
                m_IsAutoAttack = true;
                m_AutoAttackHZTimer = 0;
            }
        }
        //无敌状态倒计时
        if (IsGod)
        {
            eff_Shield.SetActive(true);
            godTimer -= Time.deltaTime;
            if (godTimer <= 0)
            {
                IsGod = false;
                eff_Shield.SetActive(false);
            }
        }


    }
     void FixedUpdate()
    { 
        Move();
        if (m_IsAutoAttack) Attack();
    }

    //敌人自动移动
    private void Move()
    {
        m_AutoMoveHZTimer += Time.deltaTime;
        //自动移动频率 
        if (m_AutoMoveHZTimer > autoMoveHZ)
        {
            canAutoMove = true;
            m_AutoMoveHZTimer = 0;
            int num = Random.Range(0, 9);  //[0,8]
            //往下走（鸟窝）
            if (num < 4)
            {
                h = 0; v = -1;
            }
            //右拐
            else if (num == 4 || num == 5)
            {
                h = 1; v = 0;
            }
            //左拐
            else if (num == 6 || num == 7)
            {
                h = 1; v = 0;
            }
            //往上走
            else
            {
                h = 0; v = 1;
            }
        }
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        //移动
        if (canAutoMove && m_CurMoveTime < autoMoveTime)
        {
            m_CurMoveTime += Time.deltaTime;
            transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime * moveSpeed);
            transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime * moveSpeed);
        }
        else
        {
            canAutoMove = false;
            m_CurMoveTime = 0;
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
        //if (h != 0) return;         //避免水平和竖直按键一起按
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
    }


    //敌人攻击
    private void Attack()
    {
        //生成子弹     子弹朝向问题： Quaternion.EulerAngles()---欧拉角(transform.rotation)转成四元素
        GameObject bullet = Instantiate(go_Bullet, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
        bullet.GetComponent<Bullet>().buttetATK = bulletAtk;
        m_IsAutoAttack = false;
    }

    /// <summary>
    /// 敌人死亡 
    /// </summary>
    /// <param name="canDestoryGodTank">是否能摧毁无敌状态的敌人 默认不可以</param>
    public void Die(bool canDestoryGodTank = false)
    {
        //if (!IsProtected || Award.canBombGodTank)
        if (!IsGod || canDestoryGodTank)
        {
            //生成粒子
            GameObject ins_Explode = Instantiate(eff_Explode, transform.position, transform.rotation);
            //摧毁当前敌人
            Destroy(this.gameObject);
            //杀敌数+1
            PlayerManager.GetInstance.score++;
            MapCreate.GetInstance.curEnemyCount--;
            //如果是红色坦克死亡则爆出奖励
            if (isRedTank)
            {
                GameObject.Find("AwardContainer").GetComponent<CreateAward>().CreateRandomAward();
            }
        }
    }
     
     
    /// <summary>
    /// 优化移动AI  敌方两坦克碰撞情况
    /// </summary>
    /// <param name="coll"></param>
    private void OnCollisionEnter2D(Collision2D coll)
    {
        //两敌方坦克碰撞 刷新一次移动
        if (coll.gameObject.tag == "Enemy")
        {
            //重置方向以及移动
            h = -h;
            v = -v;
            m_CurMoveTime = autoMoveTime - 0.2f;
        }
    } 
}

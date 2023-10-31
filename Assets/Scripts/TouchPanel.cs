using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摇杆控制面板控制物体移动，仿EasyTouch
/// 参考：https://blog.csdn.net/Mr_Sun88/article/details/84680361
/// </summary>
public class TouchPanel : MonoBehaviour
{
    /// <summary>
    /// 摇杆背景
    /// </summary>
    [SerializeField]
    private Transform _joyBg;
    /// <summary>
    /// 摇杆中心
    /// </summary>
    [SerializeField]
    private Transform _joyCenter;
    /// <summary>
    /// 摇杆半径
    /// </summary>
    private float _radius;
    /// <summary>
    /// 移动中心
    /// </summary>  
    private Vector2 _moveCenter;
    /// <summary>
    /// 鼠标到移动中心的向量
    /// </summary>
    private Vector2 _mouseToCenterVect;
    /// <summary>
    /// 鼠标到中心点的距离
    /// </summary>
    private float _mouseToCenterDistance;
    /// <summary>
    /// 水平获取值
    /// </summary>
    public float _hor { get; private set; }
    /// <summary>
    /// 垂直获取值
    /// </summary>
    public float _ver { get; private set; }
    /// <summary>
    /// 旋转角度
    /// </summary>
    private float _rotAngle;

    // Start is called before the first frame update
    void Start()
    {
        //_joyBg = transform.Find("JoyBg");
        //_joyCenter = transform.Find("JoyBg/JoyCenter");
        _player = GameObject.Find("Player").transform;
        _radius = 100;
    }
    /// <summary>
    /// 开始拖动
    /// </summary>
    public void OnDragBegain()
    {

        //移动中心点赋值
        _moveCenter = Input.mousePosition;
        Debug.Log("OnDragBegain " + _moveCenter);
        //显示摇杆
        _joyBg.gameObject.SetActive(true);
        _joyCenter.gameObject.SetActive(true);
        //摇杆背景位置修正到点击位置
        _joyBg.localPosition = _moveCenter;
        //摇杆中心位置修正到点击位置
        _joyCenter.localPosition = _moveCenter;
    }

    /// <summary>
    /// 正在拖动
    /// </summary>
    public void OnDragMove()
    {
        Debug.Log("OnDragMove");

        // 中心店到触摸点的向量赋值
        _mouseToCenterVect = (Vector2)Input.mousePosition - _moveCenter;
        //中心店到触摸点的距离计算
        _mouseToCenterDistance = Mathf.Clamp(_mouseToCenterVect.magnitude, 0, 100);
        //根据距离来判断摇杆中心的位置
        if (_mouseToCenterDistance < _radius)
        {
            //若是距离小于最大半径，这里取向量的归一值，就是模为1的向量，乘上中心到触摸点的距离，这个就是摇杆中心应该移动的方向和距离，并且移动是在移动中心的基础上，所以加上移动中心的坐标
            _joyCenter.localPosition = _mouseToCenterVect.normalized * _mouseToCenterDistance + _moveCenter;
        }
        else
        {
            //同上，不过就是限定了移动的最大距离
            _joyCenter.localPosition = _mouseToCenterVect.normalized * _radius + _moveCenter;
        }
        //摇杆中心的X - 移动中心的x就是水平的变化值，这里 /100 控制_hor在（-1，,1）之间
        _hor = (_joyCenter.localPosition.x - _moveCenter.x) / 100;
        //摇杆中心的Y - 移动中心的Y就是垂直的变化值，这里 /100 控制_hor在（-1，,1）之间
        _ver = (_joyCenter.localPosition.y - _moveCenter.y) / 100;

        //角度就是 中心店到触摸点的向量 和 2D平面Y轴正方向的夹角，这里只能显示0——180度
        _rotAngle = Vector3.Angle(_mouseToCenterVect, Vector3.up);

        //这里根据_hor的正负来判断摇杆中心是位于移动中心左侧还是右侧，然后修改度数，显示在0——360之间
        if (_hor < 0)
        {
            _rotAngle = 360 - _rotAngle;
        }
        //Vector3.forward 以Vector3.up为中心旋转轴，旋转_rotAngle度，这里计算出主角的旋转度数
        _forwardTarget = Quaternion.AngleAxis(_rotAngle, Vector3.up) * Vector3.forward;
    }

    /// <summary>
    /// 拖动结束
    /// </summary>
    public void OnDragEnd()
    {
        Debug.Log("OnDragEnd");

        //水平移动值归零
        _hor = 0;
        //垂直移动值归零
        _ver = 0;
        //隐藏摇杆
        _joyBg.gameObject.SetActive(false);
        _joyCenter.gameObject.SetActive(false);

    }
    /// 主角
    /// </summary>
    private Transform _player;
    /// <summary>
    /// 目标朝向
    /// </summary>
    private Vector3 _forwardTarget;

    // Update is called once per frame
    void Update()
    {
        //只有在水平或者垂直值大于0的情况下，主角才移动或旋转
        if (Mathf.Abs(_hor) > 0 || Mathf.Abs(_ver) > 0)
        {
            //Mathf.Clamp(_mouseToCenterDistance/100,0,1)：根据摇杆中心的移动距离判断速度
            //new Vector3(0,0,0.1f*Mathf.Clamp(_mouseToCenterDistance/100,0,1))：在Z轴方向上移动Mathf.Clamp(_mouseToCenterDistance/100,0,1)的距离，限定在（0-1）的大小
            //不停地在Z轴方向上移动Mathf.Clamp(_mouseToCenterDistance/100,0,1)的距离
            _player.position += _player.TransformDirection(new Vector3(0, 0, 0.1f * Mathf.Clamp(_mouseToCenterDistance / 100, 0, 1)));
            //根据摇杆的旋转设置主角的朝向
            _player.forward = _forwardTarget;

        }
    }
}

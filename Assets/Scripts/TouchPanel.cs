using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ҡ�˿��������������ƶ�����EasyTouch
/// �ο���https://blog.csdn.net/Mr_Sun88/article/details/84680361
/// </summary>
public class TouchPanel : MonoBehaviour
{
    /// <summary>
    /// ҡ�˱���
    /// </summary>
    [SerializeField]
    private Transform _joyBg;
    /// <summary>
    /// ҡ������
    /// </summary>
    [SerializeField]
    private Transform _joyCenter;
    /// <summary>
    /// ҡ�˰뾶
    /// </summary>
    private float _radius;
    /// <summary>
    /// �ƶ�����
    /// </summary>  
    private Vector2 _moveCenter;
    /// <summary>
    /// ��굽�ƶ����ĵ�����
    /// </summary>
    private Vector2 _mouseToCenterVect;
    /// <summary>
    /// ��굽���ĵ�ľ���
    /// </summary>
    private float _mouseToCenterDistance;
    /// <summary>
    /// ˮƽ��ȡֵ
    /// </summary>
    public float _hor { get; private set; }
    /// <summary>
    /// ��ֱ��ȡֵ
    /// </summary>
    public float _ver { get; private set; }
    /// <summary>
    /// ��ת�Ƕ�
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
    /// ��ʼ�϶�
    /// </summary>
    public void OnDragBegain()
    {

        //�ƶ����ĵ㸳ֵ
        _moveCenter = Input.mousePosition;
        Debug.Log("OnDragBegain " + _moveCenter);
        //��ʾҡ��
        _joyBg.gameObject.SetActive(true);
        _joyCenter.gameObject.SetActive(true);
        //ҡ�˱���λ�����������λ��
        _joyBg.localPosition = _moveCenter;
        //ҡ������λ�����������λ��
        _joyCenter.localPosition = _moveCenter;
    }

    /// <summary>
    /// �����϶�
    /// </summary>
    public void OnDragMove()
    {
        Debug.Log("OnDragMove");

        // ���ĵ굽�������������ֵ
        _mouseToCenterVect = (Vector2)Input.mousePosition - _moveCenter;
        //���ĵ굽������ľ������
        _mouseToCenterDistance = Mathf.Clamp(_mouseToCenterVect.magnitude, 0, 100);
        //���ݾ������ж�ҡ�����ĵ�λ��
        if (_mouseToCenterDistance < _radius)
        {
            //���Ǿ���С�����뾶������ȡ�����Ĺ�һֵ������ģΪ1���������������ĵ�������ľ��룬�������ҡ������Ӧ���ƶ��ķ���;��룬�����ƶ������ƶ����ĵĻ����ϣ����Լ����ƶ����ĵ�����
            _joyCenter.localPosition = _mouseToCenterVect.normalized * _mouseToCenterDistance + _moveCenter;
        }
        else
        {
            //ͬ�ϣ����������޶����ƶ���������
            _joyCenter.localPosition = _mouseToCenterVect.normalized * _radius + _moveCenter;
        }
        //ҡ�����ĵ�X - �ƶ����ĵ�x����ˮƽ�ı仯ֵ������ /100 ����_hor�ڣ�-1��,1��֮��
        _hor = (_joyCenter.localPosition.x - _moveCenter.x) / 100;
        //ҡ�����ĵ�Y - �ƶ����ĵ�Y���Ǵ�ֱ�ı仯ֵ������ /100 ����_hor�ڣ�-1��,1��֮��
        _ver = (_joyCenter.localPosition.y - _moveCenter.y) / 100;

        //�ǶȾ��� ���ĵ굽����������� �� 2Dƽ��Y��������ļнǣ�����ֻ����ʾ0����180��
        _rotAngle = Vector3.Angle(_mouseToCenterVect, Vector3.up);

        //�������_hor���������ж�ҡ��������λ���ƶ�������໹���Ҳ࣬Ȼ���޸Ķ�������ʾ��0����360֮��
        if (_hor < 0)
        {
            _rotAngle = 360 - _rotAngle;
        }
        //Vector3.forward ��Vector3.upΪ������ת�ᣬ��ת_rotAngle�ȣ������������ǵ���ת����
        _forwardTarget = Quaternion.AngleAxis(_rotAngle, Vector3.up) * Vector3.forward;
    }

    /// <summary>
    /// �϶�����
    /// </summary>
    public void OnDragEnd()
    {
        Debug.Log("OnDragEnd");

        //ˮƽ�ƶ�ֵ����
        _hor = 0;
        //��ֱ�ƶ�ֵ����
        _ver = 0;
        //����ҡ��
        _joyBg.gameObject.SetActive(false);
        _joyCenter.gameObject.SetActive(false);

    }
    /// ����
    /// </summary>
    private Transform _player;
    /// <summary>
    /// Ŀ�곯��
    /// </summary>
    private Vector3 _forwardTarget;

    // Update is called once per frame
    void Update()
    {
        //ֻ����ˮƽ���ߴ�ֱֵ����0������£����ǲ��ƶ�����ת
        if (Mathf.Abs(_hor) > 0 || Mathf.Abs(_ver) > 0)
        {
            //Mathf.Clamp(_mouseToCenterDistance/100,0,1)������ҡ�����ĵ��ƶ������ж��ٶ�
            //new Vector3(0,0,0.1f*Mathf.Clamp(_mouseToCenterDistance/100,0,1))����Z�᷽�����ƶ�Mathf.Clamp(_mouseToCenterDistance/100,0,1)�ľ��룬�޶��ڣ�0-1���Ĵ�С
            //��ͣ����Z�᷽�����ƶ�Mathf.Clamp(_mouseToCenterDistance/100,0,1)�ľ���
            _player.position += _player.TransformDirection(new Vector3(0, 0, 0.1f * Mathf.Clamp(_mouseToCenterDistance / 100, 0, 1)));
            //����ҡ�˵���ת�������ǵĳ���
            _player.forward = _forwardTarget;

        }
    }
}

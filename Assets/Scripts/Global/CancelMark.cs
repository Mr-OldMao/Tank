using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 当前对象被摧毁自动取消地图标记
/// </summary>
public class CancelMark : MonoBehaviour {
 
	//被销毁后取消地图标记
	private void OnDestroy()
    {
		MapCreate.GetInstance.RemoveCurPosUsed(transform.position); 
	}

}

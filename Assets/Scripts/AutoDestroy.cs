using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动销毁脚本
/// </summary>
public class AutoDestroy : MonoBehaviour {
	[Tooltip("n秒过后自动销毁")]
	public float destoryTimer = 3f;
	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, destoryTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

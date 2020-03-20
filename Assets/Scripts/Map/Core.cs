using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 鸟窝逻辑
/// </summary>
public class Core : MonoBehaviour {
	[Tooltip("鸟窝爆炸图片")]
	public Sprite coreDestory;
	[Tooltip("鸟窝爆炸特效")]
	public GameObject go_Destory;
	[Tooltip("鸟窝炸了吗？")]
	public bool isCoreDie = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// 鸟窝爆炸
	/// </summary>
	private void CoreDestory()
	{
		GetComponent<SpriteRenderer>().sprite = coreDestory;
		Instantiate(go_Destory, transform.position, transform.rotation);
		Debug.Log("GG");
		//游戏结束
		isCoreDie = true; 
		UIManager.isGameOver = true;
	}
}

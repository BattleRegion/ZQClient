using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
	public enum AttrType
	{
		UnDefine = -1,
		Yellow = 0,
		Green = 1,
		Blue = 2,
		Red = 3
	}

	public int X = 0;

	public int Y = 0;

	public List<Sprite> TileSPrite = new List<Sprite>();

	public SpriteRenderer SRender;

	public Block CurBlock;

	public AttrType CurAttrType = AttrType.UnDefine;
	
	void Awake()
	{
		int attTypeInt = Random.Range(0, 4);
		CurAttrType = (AttrType)attTypeInt;
		SRender.sprite = TileSPrite[attTypeInt];
		SRender.sortingOrder = 1;
	}
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(0.01f,0.01f,0.01f);
	}

	public void SetPos(int x, int y)
	{
		name = "Tile_" + x + "_" + y;
		X = x;
		Y = y;
		transform.localPosition = getRealPositionByXY(X, Y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Bump()
	{
		DestroyImmediate(gameObject);	
	}

	public void MoveDown(int count, FightManager fightManager, Action callback)
	{
		//todo 需要动画
		Block cBlock = CurBlock;
		cBlock.CurTile = null;
		int x = X;
		int y = Y - count;
		Block b = fightManager.GetBlockByPos(x, y);
		b.CurTile = this;
		Hashtable moveConf = new Hashtable();
		moveConf.Add("position", getRealPositionByXY(x, y));
		moveConf.Add("time", FightManager.TileMoveTime);
		moveConf.Add("easetype", iTween.EaseType.easeOutBack);
		moveConf.Add("oncompletetarget",gameObject);
		moveConf.Add("oncomplete","moveDownEnd");
		Dictionary<string, object> oncompleteparams = new Dictionary<string, object>();
		oncompleteparams.Add("x",x);
		oncompleteparams.Add("y",y);
		oncompleteparams.Add("callback",callback);
		moveConf.Add("oncompleteparams", oncompleteparams);
		iTween.MoveTo(gameObject, moveConf);
	}

	void moveDownEnd(object oncompleteparams)
	{
		Dictionary<string, object> ht =  (Dictionary<string, object>)oncompleteparams;
		int x = (int) ht["x"];
		int y = (int) ht["y"];
		Action callback = (Action) ht["callback"];
		SetPos(x, y);
		callback.Invoke();
	}
	
	Vector3 getRealPositionByXY(int x, int y)
	{
		float xPos = -2.1f + x * 0.84f;
		float yPos = -2.9f + y * 0.84f;
		return new Vector3(xPos, yPos, 0);
	}
}

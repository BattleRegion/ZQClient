﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		X = x;
		Y = y;
		float xPos = -2.1f + x * 0.84f;
		float yPos = -2.9f + y * 0.84f;
		transform.localPosition = new Vector3(xPos, yPos, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Bump()
	{
		DestroyImmediate(gameObject);	
	}

	public void Swap(Tile t)
	{
		
	}
}
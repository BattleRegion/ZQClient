using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{

	public GameObject TilesRoot;

	private int width = 6;
	private int height = 5;

	public Dictionary<String, Block> CurBlocks = new Dictionary<String, Block>();

	public Camera CurCamera;

	private bool inputEnable = false;
	
	// Use this for initialization
	void Start ()
	{
		ResetMapTile(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (inputEnable)
		{
			Ray ray = CurCamera.ScreenPointToRay(Input.mousePosition);
#if UNITY_EDITOR
			DealPcInput(ray);
#else
			DealMobileInput();
		#endif	
		}
	}

	void DealPcInput(Ray ray)
	{
		if (Input.GetMouseButtonDown(0))
		{
			SelectTile(HitTile(ray));
		}
		else if (Input.GetMouseButtonUp(0))
		{
			ActionTile(HitTile(ray));
		}
	}

	Tile HitTile(Ray ray)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
		if (hits.Length > 0)
		{
			return hits[0].transform.gameObject.GetComponent<Tile>();
		}
		return null;
	}
	
	
	void DealMobileInput()
	{
		
	}
	
	void InitBlocks()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int x = i;
				int y = j;
				Block b = new Block(x, y);
				string blockName = "Block_" +  x + "_" + y;
				b.CurTile = InitTile(x, y);
				CurBlocks.Add(blockName, b);
			}
		}

		SetInputEnable(true);
	}

	private Tile selectedTile = null;
	
	void SelectTile(Tile tile)
	{
		selectedTile = tile;
	}

	void ActionTile(Tile tile)
	{
		if (selectedTile && tile)
		{
			if (selectedTile == tile)
			{
				Bump();
			}
			else
			{
				if (InCrossRange(selectedTile, tile))
				{
					Swap(tile);
				}
			}	
		}
		else
		{
			Log.Error("selectedTile none or tile none");
		}
	}

	Block GetBlockByTile(Tile t)
	{
		return GetBlockByPos(t.X, t.Y);
	}

	Block GetBlockByPos(int x, int y)
	{
		string blockName = "Block_" +  x + "_" + y;
		return CurBlocks[blockName];
	}
	
	void Bump()
	{
		SetInputEnable(false);
		Log.Info(String.Format("bump tile {0}",selectedTile));
		Block b = GetBlockByTile(selectedTile);
		b.CurTile = null;
		selectedTile.Bump();
		selectedTile = null;
		MapTileDownAnimation();
	}

	void Swap(Tile tile)
	{
		SetInputEnable(false);
		Log.Info(String.Format("change tile {0} to {1}",selectedTile, tile));	
		selectedTile.Swap(tile);
	}
	
	bool InCrossRange(Tile from, Tile to)
	{
		if ((from.X == to.X) && (from.Y == to.Y + 1 || from.Y == to.Y - 1))
		{
			return true;
		}
		
		if ((from.Y == to.Y) && (from.X == to.X + 1 || from.X == to.X - 1))
		{
			return true;
		}
		
		return false;
	}

	void MapTileDownAnimation()
	{
		Log.Info(String.Format("MapTileDownAnimation"));
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Block b = GetBlockByPos(i, j);
				if (b.CurTile)
				{
					int count = findDownCount(b);
					if (count > 0)
					{
						Log.Info(string.Format("tile {0} need down count {1}",b.CurTile.name,count.ToString()));
						b.CurTile.MoveDown(count,this);
					}	
				}
			}
		}
	}

	int findDownCount(Block b)
	{
		int downCount = 0;
		int y = b.Y;
		for (int i = y; i >= 0; i--)
		{
			Block downB = GetBlockByPos(b.X, i);
			if (downB.CurTile == null)
			{
				downCount++;
			}
		}
		return downCount;
	}
	
	void ResetMapTile(bool first = false)
	{
		Log.Info(String.Format("ResetMapTile {0}",first));
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int x = i;
				int y = j;
				Block b;
				if (first)
				{
					b = new Block(x, y);
					b.CurTile = InitTile(x, y);
					string blockName = "Block_" +  x + "_" + y;
					CurBlocks.Add(blockName, b);	
				}
				else
				{
					b = GetBlockByPos(x, y);
					if (b.CurTile == null)
					{
						b.CurTile = InitTile(x, y);
					}
				}
			}
		}

		SetInputEnable(true);
	}

	Tile InitTile(int x, int y)
	{
		string tileName = "Tile_" +  x + "_" + y;
		Tile t = Instantiate(ResourceHelper.GetInstance().LoadPrefab("Fight/Tile")).GetComponent<Tile>();
		t.gameObject.name = tileName;
		t.SetPos(x, y);
		t.transform.parent = TilesRoot.transform;
		return t;
	}

	void SetInputEnable(bool enable)
	{
		Log.Info(String.Format("SetInputEnable {0}",enable));
		inputEnable = enable;
	}
}

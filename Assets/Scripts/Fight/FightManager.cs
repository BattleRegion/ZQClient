using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
	public enum Direction
	{
		Right = 0,
		Up = 1,
		Left = 2,
		Down = 3
	}

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
		if (Input.touchCount > 0)
		{
			Log.Info(Input.GetTouch(0).position.ToString());		
		}

		if (Input.GetMouseButtonDown(0))
		{
			Log.Info(Input.mousePosition.ToString());
		}
//		if (inputEnable)
//		{
//			Ray ray = CurCamera.ScreenPointToRay(Input.mousePosition);
//#if UNITY_EDITOR
//			DealPcInput(ray);
//#else
//			DealMobileInput();
//		#endif	
//		}
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
				Vector3 from = Vector3.right;
				Vector3 to = new Vector3(tile.X - selectedTile.X, tile.Y - selectedTile.Y ,0);
				float angle = Angle_360(from, to);
				Log.Info(from.ToString());
				Log.Info(to.ToString());
				Log.Info(Angle_360(from, to).ToString());
				if (angle > 45 && angle <= 135)
				{
					Log.Info("up");
				}
				else if (angle > 135 && angle <= 225)
				{
					Log.Info("left");
				}
				else if (angle > 225 && angle <= 315)
				{
					Log.Info("down");
				}
				else if ((angle > 315 && angle <= 360) || (angle >= 0 && angle <= 45))
				{
					Log.Info("right");
				}
			}	
		}
		else
		{
			Log.Error("selectedTile none or tile none");
		}
	}

	float Angle_360(Vector3 from_, Vector3 to_)
	{
		Vector3 v3 = Vector3.Cross(from_, to_);
		if (v3.z > 0)
			return Vector3.Angle(from_, to_);
		else
			return 360 - Vector3.Angle(from_, to_);
	}


	Block GetBlockByTile(Tile t)
	{
		return GetBlockByPos(t.X, t.Y);
	}

	public Block GetBlockByPos(int x, int y)
	{
		string blockName = "Block_" +  x + "_" + y;
		return CurBlocks[blockName];
	}
	
	void Bump()
	{
		SetInputEnable(false);
		Log.Info(String.Format("bump tile {0}",selectedTile));
		Block b = selectedTile.CurBlock;
		b.CurTile = null;
		selectedTile.Bump();
		selectedTile = null;
		MapTileDownAnimation();
	}

	//direction 
	void Swap(Tile tile)
	{
		SetInputEnable(false);
		Log.Info(String.Format("change tile {0} to {1}",selectedTile, tile));	
		selectedTile.Swap(tile, () =>
		{
			SetInputEnable(true);
		});
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
		Dictionary<Block, int> needDown = new Dictionary<Block, int>();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Block b = GetBlockByPos(i, j);
				if (b.CurTile)
				{
					int count = FindDownCount(b);
					if (count > 0)
					{
						Log.Info(string.Format("tile {0} need down count {1}", b.CurTile.name, count.ToString()));
						needDown.Add(b,count);
					}	
				}
			}
		}

		int downCount = needDown.Count;
		
		foreach (var item in needDown)
		{
			Block b = item.Key;
			int count = item.Value;
			b.CurTile.MoveDown(count,this, () =>
			{
				downCount--;
				Log.Info(String.Format("move down end {0}",downCount));
				if (downCount == 0)
				{
					ResetMapTile();
				}
			});
		}
	}

	int FindDownCount(Block b)
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

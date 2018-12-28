using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{

    private Tile curTile;
    public Tile CurTile 
    {
        get
        {
            return curTile;
        }
        set
        {
            curTile = value;
            if (curTile)
            {
                curTile.CurBlock = this;   
            }
        }  
    }    

    public int X;

    public int Y;

    public Block(int x, int y)
    {
        X = x;
        Y = y;
    }
}

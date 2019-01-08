using System;
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
            if (curTile!=null)
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

    public void SwapTile(Block swapBlock, Action callback)
    {
        Tile swapTile = swapBlock.CurTile;

        swapBlock.CurTile = CurTile;
        CurTile = swapTile;
        
        CurTile.SetPos(X, Y);
        swapBlock.CurTile.SetPos(swapBlock.X, swapBlock.Y);
        
        callback.Invoke();
    }
}

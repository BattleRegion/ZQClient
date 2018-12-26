using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{

    public Tile CurTile = null;

    public int X;

    public int Y;

    public Block(int x, int y)
    {
        X = x;
        Y = y;
    }
}

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class BulletMatrix
{
    public int Height;
    public int Width;

    private IBullet[,] grid;

    public BulletMatrix(int height, int width)
    {
        this.Height = height;
        this.Width = width;

        grid = new IBullet[this.Height, this.Width];
    }

    
}

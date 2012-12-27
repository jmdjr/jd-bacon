using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

[Serializable]
public class Position2D
{
    public int X;
    public int Y;

    public Position2D(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        Position2D o = obj as Position2D;

        if (o != null)
        {
            return o.X == this.X && o.Y == this.Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "<X: " + this.X + ", Y: " + this.Y + ">";
    }
}
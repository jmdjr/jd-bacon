using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public class Frame : JDMonoGuiBehavior
{
    private GameObject[,] grid;

    private BulletMatrix frame;

    public Position2D GridDimensions = new Position2D(10, 10);

    public override void Awake()
    {
        frame = new BulletMatrix(GridDimensions.Y, GridDimensions.X);
        grid = new GameObject[GridDimensions.Y, GridDimensions.X];

        base.Awake();
    }
}

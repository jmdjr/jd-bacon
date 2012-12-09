using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public class BulletMatrix
{
    public int Height;
    public int Width;

    private int totalToMatch = 3;

    private JDBullet[,] grid;
    private Random r;
    private int min;
    private int max;
    public BulletMatrix(int height, int width)
    {
        this.Height = height;
        this.Width = width;

        r = new Random(11);
        min = 0;
        max = BulletFactory.NumberOfLoadedBullets;
        
        grid = new JDBullet[this.Height, this.Width];
    }

    private delegate void GridStepper(int x, int y);

    // will run through and fill the grid with a random collection of the bullets.
    public void Load()
    {
        StepThroughGrid(
            ((i, j) =>
            {
                //int bulletIndex = Random.Range(min, max);
                int bulletIndex = r.Next(min, max);

                if (grid[i, j] == null)
                {
                    grid[i, j] = BulletFactory.Instance.SpawnBullet(bulletIndex);
                }
            })
        );
    }
    public void UnLoad()
    {
        StepThroughGrid(
            ((i, j) =>
            {
                dropBullet(i, j);
            })
        );
    }

    public List<Vector2> CollectMatchedBullets()
    {
        List<Vector2> collectedMatchedBullets = new List<Vector2>();

        StepThroughGrid(
            ((i, j) =>
            {
                if (isVerticalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new Vector2(i + streak, j));
                        ++streak;
                    }
                }

                if (isHorizontalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new Vector2(i, j + streak));
                        ++streak;
                    }
                }
            }));
        ;

        return collectedMatchedBullets.OrderBy(v => v.x).Distinct().ToList();
    }
    public void DropMatchedBullets(List<Vector2> collectedBullets)
    {
        foreach (Vector2 pos in collectedBullets)
        {
            dropBullet((int)pos.x, (int)pos.y);
        }
    }
    public void ShiftItemsDown(List<Vector2> collectedBullets) 
    {
        int i = 0;
        int j = 0;

        foreach (Vector2 pos in collectedBullets)
        {
            i = (int)pos.x;
            j = (int)pos.y;
            bubblePositionUp(ref i, j);
            SpawnBullet(i, j);
        }
    }
    
    public void SwapPositions(int i, int j, int i2, int j2)
    {
        if (i == i2 && j == j2 || i != i2 && j != j2)
        {
            // one of the two components must be the same, forcing horizontal or vertical positions only
            return;
        }

        JDBullet holder = grid[i, j];

        grid[i, j] = grid[i2, j2];
        grid[i2, j2] = holder;
    }
    
    private void bubblePositionUp(ref int i, int j)
    {
        while (i > 0)
        {
            SwapPositions(i, j, --i, j);
            this.Debug_PrintBulletMatrix();
            System.Threading.Thread.Sleep(50);
            Console.Clear();
        }
    }

    private bool isVerticalStreak(int i, int j)
    {
        int matches = 1;

        JDBullet toMatch = grid[i, j];

        int steps = 1;
        ++i;

        while (i < this.Height && steps < this.totalToMatch)
        {

            if (grid[i, j].BulletType == toMatch.BulletType)
            {
                ++matches;
            }

            ++steps;
            ++i;
        }

        return matches >= this.totalToMatch;
    }
    private bool isHorizontalStreak(int i, int j)
    {
        int matches = 1;

        JDBullet toMatch = grid[i, j];

        int steps = 1;
        ++j;

        while (j < this.Width && steps < this.totalToMatch)
        {

            if (grid[i, j].BulletType == toMatch.BulletType)
            {
                ++matches;
            }

            ++steps;
            ++j;
        }

        return matches >= this.totalToMatch;
    }
    private void SpawnBullet(int i, int j)
    {
        if (grid[i, j] == null)
        {
            int bulletIndex = r.Next(min, max);
            grid[i, j] = BulletFactory.Instance.SpawnBullet(bulletIndex);
        }
    }

    #region Utility
    private void dropBullet(int i, int j)
    {
        BulletFactory.Instance.DestroyBullet(grid[i, j]);
        grid[i, j] = null;
    }
    private void StepThroughGrid(GridStepper onColumn) { StepThroughGrid(null, onColumn, null); }
    private void StepThroughGrid(GridStepper enterRow, GridStepper onColumn) { StepThroughGrid(enterRow, onColumn, null); }
    private void StepThroughGrid(GridStepper enterRow, GridStepper onColumn, GridStepper endRow)
    {
        for (int i = 0; i < this.Height; ++i)
        {
            if (enterRow != null)
            {
                enterRow(i, 0);
            }

            for (int j = 0; j < this.Width; ++j)
            {
                if (onColumn != null)
                {
                    onColumn(i, j);
                }
            }

            if (endRow != null)
            {
                endRow(i, this.Width);
            }
        }
    }
    public void Debug_PrintBulletMatrix()
    {
        Console.Write("  ");
        for (int j = 0; j < this.Width; ++j)
        {
            Console.Write((char)('A' + j) + " ");
        }

        Console.Write('\n');

        StepThroughGrid(
            ((i, j) => { Console.Write((i + 1) + " "); }),
            ((i, j) =>
            {
                if (grid[i, j] != null)
                {
                    Console.ForegroundColor = grid[i, j].Debug_Color;
                    Console.Write(grid[i, j].bulletDebugChar);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(" ");
                }
                Console.Write(" ");
            }),
            ((i, j) => { Console.Write("\n"); })
        );
    }
    #endregion Utility
}

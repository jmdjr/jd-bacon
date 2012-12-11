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
    public bool enablePrinting = false;

    private JDBullet[,] grid;
    private Random r;
    private int min;
    private int max;
    public BulletMatrix(int height, int width)
    {
        this.Height = height;
        this.Width = width;

        r = new Random(10);
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

    public List<Tuple<int, int>> CollectMatchedBullets()
    {
        List<Tuple<int, int>> collectedMatchedBullets = new List<Tuple<int, int>>();

        StepThroughGrid(
            ((i, j) =>
            {
                if (isVerticalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new Tuple<int, int>(i + streak, j));
                        ++streak;
                    }
                }

                if (isHorizontalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new Tuple<int, int>(i, j + streak));
                        ++streak;
                    }
                }
            }));
        ;

        return collectedMatchedBullets.OrderBy(v => v.Item1).Distinct().ToList();
    }
    public void DropMatchedBullets(List<Tuple<int, int>> collectedBullets)
    {
        List<JDBullet> uniqueBullets = new List<JDBullet>();

        foreach (Tuple<int, int> pos in collectedBullets)
        {
            if (!uniqueBullets.Any(bullet => bullet.BulletType == grid[pos.Item1, pos.Item2].BulletType))
            {
                uniqueBullets.Add(grid[pos.Item1, pos.Item2]);
            }

            dropBullet(pos.Item1, pos.Item2);
        }
        
        foreach (JDBullet bullet in uniqueBullets)
        {
            bullet.ReportStatistics(JDIStatTypes.UNIQUES, 1);
        }
    }
    public void ShiftItemsDown(List<Tuple<int, int>> collectedBullets) 
    {
        int i = 0;
        int j = 0;

        foreach (Tuple<int, int> pos in collectedBullets)
        {
            i = pos.Item1;
            j = pos.Item2;
            bubblePositionUp(ref i, j);
            SpawnBullet(i, j);
        }
    }
    public bool CanSwapPositions(int i, int j, int i2, int j2)
    {
            Tuple<int, int> first = new Tuple<int, int>(i, j);
            Tuple<int, int> second = new Tuple<int, int>(i2, j2);
            return CanSwapPositions(first, second);
    }
    public bool CanSwapPositions(Tuple<int, int> first, Tuple<int, int> second)
    {
        int i = first.Item1;
        int j = first.Item2;

        Tuple<int, int>[] compass = new Tuple<int, int>[]
        {
            new Tuple<int, int>(i + 1, j),
            new Tuple<int, int>(i - 1, j),
            new Tuple<int, int>(i, j + 1),
            new Tuple<int, int>(i, j - 1)
        };

        return !IsOutOfBounds(first) && !IsOutOfBounds(second) && first != second && compass.Any(a => a.Equals(second));
    }
    private bool IsOutOfBounds(Tuple<int, int> position)
    {
        return position.Item1 < 0 || position.Item1 >= this.Height
            || position.Item2 < 0 || position.Item2 >= this.Width;
    }
    public void SwapPositions(Tuple<int, int> first, Tuple<int, int> second)
    {
        SwapPositions(first.Item1, first.Item2, second.Item1, second.Item2);
    }
    public void SwapPositions(int i, int j, int i2, int j2)
    {
        if (!CanSwapPositions(i, j, i2, j2)) //i == i2 && j == j2 || i != i2 && j != j2)
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
            if (enablePrinting)
            {
                Console.SetCursorPosition(0, 0);
                this.Debug_PrintBulletMatrix();
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    private bool isVerticalStreak(int i, int j)
    {
        int matches = 1;

        JDBullet toMatch = grid[i, j];

        int steps = 1;
        ++i;

        if (toMatch != null)
        {
            while (i < this.Height && steps < this.totalToMatch)
            {

                if (grid[i, j] != null && grid[i, j].BulletType == toMatch.BulletType)
                {
                    ++matches;
                }

                ++steps;
                ++i;
            }
        }
        return matches >= this.totalToMatch;
    }
    private bool isHorizontalStreak(int i, int j)
    {
        int matches = 1;

        JDBullet toMatch = grid[i, j];

        int steps = 1;
        ++j;

        if (toMatch != null)
        {
            while (j < this.Width && steps < this.totalToMatch)
            {

                if (grid[i, j] != null && grid[i, j].BulletType == toMatch.BulletType)
                {
                    ++matches;
                }

                ++steps;
                ++j;
            }
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
    public Tuple<int, int> CommandToPosition(string command)
    {
        char[] commandSplit = command.ToCharArray();

        return new Tuple<int,int>(CharToNumber(command[1]), CharToNumber(command[0]));

    }
    private int CharToNumber(char command) 
    {
        if (command >= '0' && command <= '9')
        {
            return int.Parse(command.ToString());
        }
        else if (command >= 'A' && command <= 'Z')
        {
            return 10 + (command - 'A');
        }

        return -1;
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
    private string Debug_CoordChar(int index)
    {
        if (index < 10 && index >= 0) return index.ToString();
        if (index >= 10 && index < 36) return ((char)('A' + index)).ToString();
        return "?";
    }
    public void Debug_PrintBulletMatrix()
    {
        Console.Write("  ");
        for (int j = 0; j < this.Width; ++j)
        {
            Console.Write(Debug_CoordChar(j) + " ");
        }

        Console.WriteLine();

        StepThroughGrid(
            ((i, j) => { Console.Write(Debug_CoordChar(i) + " "); }),
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
            ((i, j) =>
            {
                if (GameStatistics.Instance.HasStatisticByIndex(i))
                {
                    Console.Write(GameStatistics.Instance.GetStatisticNameByIndex(i) + ": " + GameStatistics.Instance.GetStatisticValueByIndex(i));
                }

                Console.WriteLine();
            })
        );
    }
    #endregion Utility
}

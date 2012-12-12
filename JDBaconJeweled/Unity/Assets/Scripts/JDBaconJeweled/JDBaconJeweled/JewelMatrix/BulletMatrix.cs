﻿using UnityEngine;
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

        r = new Random();
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
    public List<KeyValuePair<int, int>> CollectMatchedBullets()
    {
        List<KeyValuePair<int, int>> collectedMatchedBullets = new List<KeyValuePair<int, int>>();

        StepThroughGrid(
            ((i, j) =>
            {
                if (isVerticalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new KeyValuePair<int, int>(i + streak, j));
                        ++streak;
                    }
                }

                if (isHorizontalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new KeyValuePair<int, int>(i, j + streak));
                        ++streak;
                    }
                }
            }));
        ;

        return collectedMatchedBullets.OrderBy(v => v.Key).Distinct().ToList();
    }
    public bool CanMatchMore()
    {
        bool thereIsAMatch = false;
        StepThroughGrid((i, j) => {
            if (thereIsAMatch)
            {
                return;
            }

            if (positionCanMakeMatches(i, j))
            {
                thereIsAMatch = true;
            }
        });

        return thereIsAMatch;
    }
    private bool positionCanMakeMatches(int i, int j)
    {
        return positionCanMakeMatches(new KeyValuePair<int, int>(i, j));
    }
    private bool positionCanMakeMatches(KeyValuePair<int, int> position)
    {
        int i = position.Key, j = position.Value;
        KeyValuePair<int, int>[] compass = new KeyValuePair<int, int>[]
        {
            new KeyValuePair<int, int>(i + 1, j),
            new KeyValuePair<int, int>(i - 1, j),
            new KeyValuePair<int, int>(i, j + 1),
            new KeyValuePair<int, int>(i, j - 1)
        };

        foreach (KeyValuePair<int, int> direction in compass)
        {
            if (CanSwapPositions(direction, position))
            {
                SwapPositions(position, direction);

                if (isStreak(position) || isStreak(direction))
                {
                    SwapPositions(position, direction);
                    return true;
                }
            }
        }

        return false;
    }

    public void DropMatchedBullets(List<KeyValuePair<int, int>> collectedBullets)
    {
        List<JDBullet> uniqueBullets = new List<JDBullet>();

        foreach (KeyValuePair<int, int> pos in collectedBullets)
        {
            if (!uniqueBullets.Any(bullet => bullet.BulletType == grid[pos.Key, pos.Value].BulletType))
            {
                uniqueBullets.Add(grid[pos.Key, pos.Value]);
            }

            dropBullet(pos.Key, pos.Value);
        }
        
        foreach (JDBullet bullet in uniqueBullets)
        {
            bullet.ReportStatistics(JDIStatTypes.UNIQUES, 1);
        }
    }
    public void ShiftItemsDown(List<KeyValuePair<int, int>> collectedBullets) 
    {
        int i = 0;
        int j = 0;

        foreach (KeyValuePair<int, int> pos in collectedBullets)
        {
            i = pos.Key;
            j = pos.Value;
            bubblePositionUp(ref i, j);
            SpawnBullet(i, j);
        }
    }
    public bool CanSwapPositions(int i, int j, int i2, int j2)
    {
            KeyValuePair<int, int> first = new KeyValuePair<int, int>(i, j);
            KeyValuePair<int, int> second = new KeyValuePair<int, int>(i2, j2);
            return CanSwapPositions(first, second);
    }
    public bool CanSwapPositions(KeyValuePair<int, int> first, KeyValuePair<int, int> second)
    {
        int i = first.Key;
        int j = first.Value;

        KeyValuePair<int, int>[] compass = new KeyValuePair<int, int>[]
        {
            new KeyValuePair<int, int>(i + 1, j),
            new KeyValuePair<int, int>(i - 1, j),
            new KeyValuePair<int, int>(i, j + 1),
            new KeyValuePair<int, int>(i, j - 1)
        };
        bool areNotEqual = !(first.Key == second.Key && first.Value == second.Value);
        return !IsOutOfBounds(first) && !IsOutOfBounds(second) && areNotEqual && compass.Any(a => a.Equals(second));
    }
    private bool IsOutOfBounds(KeyValuePair<int, int> position)
    {
        return position.Key < 0 || position.Key >= this.Height
            || position.Value < 0 || position.Value >= this.Width;
    }
    public void SwapPositions(KeyValuePair<int, int> first, KeyValuePair<int, int> second)
    {
        SwapPositions(first.Key, first.Value, second.Key, second.Value);
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
#if DEBUG || RELEASE
                Console.SetCursorPosition(0, 0);
                this.Debug_PrintBulletMatrix();
                System.Threading.Thread.Sleep(100);
#endif
            }
        }
    }

    private bool isStreak(KeyValuePair<int, int> pos)
    {
        return isStreak(pos.Key, pos.Value);
    }
    private bool isStreak(int i, int j)
    {
        return isVerticalStreak(i, j) || isHorizontalStreak(i, j);
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
    public KeyValuePair<int, int> CommandToPosition(string command)
    {
        char[] commandSplit = command.ToCharArray();

        return new KeyValuePair<int, int>(CharToNumber(command[1]), CharToNumber(command[0]));

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
        if (index >= 10 && index < 36) return ((char)('A' + index - 10)).ToString();
        return "?";
    }
    public void Debug_PrintBulletMatrix()
    {
#if DEBUG || RELEASE
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
#endif
    }
    #endregion Utility
}

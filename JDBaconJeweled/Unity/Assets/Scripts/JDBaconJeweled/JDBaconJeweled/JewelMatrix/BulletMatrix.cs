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
    private delegate void GridStepper(int x, int y);

    public BulletMatrix(int height, int width)
    {
        this.Height = height;
        this.Width = width;

        r = new Random();
        min = 0;
        max = BulletFactory.NumberOfLoadedBullets;
        
        grid = new JDBullet[this.Height, this.Width];
    }

    #region Checks
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
        return positionCanMakeMatches(new Position2D(j, i));
    }
    private bool positionCanMakeMatches(Position2D position)
    {
        Position2D[] compass = createCompass(position);

        foreach (Position2D direction in compass)
        {
            if (CanSwapPositions(direction, position))
            {
                SwapPositions(position, direction);
                bool swapSuccess = CollectMatchedBullets().Count > 0;
                SwapPositions(position, direction);

                if (swapSuccess)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public bool IsBadSwap(Position2D first, Position2D second)
    {
        SwapPositions(first, second);

#if DEBUG || RELEASE
        if (this.enablePrinting)
        {
            Debug_PrintAndSleep();
        }
#endif
        bool anyMatches = CollectMatchedBullets().Count > 0;
        SwapPositions(first, second);

#if DEBUG || RELEASE
        if (this.enablePrinting)
        {
            Debug_PrintAndSleep();
        }
#endif
        return anyMatches;
    }
    public bool CanSwapPositions(int i, int j, int i2, int j2)
    {
        Position2D first = new Position2D(j, i);
        Position2D second = new Position2D(j2, i2);
        return CanSwapPositions(first, second);
    }
    public bool CanSwapPositions(Position2D first, Position2D second)
    {
        Position2D[] compass = createCompass(first);
        bool areNotEqual = !(first.Equals(second));
        return !IsOutOfBounds(first) && !IsOutOfBounds(second) && areNotEqual && compass.Any(a => a.Equals(second));
    }
    private bool IsOutOfBounds(Position2D position)
    {
        return position.Y < 0 || position.Y >= this.Height
            || position.X < 0 || position.X >= this.Width;
    }
    private bool isStreak(Position2D pos)
    {
        return isStreak(pos.Y, pos.X);
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
    #endregion
    
    #region Accessors
    #endregion

    #region Load and Balancing
    // will run through and fill the grid with a random collection of the bullets.
    public void Load(bool printEnabled)
    {
        this.enablePrinting = printEnabled;

        StepThroughGrid(
            ((i, j) =>
            {
                //int bulletIndex = Random.Range(min, max);
                int bulletIndex = r.Next(min, max);

                if (grid[i, j] == null)
                {
                    grid[i, j] = BulletFactory.Instance.SpawnBullet(bulletIndex);
                    Debug_PrintAndSleep();
                }
            })
        );

        BalanceGrid(printEnabled, true);
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
    public List<Position2D> CollectMatchedBullets()
    {
        List<Position2D> collectedMatchedBullets = new List<Position2D>();

        StepThroughGrid(
            ((i, j) =>
            {
                if (isVerticalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new Position2D(j, i + streak));
                        ++streak;
                    }
                }

                if (isHorizontalStreak(i, j))
                {
                    int streak = 0;
                    while (streak < this.totalToMatch)
                    {
                        collectedMatchedBullets.Add(new Position2D(j + streak, i));
                        ++streak;
                    }
                }
            }));
        ;

        return collectedMatchedBullets.OrderBy(v => v.Y).Distinct().ToList();
    }
    public void DropMatchedBullets(List<Position2D> collectedBullets)
    {
        List<JDBullet> uniqueBullets = new List<JDBullet>();

        foreach (Position2D pos in collectedBullets)
        {
            JDBullet gridBullet = grid[pos.Y, pos.X];
            if (gridBullet != null && !uniqueBullets.Any(bullet => bullet.BulletType == gridBullet.BulletType))
            {
                uniqueBullets.Add(gridBullet);
            }

            dropBullet(pos.Y, pos.X);
        }

        foreach (JDBullet bullet in uniqueBullets)
        {
            bullet.ReportStatistics(JDIStatTypes.UNIQUES, 1);
        }
    }
    public void ShiftItemsDown(List<Position2D> collectedBullets)
    {
        int i = 0;
        int j = 0;

        foreach (Position2D pos in collectedBullets)
        {
            i = pos.Y;
            j = pos.X;
            bubblePositionUp(ref i, j);
            SpawnBullet(i, j);
            Debug_PrintAndSleep();
        }
    }
    public void SwapPositions(Position2D first, Position2D second)
    {
        SwapPositions(first.Y, first.X, second.Y, second.X);
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
    public void BalanceGrid(bool printEnabled, bool resetStatistics)
    {
        this.enablePrinting = printEnabled;
        List<Position2D> matches = this.CollectMatchedBullets();
        do
        {
            this.DropMatchedBullets(matches);
            this.ShiftItemsDown(matches);
            matches = this.CollectMatchedBullets();
        }
        while (matches.Count > 0);

        if (resetStatistics)
        {
            BulletFactory.Instance.ResetStatistics();
        }
    }
    private void SpawnBullet(int i, int j)
    {
        if (grid[i, j] == null)
        {
            int bulletIndex = r.Next(min, max);
            grid[i, j] = BulletFactory.Instance.SpawnBullet(bulletIndex);
        }
    }
    private void dropBullet(int i, int j)
    {
        BulletFactory.Instance.DestroyBullet(grid[i, j]);
        grid[i, j] = null;
    }
    private void bubblePositionUp(ref int i, int j)
    {
        while (i > 0)
        {
            SwapPositions(i, j, --i, j);
            Debug_PrintAndSleep();
        }
    }
    #endregion

    #region Utility
    private Position2D[] createCompass(Position2D center)
    {
        int i = center.X;
        int j = center.Y;

        return new Position2D[]
        {
            new Position2D(i + 1, j),
            new Position2D(i - 1, j),
            new Position2D(i, j + 1),
            new Position2D(i, j - 1)
        };
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
    #endregion Utility

    #region Debug Functions
    public Position2D CommandToPosition(string command)
    {
        char[] commandSplit = command.ToCharArray();

        return new Position2D(CharToNumber(command[0]), CharToNumber(command[1]));

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
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.Write(" ");
                    Console.ResetColor();
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
    private string Debug_CoordChar(int index)
    {
        if (index < 10 && index >= 0) return index.ToString();
        if (index >= 10 && index < 36) return ((char)('A' + index - 10)).ToString();
        return "?";
    }
    private void Debug_PrintAndSleep()
    {
        if (enablePrinting)
        {
#if DEBUG || RELEASE
            Console.SetCursorPosition(0, 0);
            this.Debug_PrintBulletMatrix();
            System.Threading.Thread.Sleep(100);
#endif
        }
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameStatistics
{
    private Dictionary<string, int> stats;
    private static GameStatistics instance;
    public static GameStatistics Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameStatistics();
            }
            return instance;
        }
    }

    private GameStatistics()
    {
        stats = new Dictionary<string, int>();
    }

    public int GetStatistic(string name)
    {
        if (stats.ContainsKey(name))
        {
            return stats[name];
        }

        return -1;
    }

    public void UpdateStatistic(string name, int updateByAmount)
    {
        if (stats.ContainsKey(name))
        {
            stats[name] += updateByAmount;
        }
    }

    public void CreateStatistic(string name, int initialValue)
    {
        if (!stats.ContainsKey(name))
        {
            stats.Add(name, initialValue);
        }
        else
        {
            stats[name] = initialValue;
        }
    }

    public string GetStatisticNameByIndex(int keyIndex)
    {
        if (keyIndex >= 0 || keyIndex < stats.Keys.Count)
        {
            return stats.Keys.ToArray()[keyIndex];
        }

        return "";
    }

    public int GetStatisticValueByIndex(int keyIndex)
    {
        if (keyIndex >= 0 || keyIndex < stats.Values.Count)
        {
            return stats.Values.ToArray()[keyIndex];
        }

        return -1;
    }

    public bool HasStatisticByIndex(int index)
    {
        return index >= 0 && index < this.stats.Count;
    }
}

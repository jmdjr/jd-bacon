using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameStatistics
{
    static string separator = "||";
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

    private Dictionary<string, int> stats;
    public JDIStatTypes AllowedBulletStat;

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
    public void SetStatistic(string name, int initialValue)
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
        if (HasStatisticByIndex(keyIndex))
        {
            return stats.Keys.ToArray()[keyIndex];
        }

        return "";
    }
    public int GetStatisticValueByIndex(int keyIndex)
    {
        if (HasStatisticByIndex(keyIndex))
        {
            return stats.Values.ToArray()[keyIndex];
        }

        return -1;
    }
    public bool HasStatisticByIndex(int index)
    {
        return index >= 0 && index < this.stats.Count;
    }

    public string SubGroup(string GroupTitle, string Statistic)
    {
        return GroupTitle + separator + Statistic;
    }
    public void EstablishGroup(string GroupTitle, string[] statisticNames)
    {
        foreach (string statName in statisticNames)
        {
            string stat = SubGroup(GroupTitle, statName);
            SetStatistic(stat, 0);
        }
    }
    public void ResetStatisticGroup(string GroupTitle)
    {
        var group = stats.Keys.Where<string>(k => k.StartsWith(GroupTitle + separator));
        foreach (var key in group)
        {
            stats[key] = 0;
        }
    }

    public List<string> GetAllStatNamesThatHave(string statPhrase)
    {
        return stats.Keys.Where(s => s.Contains(statPhrase)).ToList();
    }

    public void UpdateAllStatsThatHave(string statphrase, int updateValue)
    {
        foreach (string statname in GetAllStatNamesThatHave(statphrase))
        {
            this.UpdateStatistic(statname, updateValue);
        }
    }
}

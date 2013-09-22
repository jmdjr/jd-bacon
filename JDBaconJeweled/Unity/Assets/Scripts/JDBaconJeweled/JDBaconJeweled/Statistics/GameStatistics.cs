using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameStatistics : JDISavableObject
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

    private List<Stat> stats;
    public JDIStatTypes AllowedBulletStat;

    private GameStatistics()
    {
        stats = new List<Stat>();
    }
    private Stat getStat(string name)
    {
        return stats.FirstOrDefault(i => i.statName == name);
    }
    public int GetStatistic(string name)
    {
        Stat s = getStat(name);
        if (s != null)
        {
            return s.statValue;
        }

        return -1;
    }
    public void UpdateStatistic(string name, int updateByAmount)
    {
        Stat s = getStat(name);
        if (s != null)
        {
            s.statValue += updateByAmount;
        }
    }
    public void SetStatistic(string name, int initialValue)
    {
        Stat s = getStat(name);
        if (s == null)
        {
            stats.Add(new Stat(name, initialValue));
        }
        else
        {
            s.statValue = initialValue;
        }
    }
    public string GetStatisticNameByIndex(int keyIndex)
    {
        if (HasStatisticByIndex(keyIndex))
        {
            return stats[keyIndex].statName;
        }

        return "";
    }
    public int GetStatisticValueByIndex(int keyIndex)
    {
        if (HasStatisticByIndex(keyIndex))
        {
            return stats[keyIndex].statValue;
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
        var group = stats.Where<Stat>(k => k.statName.Contains(GroupTitle + separator));
        foreach (var key in group)
        {
            key.statValue = 0;
        }
    }

    public List<string> GetAllStatNamesThatHave(string statPhrase)
    {
        return stats
            .Where(i => i.statName.Contains(statPhrase))
            .ToList()
            .ConvertAll<string>(i => i.statName)
            .ToList();
    }

    public void UpdateAllStatsThatHave(string statphrase, int updateValue)
    {
        foreach (string statname in GetAllStatNamesThatHave(statphrase))
        {
            this.UpdateStatistic(statname, updateValue);
        }
    }

    public string SaveData()
    {
        return JDGameUtilz.SerializeObject(stats, "Statistics", typeof(List<Stat>));
    }

    public void LoadData(string savefiletext)
    {
        int rootStart = savefiletext.IndexOf("<Statistics>");

        if (rootStart != -1)
        {
            int rootEnd = savefiletext.IndexOf("</Statistics>") + "</Statistics>".Length;
            string partialText = savefiletext.Substring(rootStart, rootEnd - rootStart);
            stats = (List<Stat>)JDGameUtilz.DeserializeObject(partialText, "Statistics", typeof(List<Stat>), JDGameUtilz.EncodingType.UTF8);
        }
    }

    public string Name
    {
        get { return "Game Statistics"; }
        set { } 
    }

    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.OBJECT; }
    }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        return false;
    }
}

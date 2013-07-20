using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Stat
{
    public string statName;
    public int statValue;
    public Stat() { }
    public Stat(string n, int v)
    {
        statName = n;
        statValue = v;
    }
}

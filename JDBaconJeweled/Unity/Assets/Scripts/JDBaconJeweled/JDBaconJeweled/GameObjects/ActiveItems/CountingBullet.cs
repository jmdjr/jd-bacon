using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CountingBullet : JDMonoBodyBehavior
{
    DynamicText myCount = null;
    GameStatistics stats = null;
    JDBullet reference = null;
    LevelManager level = null;

    public string ManualName = "";

    public override void Awake()
    {
        base.Awake();

        myCount = DynamicText.GetTextMesh(this);
        stats = GameStatistics.Instance;
        level = LevelManager.Instance;
    }

    public override void Update()
    {
        base.Update();
        int bulletStat = stats.GetStatistic(stats.SubGroup(level.CurrentLevelName(), ManualName));
        string statString = string.Format("x{0}", bulletStat.ToString("00"));
        this.myCount.SetText(statString);
    }
}

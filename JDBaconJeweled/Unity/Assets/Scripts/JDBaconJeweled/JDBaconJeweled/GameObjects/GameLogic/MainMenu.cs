using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.IO;

public class MainMenu : JDMenu
{
    public override void MenuUpdate()
    {
    }

    public override void AssignButtonMenus()
    {
        if (this.menuButtons["Exit Game"] != null)
        {
            // should have a confirmation menu to ask if they really want to quit, maybe save too.
        }

        if (this.menuButtons["Options"] != null)
        {
            this.menuButtons["Options"].AssignMenu(navigator.GetMenu("Options Menu"));
        }
        if (this.menuButtons["Start Game"] != null)
        {
            this.menuButtons["Start Game"].AssignMenu(navigator.GetMenu("Level Select Menu"));
        }
        if (this.menuButtons["Save/Load Game"] != null)
        {
            this.menuButtons["Save/Load Game"].OnClick = SaveLoad;
        }
    }

    private void SaveLoad(object sender, EventArgs args)
    {
        string TestFile = "C:/Projects/jmdTestSave.xml";
        string testFileXML = "";
        if (File.Exists(TestFile))
        {
            testFileXML = JDGameUtilz.LoadXML(TestFile);
            LevelManager.Instance.LoadData(testFileXML);
            WeaponFactory.Instance.LoadData(testFileXML);
            BulletFactory.Instance.LoadData(testFileXML);
            GameStatistics.Instance.LoadData(testFileXML);
        }
        else
        {
            string levelData = LevelManager.Instance.SaveData().Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            string weaponData = WeaponFactory.Instance.SaveData().Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            string bulletData = BulletFactory.Instance.SaveData().Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            string statsData = GameStatistics.Instance.SaveData().Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            JDGameUtilz.CreateXML(TestFile, "<root>" + levelData + weaponData + bulletData + statsData + "</root>");
        }
    }
}

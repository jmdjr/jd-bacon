using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.IO;

public class LoadGameMenu : JDMenu
{
    string saveFileName = "";
    private void SaveLoad()
    {
        string root = "C:/GameSaves/";
        
        string testFileXML = "";

        if (!Directory.Exists(root))
        {
            Directory.CreateDirectory(root);
        }

        root += saveFileName;

        if (File.Exists(root))
        {
            testFileXML = JDGameUtilz.LoadXML(root);
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
            JDGameUtilz.CreateXML(root, "<root>" + levelData + weaponData + bulletData + statsData + "</root>");
        }
    }

    public override void RegisterTouchingEvents()
    {
        base.RegisterTouchingEvents();
    }

    public override void UnregisterTouchingEvents()
    {
        base.UnregisterTouchingEvents();
    }

    public override void MenuUpdate()
    {
    }

    public override void AssignButtonMenus()
    {
        if (this.menuButtons["Save File 1"] != null)
        {
            this.menuButtons["Save File 1"].OnClick = SaveFile;
        }
        if (this.menuButtons["Save File 2"] != null)
        {
            this.menuButtons["Save File 2"].OnClick = SaveFile;
        }
        if (this.menuButtons["Save File 3"] != null)
        {
            this.menuButtons["Save File 3"].OnClick = SaveFile;
        }
    }

    public void SaveFile(object sender, EventArgs args)
    {
        JDMenuButton saveButton = sender as JDMenuButton;

        saveFileName = sender.ToString();
        saveFileName = saveFileName.Substring(0, saveFileName.IndexOf("(") - 1) + ".xml";
        SaveLoad();

        if (saveButton != null)
        {
            DynamicText savefileText = DynamicText.GetTextMesh(saveButton);
            savefileText.SetText("PLAYER SAVED!");
        }

        saveFileName = "";
    }
}
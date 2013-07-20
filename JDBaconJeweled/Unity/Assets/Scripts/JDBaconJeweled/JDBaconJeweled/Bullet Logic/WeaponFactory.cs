using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;


public class WeaponFactory : JDISavableObject
{
    string WeaponDefinitionFile = "./Assets/Definitions/WeaponDefinitions.xml";

    private List<JDWeapon> WeaponReferences = new List<JDWeapon>(); // preloaded list of all weapons.
    private List<JDWeapon> WeaponCollection = new List<JDWeapon>(); // currently available weapons.
    private LevelManager levelManager = LevelManager.Instance;
    private static WeaponFactory instance;
    public static WeaponFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WeaponFactory();
            }
            return instance;
        }
    }

    public string Name { get; set; }
    public JDIObjectTypes JDType { get { return JDIObjectTypes.OBJECT; } }
    public bool ReportStatistics(JDIStatTypes stat, int valueShift) { return false; }

    private WeaponFactory()
    {
        WeaponReferences = (List<JDWeapon>)JDGameUtilz.DeserializeObject(JDGameUtilz.LoadXML(WeaponDefinitionFile),
            "WeaponDefinitions", typeof(List<JDWeapon>), JDGameUtilz.EncodingType.UTF8);

        // load initially available weapons
        CollectAvailableWeapons();
    }
    
    public void CollectAvailableWeapons()
    {
        this.WeaponCollection.Clear();

        foreach (JDWeapon weapon in WeaponReferences)
        {
            if (weapon.Unlocked && levelManager.CurrentLevel() >= weapon.AccessedLevel)
            {
                WeaponCollection.Add(weapon);
            }

            // then add the ones made available to user through his save file.
        }
    }

    public IList<JDWeapon> AvailableWeapons()
    {
        return WeaponCollection.AsReadOnly();
    }

    public JDWeapon GetReference(int Id)
    {
        JDWeapon reference = null;
        if (Id > 0 && Id <= this.WeaponReferences.Max(w => w.Id))
        {
            reference = this.WeaponReferences.FirstOrDefault(b => b.Id == Id);
        }

        return reference;
    }

    public string SaveData()
    {
        return JDGameUtilz.SerializeObject(WeaponReferences, "WeaponDefinitions", typeof(List<JDWeapon>));
    }

    public void LoadData(string savefiletext)
    {
        int rootStart = savefiletext.IndexOf("<WeaponDefinitions>");
        int rootEnd = savefiletext.IndexOf("</WeaponDefinitions>") + "</WeaponDefinitions>".Length;
        string partialText = savefiletext.Substring(rootStart, rootEnd - rootStart);
        WeaponReferences = (List<JDWeapon>)JDGameUtilz.DeserializeObject(partialText, "WeaponDefinitions", typeof(List<JDWeapon>), JDGameUtilz.EncodingType.UTF8);
    }

}

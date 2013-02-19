using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;


public class WeaponFactory
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

    private WeaponFactory()
    {
        WeaponReferences = (List<JDWeapon>)JDGameUtilz.DeserializeObject(JDGameUtilz.LoadXML(WeaponDefinitionFile),
            "WeaponDefinitions", typeof(List<JDWeapon>), JDGameUtilz.EncodingType.UTF8);

        // string Serial = JDGameUtilz.SerializeObject(WeaponReferences, "WeaponDefinitions", typeof(List<JDWeapon>));

        // load initially available weapons
        CollectAvailableWeapons();

        Debug.Log("weapon count: " + this.WeaponReferences.Count);
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
}

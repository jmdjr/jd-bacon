
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

[Serializable]
public class JDCharacter : JDICharacter
{
    #region Variables
    [SerializeField]
    private String _name;
    #endregion

    #region Properties
    public string Name { get; set; }
    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.CHARACTER; }
    }

    public JDIAnimator Animator
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }
    public int MaxHitPoints
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }
    public int HitPoints
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }
    public int CollisionDamage
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }
    public int InflictingDamage()
    {
        throw new NotImplementedException();
    }

    public Event WasHitWithWeapon(JDICharacter other, JDIWeapon weapon)
    {
        throw new NotImplementedException();
    }
}
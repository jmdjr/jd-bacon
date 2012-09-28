
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
    private String name;
    [SerializeField]
    private int maxHitPoints;
    [SerializeField]
    private int hitPoints;
    [SerializeField]
    private int collisionDamage;
    [SerializeField]
    private JDIAnimator animator;
    #endregion

    #region Properties
    public JDIObjectTypes JDType { get { return JDIObjectTypes.CHARACTER; } }
    public string Name { get { return this.name; } set { this.name = value; } }
    public int MaxHitPoints { get { return this.maxHitPoints; } set { this.maxHitPoints = value; } }
    public int HitPoints { get { return this.hitPoints; } set { this.hitPoints = value; } }
    public int CollisionDamage { get { return this.collisionDamage; } set { this.collisionDamage = value; } }
    public JDIAnimator Animator { get { return this.animator; } set { this.animator = value; } }
    #endregion

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        throw new NotImplementedException();
    }

    public void UpdateHealth(int amount)
    {
        this.HitPoints += amount;
    }
    public virtual int InflictingDamage() { return this.CollisionDamage; }

    public Event WasHitWithWeapon(JDICharacter other, JDIWeapon weapon)
    {
        throw new NotImplementedException();
    }
}